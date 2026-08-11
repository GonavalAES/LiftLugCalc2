using ImGuiNET;

namespace LiftLugCalc2.GUI.Windows.Navigation;

public static class MenuWindow
{
    public static void Render(GuiController controller)
    {
        ImGui.SeparatorText("Main Menu");

        ImGui.Spacing();

        ImGui.TextWrapped(
            "Engineering lifting lug calculations.");

        ImGui.Spacing();

        ImGui.Text(
            "Select an operation from the navigation panel.");
    }
}
