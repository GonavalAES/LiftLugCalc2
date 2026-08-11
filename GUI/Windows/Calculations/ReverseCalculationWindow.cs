using ImGuiNET;

using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.GUI.Windows.Calculations;

public static class ReverseCalculationWindow
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;
        Project project = session.CurrentProject!;

        GuiCommon.SectionHeader("Run Reverse Calculation");

        GuiCommon.Spacer();

        //--------------------------------------------------------
        // Project
        //--------------------------------------------------------

        ImGui.Text($"Project : {project.Name}");

        //--------------------------------------------------------
        // Calculation
        //--------------------------------------------------------

        ImGui.Text("Calculation : Reverse Calculation");

        //--------------------------------------------------------
        // Material
        //--------------------------------------------------------

        if (session.SelectedMaterial != null)
        {
            ImGui.Text(
                $"Material : {session.SelectedMaterial.Designation}");
        }
        else
        {
            ImGui.TextDisabled("Material : Not selected");
        }

        //--------------------------------------------------------
        // Applied load
        //--------------------------------------------------------

        ImGui.Text(
            $"WLL : {project.WLL:N2} kg");

        //--------------------------------------------------------
        // Lifting points
        //--------------------------------------------------------

        ImGui.Text(
            $"Lifting points : {project.NumberPoints}");

        GuiCommon.Spacer();

        ImGui.Separator();

        GuiCommon.Spacer();

        //--------------------------------------------------------
        // Explanation
        //--------------------------------------------------------

        ImGui.TextWrapped(
            "The reverse calculation will evaluate the available "
          + "lug types and identify the smallest lug that satisfies "
          + "the required capacity and engineering checks.");

        GuiCommon.Spacer();

        ImGui.Text(
            $"Available lug types : {AppState.Lugs.Count}");

        GuiCommon.Spacer();

        ImGui.TextWrapped(
            "Review the engineering data above. "
          + "Press \"Run Calculation\" to perform the reverse calculation.");
    }
}
