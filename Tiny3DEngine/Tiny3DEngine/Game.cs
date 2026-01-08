using OpenTK.Graphics.OpenGL4;
using OpenTK.Mathematics;
using OpenTK.Windowing.Common;
using OpenTK.Windowing.Desktop;
using OpenTK.Windowing.GraphicsLibraryFramework;

namespace Tiny3DEngine
{
    public class Game : GameWindow
    {
        private Camera camera;

        private float rotationAngle = 0f;

        public Game(GameWindowSettings gameWindowSettings, NativeWindowSettings nativeWindowSettings)
            : base(gameWindowSettings, nativeWindowSettings)
        {
        }

        protected override void OnLoad()
        {
            base.OnLoad();

            GL.ClearColor(0.1f, 0.1f, 0.1f, 1.0f);
            GL.Enable(EnableCap.DepthTest);

            // Initialize the camera
            camera = new Camera(Vector3.UnitZ * 3f, Size.X / (float)Size.Y);

            // Hide the cursor for first-person style control
            CursorState = CursorState.Grabbed;
        }

        protected override void OnUpdateFrame(FrameEventArgs args)
        {
            base.OnUpdateFrame(args);

            var input = KeyboardState;

            const float cameraSpeed = 2.5f;
            const float sensitivity = 0.2f;

            // Close the window
            if (input.IsKeyDown(Keys.Escape))
                Close();

            // WASD movement
            if (input.IsKeyDown(Keys.W))
                camera.Position += camera.Front * cameraSpeed * (float)args.Time;
            if (input.IsKeyDown(Keys.S))
                camera.Position -= camera.Front * cameraSpeed * (float)args.Time;
            if (input.IsKeyDown(Keys.A))
                camera.Position -= camera.Right * cameraSpeed * (float)args.Time;
            if (input.IsKeyDown(Keys.D))
                camera.Position += camera.Right * cameraSpeed * (float)args.Time;

            // Simple rotation animation for the sphere
            rotationAngle += 50f * (float)args.Time; // degrees per second
        }

        protected override void OnRenderFrame(FrameEventArgs args)
        {
            base.OnRenderFrame(args);

            GL.Clear(ClearBufferMask.ColorBufferBit | ClearBufferMask.DepthBufferBit);

            // Setup basic MVP matrix
            Matrix4 model = Matrix4.CreateRotationY(MathHelper.DegreesToRadians(rotationAngle));
            Matrix4 view = camera.GetViewMatrix();
            Matrix4 projection = camera.GetProjectionMatrix();

            GL.MatrixMode(MatrixMode.Projection);
            GL.LoadMatrix(ref projection);

            GL.MatrixMode(MatrixMode.Modelview);
            GL.LoadMatrix(ref view * model);

            RenderSphere();

            SwapBuffers();
        }

        private void RenderSphere()
        {
            int latitudeBands = 30;
            int longitudeBands = 30;
            float radius = 1.0f;

            GL.Begin(PrimitiveType.Quads);
            for (int latNumber = 0; latNumber < latitudeBands; latNumber++)
            {
                float theta = latNumber * MathF.PI / latitudeBands;
                float thetaNext = (latNumber + 1) * MathF.PI / latitudeBands;

                for (int longNumber = 0; longNumber < longitudeBands; longNumber++)
                {
                    float phi = longNumber * 2 * MathF.PI / longitudeBands;
                    float phiNext = (longNumber + 1) * 2 * MathF.PI / longitudeBands;

                    Vector3 v1 = SphericalToCartesian(radius, theta, phi);
                    Vector3 v2 = SphericalToCartesian(radius, thetaNext, phi);
                    Vector3 v3 = SphericalToCartesian(radius, thetaNext, phiNext);
                    Vector3 v4 = SphericalToCartesian(radius, theta, phiNext);

                    GL.Color3(0.4f, 0.7f, 1.0f);
                    GL.Normal3(v1);
                    GL.Vertex3(v1);
                    GL.Normal3(v2);
                    GL.Vertex3(v2);
                    GL.Normal3(v3);
                    GL.Vertex3(v3);
                    GL.Normal3(v4);
                    GL.Vertex3(v4);
                }
            }
            GL.End();
        }

        private Vector3 SphericalToCartesian(float r, float theta, float phi)
        {
            float x = r * MathF.Sin(theta) * MathF.Cos(phi);
            float y = r * MathF.Cos(theta);
            float z = r * MathF.Sin(theta) * MathF.Sin(phi);
            return new Vector3(x, y, z);
        }

        protected override void OnResize(ResizeEventArgs e)
        {
            base.OnResize(e);
            GL.Viewport(0, 0, e.Width, e.Height);
            camera.AspectRatio = e.Width / (float)e.Height;
        }
    }
}
