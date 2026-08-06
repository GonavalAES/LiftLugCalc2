using ImGuiNET;

namespace LiftLugCalc2.GUI.Windows;

public static class CalculationModeWindow
{
    public static void Render(Session session)
    {
        ImGui.Begin("Calculation Mode");

        ImGui.SeparatorText("Calculation Type");

        ImGui.TextWrapped(
            "Choose whether to verify a selected lifting lug " +
            "or automatically determine the most suitable lug.");

        ImGui.Spacing();

        if (ImGui.RadioButton(
            "Forward Calculation",
            session.CalculationMode == CalculationMode.Forward))
        {
            session.CalculationMode = CalculationMode.Forward;
        }

        ImGui.TextDisabled(
            "Verify a selected lifting lug.");

        ImGui.Spacing();

        if (ImGui.RadioButton(
            "Reverse Calculation",
            session.CalculationMode == CalculationMode.Reverse))
        {
            session.CalculationMode = CalculationMode.Reverse;
        }

        ImGui.TextDisabled(
            "Automatically select the smallest suitable lifting lug.");

        ImGui.Separator();

        if (ImGui.Button("Back"))
        {
            session.CurrentScreen = Screen.LiftGeometry;
        }

        ImGui.SameLine();

        if (ImGui.Button("Next"))
        {
            session.CurrentScreen = Screen.MaterialSelection;
        }

        ImGui.End();
    }
}
