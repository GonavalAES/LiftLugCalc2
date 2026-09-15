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
        bool exploratory = mode == CalculationMode.Exploratory;

        if (ImGui.RadioButton("Forward Calculation", forward)) session.CalculationMode = CalculationMode.Forward;
        if (ImGui.RadioButton("Reverse Calculation", reverse)) session.CalculationMode = CalculationMode.Reverse;
        if (ImGui.RadioButton("Exploratory Calculation", exploratory)) session.CalculationMode = CalculationMode.Exploratory;

        GuiCommon.Spacer();
        ImGui.Separator();
        GuiCommon.Spacer();

        DrawModeDescription(session.CalculationMode);
    }

    private static void DrawModeDescription(CalculationMode mode)
    {
        switch (mode)
        {
            case CalculationMode.Forward:
                ImGui.TextWrapped("Checks a selected catalogue lug against the specified lifting load.");
                break;
            case CalculationMode.Reverse:
                ImGui.TextWrapped("Evaluates the available catalogue lugs and suggests the smallest suitable lug.");
                break;
            case CalculationMode.Exploratory:
                ImGui.TextWrapped("Starts with a selected reference lug. The user may change one or more lug properties " +
                                  "and verify the resulting custom lug.");
                break;
            default:
                ImGui.TextWrapped("Select a calculation mode.");
                break;
        }
    }
}
