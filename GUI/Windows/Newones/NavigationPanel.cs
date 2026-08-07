using ImGuiNET;

namespace LiftLugCalc2.GUI.Windows.Newones;

public static class NavigationPanel
{
    /// <summary>
    /// Draws the application navigation panel.
    /// </summary>
    public static void Render(GuiController controller)
    {
        GuiCommon.SectionHeader("Project");

        ImGui.BulletText("Project Setup");
        ImGui.BulletText("Weight");
        ImGui.BulletText("Lift Geometry");
        ImGui.BulletText("Calculation Mode");
        ImGui.BulletText("Material");
        ImGui.BulletText("Lug");
        ImGui.BulletText("Run");
        ImGui.BulletText("Results");
    }
}
