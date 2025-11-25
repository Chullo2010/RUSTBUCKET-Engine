using OpenTK.Mathematics;

public class Camera
{
    public Vector3 Position;
    public float Pitch;   // X rotation
    public float Yaw;     // Y rotation
    public float Speed = 3.0f;
    public float Sensitivity = 0.2f;
    public bool firstMove = true;
    public Vector2 lastMousePos;

    public Camera(Vector3 position)
    {
        Position = position;
        Pitch = 0;
        Yaw = -90f; // Looks forward along -Z
    }

    public Matrix4 GetViewMatrix()
    {
        Vector3 direction = GetForwardVector();
        return Matrix4.LookAt(Position, Position + direction, Vector3.UnitY);
    }

    public Matrix4 GetProjectionMatrix(float aspectRatio)
    {
        return Matrix4.CreatePerspectiveFieldOfView(
            MathHelper.DegreesToRadians(70f),
            aspectRatio,
            0.1f,
            100f
        );
    }

    public void MoveForward(float delta)
    {
        Position += GetForwardVector() * Speed * delta;
    }

    public void MoveBackward(float delta)
    {
        Position -= GetForwardVector() * Speed * delta;
    }

    public void MoveRight(float delta)
    {
        Position += GetRightVector() * Speed * delta;
    }

    public void MoveLeft(float delta)
    {
        Position -= GetRightVector() * Speed * delta;
    }

    public void Rotate(float deltaX, float deltaY)
    {
        Yaw += deltaX * Sensitivity;
        Pitch -= deltaY * Sensitivity;

        Pitch = MathHelper.Clamp(Pitch, -89f, 89f);
    }

    private Vector3 GetForwardVector()
    {
        float pitchRad = MathHelper.DegreesToRadians(Pitch);
        float yawRad = MathHelper.DegreesToRadians(Yaw);

        return new Vector3(
            MathF.Cos(pitchRad) * MathF.Cos(yawRad),
            MathF.Sin(pitchRad),
            MathF.Cos(pitchRad) * MathF.Sin(yawRad)
        ).Normalized();
    }

    private Vector3 GetRightVector()
    {
        return Vector3.Normalize(Vector3.Cross(GetForwardVector(), Vector3.UnitY));
    }
}
