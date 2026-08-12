using ImGuiNET;

using LiftLugCalc2.GUI.Windows.Navigation;

using System.Numerics;

namespace LiftLugCalc2.GUI.Windows;

public static class MainWindow
{
    public static void Render(GuiController controller)
    {
        // Occupy the entire application client area.
        Vector2 displaySize = ImGui.GetIO().DisplaySize;

        ImGui.SetNextWindowPos(Vector2.Zero);
        ImGui.SetNextWindowSize(displaySize);
        ImGui.Begin("Lift Lug Calculator", ImGuiWindowFlags.NoTitleBar |
                                           ImGuiWindowFlags.NoResize |
                                           ImGuiWindowFlags.NoMove |
                                           ImGuiWindowFlags.NoCollapse);

        // Calculate shell dimensions.
        Vector2 available = ImGui.GetContentRegionAvail();

        float navigationWidth = GuiConstants.NavigationWidth;
        float statusHeight = GuiConstants.StatusBarHeight;
        float commandHeight = GuiConstants.CommandBarHeight;
        float contentHeight = available.Y - statusHeight - commandHeight - GuiConstants.Padding * 2.0f;

        // Navigation panel
        GuiCommon.BeginPanel("Navigation", new Vector2(navigationWidth, contentHeight));
        NavigationPanel.Render(controller);
        GuiCommon.EndPanel();
        ImGui.SameLine();

        // Content panel
        GuiCommon.BeginPanel("Content", new Vector2(0, contentHeight));
        controller.RenderCurrentPage();
        GuiCommon.EndPanel();

        // Status bar
        GuiCommon.BeginPanel("Status", new Vector2(0, statusHeight));
        StatusBar.Render(controller);
        GuiCommon.EndPanel();

        // Command bar
        GuiCommon.BeginPanel("Commands", new Vector2(0, commandHeight));
        CommandBar.Render(controller);
        GuiCommon.EndPanel();
        ImGui.End();
    }
}
