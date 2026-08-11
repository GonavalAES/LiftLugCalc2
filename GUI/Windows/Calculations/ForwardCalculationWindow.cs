using ImGuiNET;

using LiftLugCalc2.GUI.Helpers;

namespace LiftLugCalc2.GUI.Windows.Calculations;

public static class ForwardCalculationWindow
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;

        GuiCommon.SectionHeader("Run Forward Calculation");

        GuiCommon.Spacer();

        ImGui.Text($"Project : {session.CurrentProject!.Name}");

        ImGui.Text($"Material : {session.SelectedMaterial!.Designation}");

        ImGui.Text($"Lug : {LugPresentation.GetLugTypeName(session.SelectedLug!.LugType)}");

        ImGui.Text($"WLL : {session.CurrentProject.WLL:N0} kg");

        ImGui.Text($"Lifting points : {session.CurrentProject.NumberPoints}");

        GuiCommon.Spacer();

        ImGui.Separator();

        GuiCommon.Spacer();

        ImGui.TextWrapped(
            "Review the engineering data above. "
          + "If everything is correct, press "
          + "\"Run Calculation\".");
    }
}
