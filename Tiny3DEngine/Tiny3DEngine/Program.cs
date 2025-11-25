using OpenTK.Windowing.Desktop;

class Program
{
    static void Main()
    {
        var nativeSettings = new NativeWindowSettings()
        {
            Size = new OpenTK.Mathematics.Vector2i(1280, 720),
            Title = "My Engine"
        };

        using var window = new Game(GameWindowSettings.Default, nativeSettings);
        window.Run();
    }
}
