using OpenTK.Windowing.Desktop;

namespace Tiny3DEngine
{
    internal class Program
    {
        static void Main()
        {
            var gameSettings = new GameWindowSettings
            {
                RenderFrequency = 60,
                UpdateFrequency = 60
            };

            var nativeSettings = new NativeWindowSettings
            {
                Title = "Tiny3DEngine",
                Size = new OpenTK.Mathematics.Vector2i(800, 600)
            };

            using (var game = new Game(gameSettings, nativeSettings))
            {
                game.Run();
            }
        }
    }
}
