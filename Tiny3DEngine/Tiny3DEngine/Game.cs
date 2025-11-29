using OpenTK.Graphics.OpenGL4;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;
using OpenTK.Mathematics;
using System;
using System.IO;

public class Game : GameWindow
{
    private enum GameState { Menu, Settings, Playing }
    private GameState currentState = GameState.Menu;

    private MenuUI menuUI;

    private int VBO, VAO, EBO, shaderProgram;

    private Camera camera;
    private float rotation = 0f;

    private float[] vertices =
    {
        -0.5f, -0.5f, -0.5f, 1f,0f,0f,
         0.5f, -0.5f, -0.5f, 0f,1f,0f,
         0.5f, 0.5f, -0.5f, 0f,0f,1f,
        -0.5f, 0.5f, -0.5f, 1f,1f,0f,
        -0.5f, -0.5f, 0.5f, 1f,0f,1f,
         0.5f, -0.5f, 0.5f, 0f,1f,1f,
         0.5f, 0.5f, 0.5f, 1f,1f,1f,
        -0.5f, 0.5f, 0.5f, 0f,0f,0f
    };

    private uint[] indices =
    {
        0,1,2,2,3,0,
        4,5,6,6,7,4,
        0,4,7,7,3,0,
        1,5,6,6,2,1,
        0,1,5,5,4,0,
        3,2,6,6,7,3
    };

    public Game(GameWindowSettings gws, NativeWindowSettings nws)
        : base(gws, nws) { }

    protected override void OnLoad()
    {
        base.OnLoad();

        menuUI = new MenuUI(this);
        currentState = GameState.Menu;

        GL.ClearColor(0.1f, 0.1f, 0.1f, 1f);

        camera = new Camera(new Vector3(0f,0f,3f));

        int vertexShader = GL.CreateShader(ShaderType.VertexShader);
        GL.ShaderSource(vertexShader, File.ReadAllText("shader.vert"));
        GL.CompileShader(vertexShader);

        int fragmentShader = GL.CreateShader(ShaderType.FragmentShader);
        GL.ShaderSource(fragmentShader, File.ReadAllText("shader.frag"));
        GL.CompileShader(fragmentShader);

        shaderProgram = GL.CreateProgram();
        GL.AttachShader(shaderProgram, vertexShader);
        GL.AttachShader(shaderProgram, fragmentShader);
        GL.LinkProgram(shaderProgram);

        GL.DeleteShader(vertexShader);
        GL.DeleteShader(fragmentShader);

        VAO = GL.GenVertexArray();
        VBO = GL.GenBuffer();
        EBO = GL.GenBuffer();

        GL.BindVertexArray(VAO);
        GL.BindBuffer(BufferTarget.ArrayBuffer, VBO);
        GL.BufferData(BufferTarget.ArrayBuffer, vertices.Length * sizeof(float), vertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, EBO);
        GL.BufferData(BufferTarget.ElementArrayBuffer, indices.Length * sizeof(uint), indices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0,3,VertexAttribPointerType.Float,false,6*sizeof(float),0);
        GL.EnableVertexAttribArray(0);

        GL.VertexAttribPointer(1,3,VertexAttribPointerType.Float,false,6*sizeof(float),3*sizeof(float));
        GL.EnableVertexAttribArray(1);

        GL.Enable(EnableCap.DepthTest);
    }

    protected override void OnUpdateFrame(FrameEventArgs args)
    {
        base.OnUpdateFrame(args);
        if (!IsFocused) return;

        float delta = (float)args.Time;

        if (currentState == GameState.Menu)
        {
            menuUI.UpdateMenu(MouseState);
            return;
        }
        if (currentState == GameState.Settings)
        {
            menuUI.UpdateSettings(MouseState);
            return;
        }

        var input = KeyboardState;

        if (input.IsKeyDown(Keys.W)) camera.MoveForward(delta);
        if (input.IsKeyDown(Keys.S)) camera.MoveBackward(delta);
        if (input.IsKeyDown(Keys.A)) camera.MoveLeft(delta);
        if (input.IsKeyDown(Keys.D)) camera.MoveRight(delta);

        var mouse = MouseState;
        if (camera.firstMove)
        {
            camera.lastMousePos = new Vector2(mouse.X, mouse.Y);
            camera.firstMove = false;
        }
        else
        {
            float deltaX = mouse.X - camera.lastMousePos.X;
            float deltaY = mouse.Y - camera.lastMousePos.Y;
            camera.lastMousePos = new Vector2(mouse.X, mouse.Y);
            camera.Rotate(deltaX, deltaY);
        }
    }

    protected override void OnRenderFrame(FrameEventArgs args)
    {
        base.OnRenderFrame(args);
        GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

        if (currentState == GameState.Menu)
        {
            menuUI.RenderMenu();
            SwapBuffers();
            return;
        }
        if (currentState == GameState.Settings)
        {
            menuUI.RenderSettings();
            SwapBuffers();
            return;
        }

        GL.UseProgram(shaderProgram);
        GL.BindVertexArray(VAO);

        rotation += 50f * (float)args.Time;
        Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotation));
        Matrix4 view = camera.GetViewMatrix();
        Matrix4 projection = camera.GetProjectionMatrix(Size.X / (float)Size.Y);

        int modelLoc = GL.GetUniformLocation(shaderProgram, "model");
        int viewLoc = GL.GetUniformLocation(shaderProgram, "view");
        int projLoc = GL.GetUniformLocation(shaderProgram, "projection");

        GL.UniformMatrix4(modelLoc,false,ref model);
        GL.UniformMatrix4(viewLoc,false,ref view);
        GL.UniformMatrix4(projLoc,false,ref projection);

        GL.DrawElements(PrimitiveType.Triangles, indices.Length, DrawElementsType.UnsignedInt,0);
        SwapBuffers();
    }

    public void StartGame() { currentState = GameState.Playing; CursorState = CursorState.Grabbed; }
    public void OpenSettings() { currentState = GameState.Settings; CursorState = CursorState.Normal; }
    public void ReturnToMenu() { currentState = GameState.Menu; CursorState = CursorState.Normal; }
}
