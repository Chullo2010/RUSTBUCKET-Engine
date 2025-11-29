using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.GraphicsLibraryFramework;
using System;
using System.Collections.Generic;

public class MenuUI
{
    private Game game;

    private int shaderProgram;
    private int vao, vbo, ebo;

    private float[] quadVertices = { 0f, 0f, 1f, 0f, 1f, 1f, 0f, 1f };
    private uint[] quadIndices = { 0, 1, 2, 2, 3, 0 };

    private struct Button { public Vector2 pos; public Vector2 size; public string label; }
    private Button playButton;
    private Button settingsButton;
    private Button backButton;

    // --- BITMAP FONT ---
    private Dictionary<char, byte[]> font = new Dictionary<char, byte[]>();

    public MenuUI(Game game)
    {
        this.game = game;

        float w = game.Size.X;
        float h = game.Size.Y;

        playButton = new Button { pos = new Vector2(w / 2 - 125, h / 2 + 50), size = new Vector2(250, 60), label = "Play" };
        settingsButton = new Button { pos = new Vector2(w / 2 - 125, h / 2 - 50), size = new Vector2(250, 60), label = "Settings" };
        backButton = new Button { pos = new Vector2(20, 20), size = new Vector2(150, 50), label = "Back" };

        SetupShader();
        SetupBuffers();
        SetupFont();
    }

    private void SetupShader()
    {
        string vertexSrc = @"
#version 330 core
layout(location = 0) in vec2 aPos;
uniform vec2 uPosition;
uniform vec2 uSize;
uniform vec2 uScreen;
void main()
{
    vec2 pos = uPosition + aPos * uSize;
    vec2 clip = pos / uScreen * 2.0 - 1.0;
    gl_Position = vec4(clip.x, -clip.y, 0.0, 1.0);
}";
        string fragmentSrc = @"
#version 330 core
out vec4 FragColor;
uniform vec3 uColor;
void main()
{
    FragColor = vec4(uColor,1.0);
}";
        int v = GL.CreateShader(OpenTK.Graphics.OpenGL4.ShaderType.VertexShader);
        GL.ShaderSource(v, vertexSrc);
        GL.CompileShader(v);

        int f = GL.CreateShader(OpenTK.Graphics.OpenGL4.ShaderType.FragmentShader);
        GL.ShaderSource(f, fragmentSrc);
        GL.CompileShader(f);

        shaderProgram = GL.CreateProgram();
        GL.AttachShader(shaderProgram, v);
        GL.AttachShader(shaderProgram, f);
        GL.LinkProgram(shaderProgram);
        GL.DeleteShader(v);
        GL.DeleteShader(f);
    }

    private void SetupBuffers()
    {
        vao = GL.GenVertexArray();
        vbo = GL.GenBuffer();
        ebo = GL.GenBuffer();

        GL.BindVertexArray(vao);
        GL.BindBuffer(BufferTarget.ArrayBuffer, vbo);
        GL.BufferData(BufferTarget.ArrayBuffer, quadVertices.Length * sizeof(float), quadVertices, BufferUsageHint.StaticDraw);

        GL.BindBuffer(BufferTarget.ElementArrayBuffer, ebo);
        GL.BufferData(BufferTarget.ElementArrayBuffer, quadIndices.Length * sizeof(uint), quadIndices, BufferUsageHint.StaticDraw);

        GL.VertexAttribPointer(0, 2, VertexAttribPointerType.Float, false, 2 * sizeof(float), 0);
        GL.EnableVertexAttribArray(0);
    }

    private void SetupFont()
    {
        // Simple 5x5 pixel letters for A-Z, 0-9, etc.
        // Each byte is a row, LSB = leftmost pixel
        font['A'] = new byte[] { 0b01000,0b10100,0b11100,0b10100,0b10100 };
font['B'] = new byte[] { 0b11000,0b10100,0b11000,0b10100,0b11000 };
font['C'] = new byte[] { 0b01110,0b10000,0b10000,0b10000,0b01110 };
font['D'] = new byte[] { 0b11000,0b10100,0b10100,0b10100,0b11000 };
font['E'] = new byte[] { 0b11110,0b10000,0b11100,0b10000,0b11110 };
font['F'] = new byte[] { 0b11110,0b10000,0b11100,0b10000,0b10000 };
font['G'] = new byte[] { 0b01110,0b10000,0b10110,0b10010,0b01110 };
font['H'] = new byte[] { 0b10100,0b10100,0b11100,0b10100,0b10100 };
font['I'] = new byte[] { 0b11100,0b01000,0b01000,0b01000,0b11100 };
font['J'] = new byte[] { 0b00110,0b00010,0b00010,0b10010,0b01100 };
font['K'] = new byte[] { 0b10100,0b10100,0b11000,0b10100,0b10100 };
font['L'] = new byte[] { 0b10000,0b10000,0b10000,0b10000,0b11100 };
font['M'] = new byte[] { 0b10001,0b11011,0b10101,0b10001,0b10001 };
font['N'] = new byte[] { 0b10001,0b11001,0b10101,0b10011,0b10001 };
font['O'] = new byte[] { 0b01110,0b10001,0b10001,0b10001,0b01110 };
font['P'] = new byte[] { 0b11100,0b10100,0b11100,0b10000,0b10000 };
font['Q'] = new byte[] { 0b01110,0b10001,0b10001,0b10011,0b01111 };
font['R'] = new byte[] { 0b11100,0b10100,0b11100,0b10100,0b10100 };
font['S'] = new byte[] { 0b01110,0b10000,0b01100,0b00010,0b11100 };
font['T'] = new byte[] { 0b11100,0b01000,0b01000,0b01000,0b01000 };
font['U'] = new byte[] { 0b10100,0b10100,0b10100,0b10100,0b11100 };
font['V'] = new byte[] { 0b10100,0b10100,0b10100,0b10100,0b01000 };
font['W'] = new byte[] { 0b10001,0b10001,0b10101,0b11011,0b10001 };
font['X'] = new byte[] { 0b10100,0b10100,0b01000,0b10100,0b10100 };
font['Y'] = new byte[] { 0b10100,0b10100,0b01000,0b01000,0b01000 };
font['Z'] = new byte[] { 0b11110,0b00010,0b00100,0b01000,0b11110 };
font[' '] = new byte[] {0,0,0,0,0};

    }

