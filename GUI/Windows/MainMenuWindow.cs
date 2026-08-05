using ImGuiNET;

namespace LiftLugCalc2.GUI.Windows;

public static class MainMenuWindow
{
    public static void Render(Session state)
    {
        ImGui.SetNextWindowPos(new System.Numerics.Vector2(20, 20), ImGuiCond.Once);
        ImGui.SetNextWindowSize(new System.Numerics.Vector2(350, 250), ImGuiCond.Once);

        ImGui.Begin("Lift Lug Calculator");

        ImGui.SeparatorText("Main Menu");

        if (ImGui.Button("New Project", new System.Numerics.Vector2(200, 40)))
        {
            state.CurrentScreen = Screen.NewProject;
        }

        ImGui.Spacing();

        if (ImGui.Button("Open Project", new System.Numerics.Vector2(200, 40)))
        {
            state.CurrentScreen = Screen.OpenProject;
        }

        ImGui.Spacing();

        if (ImGui.Button("Exit", new System.Numerics.Vector2(200, 40)))
        {
            Environment.Exit(0);
        }

        ImGui.End();
    }
}
