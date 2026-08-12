using ImGuiNET;

using LiftLugCalc2.Core.Utilities;

namespace LiftLugCalc2.GUI.Windows;

public static class WeightDefinitionWindow
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;

        GuiCommon.SectionHeader("Weight Definition");

        GuiCommon.Spacer();

        //--------------------------------------------------------
        // Weight basis
        //--------------------------------------------------------

        ImGui.Text("Weight Basis");

        int weightBasis = session.WeightBasis;

        ImGui.RadioButton(
            "Actual Object Weight",
            ref weightBasis,
            1);

        ImGui.RadioButton(
            "Working Load Limit (WLL)",
            ref weightBasis,
            2);

        session.WeightBasis = weightBasis;

        GuiCommon.Spacer();

        //--------------------------------------------------------
        // Nominal weight
        //--------------------------------------------------------

        double nominalWeight = session.NominalWeightKg;

        ImGui.SetNextItemWidth(90.0f);

        if (GuiCommon.InputDouble(
        "Value [kg]",
        ref nominalWeight))
        {
            session.NominalWeightKg = nominalWeight;
        }

        GuiCommon.Spacer();

        //--------------------------------------------------------
        // Weight Correction Factor
        //--------------------------------------------------------

        GuiCommon.SectionHeader("Weight Calculation Factor");

        int wcf = session.WcfSelection;

        ImGui.RadioButton(
            $"Measured (±3%)    ({Constants.WCF_MEASURED:F2})",
            ref wcf,
            1);

        ImGui.RadioButton(
            $"Updated drawings ({Constants.WCF_DETAILED_UPDATED:F2})",
            ref wcf,
            2);

        ImGui.RadioButton(
            $"Older drawings   ({Constants.WCF_DETAILED_OLDER:F2})",
            ref wcf,
            3);

        ImGui.RadioButton(
            $"Standard value   ({Constants.WCF_STANDARD:F2})",
            ref wcf,
            4);

        session.WcfSelection = wcf;
    }
}
