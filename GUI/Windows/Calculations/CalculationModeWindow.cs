using ImGuiNET;

using LiftLugCalc2.GUI.Enums;

namespace LiftLugCalc2.GUI.Windows.Calculations;

public static class CalculationModeWindow
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;

        GuiCommon.SectionHeader("Calculation Mode");

        GuiCommon.Spacer();

        ImGui.Text("Select the type of calculation to perform:");

        GuiCommon.Spacer();

        CalculationMode mode = session.CalculationMode;

        bool forward = mode == CalculationMode.Forward;
        bool reverse = mode == CalculationMode.Reverse;

        if (ImGui.RadioButton(
            "Forward Calculation",
            forward))
        {
            session.CalculationMode = CalculationMode.Forward;
        }

        if (ImGui.RadioButton(
            "Reverse Calculation",
            reverse))
        {
            session.CalculationMode = CalculationMode.Reverse;
        }

        GuiCommon.Spacer();

        ImGui.Separator();

        GuiCommon.Spacer();

        if (session.CalculationMode == CalculationMode.Forward)
        {
            ImGui.TextWrapped(
                "Checks a selected lug against the specified load.");
        }
        else
        {
            ImGui.TextWrapped(
                "Suggests the smallest suitable lug for the specified load.");
        }
    }
}
