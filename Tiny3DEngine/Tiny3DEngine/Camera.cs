using OpenTK.Mathematics;

namespace Tiny3DEngine
{
    public class Camera
    {
        public Vector3 Position;
        public Vector3 Front = -Vector3.UnitZ;
        public Vector3 Up = Vector3.UnitY;
        public Vector3 Right => Vector3.Cross(Front, Up).Normalized();

        public float Fov = 60f;
        public float AspectRatio = 4f / 3f;
        public float Near = 0.1f;
        public float Far = 100f;

        public Camera(Vector3 position)
        {
            Position = position;
        }

        public Matrix4 GetViewMatrix()
        {
            return Matrix4.LookAt(Position, Position + Front, Up);
        }

        public Matrix4 GetProjectionMatrix()
        {
            return Matrix4.CreatePerspectiveFieldOfView(MathHelper.DegreesToRadians(Fov), AspectRatio, Near, Far);
        }
    }
}
