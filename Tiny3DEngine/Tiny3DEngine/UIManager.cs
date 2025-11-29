using ImGuiNET;
using OpenTK.Mathematics;

public class UIManager
{
    public enum MenuState
    {
        MainMenu,
        Settings,
        None
    }

    public MenuState State = MenuState.MainMenu;

    private string[] resolutions = 
    {
        "1280x720",
        "1600x900",
        "1920x1080",
        "2560x1440",
        "3840x2160"
    };

    private int selectedResolution = 2;
    private int targetFPS = 60;

    public bool PlayPressed = false;

    public void Render()
    {
        if (State == MenuState.None)
            return;

        ImGui.Begin("RustBucket Engine Menu",
            ImGuiWindowFlags.NoDecoration |
            ImGuiWindowFlags.NoMove |
            ImGuiWindowFlags.AlwaysAutoResize |
            ImGuiWindowFlags.NoBackground);

        // Center window
        ImGui.SetWindowPos(new System.Numerics.Vector2(50, 50));

        // Title
        ImGui.PushStyleColor(ImGuiCol.Text, new System.Numerics.Vector4(1, 0.6f, 0.2f, 1));
        ImGui.Text("RustBucket Engine");
        ImGui.PopStyleColor();

        ImGui.Separator();

        if (State == MenuState.MainMenu)
        {
            if (ImGui.Button("Play", new System.Numerics.Vector2(200, 40)))
            {
                PlayPressed = true;
                State = MenuState.None;
            }

            if (ImGui.Button("Settings", new System.Numerics.Vector2(200, 40)))
            {
                State = MenuState.Settings;
            }
        }
        else if (State == MenuState.Settings)
        {
            ImGui.Text("Resolution:");
            ImGui.Combo("##resolution", ref selectedResolution, resolutions, resolutions.Length);

            ImGui.Text("Target FPS:");
            ImGui.InputInt("##fps", ref targetFPS);

            ImGui.Spacing();

            if (ImGui.Button("Back", new System.Numerics.Vector2(200, 40)))
                State = MenuState.MainMenu;
        }

        ImGui.End();
    }

    public (int width, int height, int fps) GetSettings()
    {
        string[] parts = resolutions[selectedResolution].Split('x');
        return (int.Parse(parts[0]), int.Parse(parts[1]), targetFPS);
    }
}