    public void UpdateMenu(MouseState mouse)
    {
        if (mouse.IsButtonPressed(MouseButton.Left))
        {
            Vector2 m = new Vector2(mouse.X, game.Size.Y - mouse.Y);

            if (PointInRect(m, playButton)) game.StartGame();
            if (PointInRect(m, settingsButton)) game.OpenSettings();
        }
    }

    public void UpdateSettings(MouseState mouse)
    {
        if (mouse.IsButtonPressed(MouseButton.Left))
        {
            Vector2 m = new Vector2(mouse.X, game.Size.Y - mouse.Y);

            if (PointInRect(m, backButton)) game.ReturnToMenu();
        }
    }

    private bool PointInRect(Vector2 mouse, Button btn)
    {
        return mouse.X >= btn.pos.X && mouse.X <= btn.pos.X + btn.size.X &&
               mouse.Y >= btn.pos.Y && mouse.Y <= btn.pos.Y + btn.size.Y;
    }

    public void RenderMenu()
    {
        DrawButton(playButton, new Vector3(0.2f, 0.8f, 0.3f));
        DrawText(playButton.label, playButton.pos + new Vector2(20, 15), new Vector3(1f, 1f, 1f), 8f);

        DrawButton(settingsButton, new Vector3(0.8f, 0.2f, 0.2f));
        DrawText(settingsButton.label, settingsButton.pos + new Vector2(20, 15), new Vector3(1f, 1f, 1f), 8f);
    }

    public void RenderSettings()
    {
        DrawButton(backButton, new Vector3(0.2f, 0.2f, 0.8f));
        DrawText(backButton.label, backButton.pos + new Vector2(20, 10), new Vector3(1f, 1f, 1f), 6f);
    }

    private void DrawButton(Button btn, Vector3 color)
    {
        GL.UseProgram(shaderProgram);
        GL.BindVertexArray(vao);

        GL.Uniform2(GL.GetUniformLocation(shaderProgram, "uPosition"), btn.pos);
        GL.Uniform2(GL.GetUniformLocation(shaderProgram, "uSize"), btn.size);
        GL.Uniform2(GL.GetUniformLocation(shaderProgram, "uScreen"), new Vector2(game.Size.X, game.Size.Y));
        GL.Uniform3(GL.GetUniformLocation(shaderProgram, "uColor"), color);

        GL.DrawElements(PrimitiveType.Triangles, quadIndices.Length, DrawElementsType.UnsignedInt, 0);
    }

    // --- DRAW TEXT ---
    private void DrawText(string text, Vector2 pos, Vector3 color, float scale)
    {
        float startX = pos.X;

        foreach (char c in text.ToUpper())
        {
            if (!font.ContainsKey(c)) continue;
            byte[] bitmap = font[c];

            for (int y = 0; y < bitmap.Length; y++)
            {
                for (int x = 0; x < 5; x++)
                {
                    if (((bitmap[y] >> (4 - x)) & 1) == 1)
                    {
                        Vector2 p = new Vector2(pos.X + x * scale, pos.Y + y * scale);
                        DrawPixel(p, scale, color);
                    }
                }
            }
            pos.X += 6 * scale; // letter spacing
        }
    }

    private void DrawPixel(Vector2 pos, float size, Vector3 color)
    {
        GL.UseProgram(shaderProgram);
        GL.BindVertexArray(vao);

        GL.Uniform2(GL.GetUniformLocation(shaderProgram, "uPosition"), pos);
        GL.Uniform2(GL.GetUniformLocation(shaderProgram, "uSize"), new Vector2(size, size));
        GL.Uniform2(GL.GetUniformLocation(shaderProgram, "uScreen"), new Vector2(game.Size.X, game.Size.Y));
        GL.Uniform3(GL.GetUniformLocation(shaderProgram, "uColor"), color);

        GL.DrawElements(PrimitiveType.Triangles, quadIndices.Length, DrawElementsType.UnsignedInt, 0);
    }
}
