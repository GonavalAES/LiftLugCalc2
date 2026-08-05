using ImGuiNET;

using LiftLugCalc2.Core.Engine;
using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.GUI.Windows;

public static class WeightWindow
{
    public static void Render(Session session)
    {
        ImGui.Begin("Weight Definition");

        ImGui.SeparatorText("Weight Basis");
        ImGui.RadioButton("Actual Object Weight", ref session.WeightBasis, 1);
        ImGui.RadioButton("Working Load Limit (WLL)", ref session.WeightBasis, 2);

        ImGui.Spacing();

        if (session.WeightBasis == 1) ImGui.InputDouble("Actual Weight [kg]", ref session.NominalWeightKg);
        if (session.WeightBasis == 2) ImGui.InputDouble("Working Load Limit [kg]", ref session.NominalWeightKg);

        ImGui.Separator();

        ImGui.SeparatorText("Weight Correction Factor");
        ImGui.RadioButton($"Weighing / measured ±3% (WCF = " +
            $"{Constants.WCF_MEASURED:F2})", ref session.WcfSelection, 1);
        ImGui.RadioButton($"Detailed calc from updated drawings (WCF = " +
            $"{Constants.WCF_DETAILED_UPDATED:F2})", ref session.WcfSelection, 2);
        ImGui.RadioButton($"Detailed calc from older/less accurate drawings (WCF = " +
            $"{Constants.WCF_DETAILED_OLDER:F2})", ref session.WcfSelection, 3);
        ImGui.RadioButton($"Standard-mandated (WCF = " +
            $"{Constants.WCF_STANDARD:F2})", ref session.WcfSelection, 4);

        ImGui.Spacing();

        if (ImGui.Button("Back")) session.CurrentScreen = Screen.NewProject;

        ImGui.SameLine();

        if (ImGui.Button("Next"))
        {
            if (session.WeightBasis == 0)
            {
                session.StatusMessage = "Please select the weight basis.";
                session.StatusType = StatusType.Error;
            }
            else if (session.NominalWeightKg <= 0.0)
            {
                session.StatusMessage = "Weight must be greater than zero.";
                session.StatusType = StatusType.Error;
            }
            else
            {
                double wcf = PreliminaryCalculations.ChoosingWCF(session.WcfSelection.ToString());
                session.CurrentProject!.WLL = session.NominalWeightKg * wcf;

                session.StatusMessage = "Weight definition completed.";
                session.StatusType = StatusType.Information;

                session.CurrentScreen = Screen.LiftGeometry;
            }
        }

        ImGui.End();
    }
}