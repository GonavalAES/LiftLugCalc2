using ImGuiNET;

namespace LiftLugCalc2.GUI.Windows.ProjectSetups;

public static class OpenProjectWindow
{
    public static void Render(Session state)
    {
        ImGui.Begin("Open Project");

        ImGui.Text("Open existing project");

        if (ImGui.Button("Back"))
        {
            state.CurrentScreen = Screen.MainMenu;
        }

        ImGui.End();
    }
}
