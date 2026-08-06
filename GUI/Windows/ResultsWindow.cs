using ImGuiNET;

using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.GUI.Windows;

public static class ResultsWindow
{
    public static void Render(Session session)
    {
        if (session.CurrentResult is null)
            return;

        CalculationResult result = session.CurrentResult;

        ImGui.Begin("Calculation Results");

        // Overall result

        ImGui.SeparatorText("Overall Result");

        if (result.Pass)
        {
            ImGui.TextColored(
                new System.Numerics.Vector4(0.0f, 0.8f, 0.0f, 1.0f),
                "PASS");
        }
        else
        {
            ImGui.TextColored(
                new System.Numerics.Vector4(0.9f, 0.2f, 0.2f, 1.0f),
                "FAIL");
        }

        ImGui.Text($"Calculation Type : {result.CalculationType}");
        ImGui.Text($"Applied Load     : {result.AppliedLoad:F1} kN");

        ImGui.Separator();

        // Engineering checks

        ImGui.SeparatorText("Engineering Checks");

        ImGui.Text($"Net Section Capacity : {result.TensionCapacity:F1} kN");
        ImGui.Text($"Safety Factor        : {result.FSTension:F2}");

        ImGui.Spacing();

        ImGui.Text($"Shear Capacity       : {result.ShearCapacity:F1} kN");
        ImGui.Text($"Safety Factor        : {result.FSShear:F2}");

        ImGui.Spacing();

        ImGui.Text($"Bearing Capacity     : {result.BearingCapacity:F1} kN");
        ImGui.Text($"Safety Factor        : {result.FSBearing:F2}");

        ImGui.Spacing();

        ImGui.Text($"Tear-Out Capacity    : {result.TearOutCapacity:F1} kN");
        ImGui.Text($"Safety Factor        : {result.FSTearOut:F2}");

        ImGui.Spacing();

        ImGui.Text($"Weld Capacity        : {result.WeldCapacity:F1} kN");
        ImGui.Text($"Safety Factor        : {result.FSWeld:F2}");

        ImGui.Spacing();

        ImGui.Text($"Minimum Safety Factor : {result.MinimumFS:F2}");

        ImGui.Text(
            $"Weld Geometry : {(result.WeldGeometryOK ? "OK" : "NOT OK")}");

        ImGui.Separator();

        // Next actions

        ImGui.SeparatorText("Next Action");

        ImGui.BeginDisabled();

        ImGui.Button("Detailed Report");

        ImGui.Button("Export Report");

        ImGui.Button("Save Project");

        ImGui.EndDisabled();

        ImGui.Separator();

        if (ImGui.Button("New Calculation"))
        {
            // Reset current engineering session

            session.CurrentProject = null;
            session.SelectedMaterial = null;
            session.SelectedLug = null;
            session.CurrentResult = null;

            session.ProjectName = string.Empty;
            session.CreatedBy = string.Empty;
            session.Revision = string.Empty;

            session.WeightBasis = 0;
            session.NominalWeightKg = 0.0;
            session.WcfSelection = 1;

            session.CalculationMode = CalculationMode.Forward;

            session.StatusMessage = string.Empty;
            session.StatusType = StatusType.Information;

            session.CurrentScreen = Screen.NewProject;
        }

        ImGui.SameLine();

        if (ImGui.Button("Main Menu"))
        {
            session.CurrentScreen = Screen.MainMenu;
        }

        ImGui.End();
    }
}
