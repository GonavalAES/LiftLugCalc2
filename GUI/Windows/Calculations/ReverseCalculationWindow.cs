using ImGuiNET;

using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.GUI.Windows.Calculations;

public static class ReverseCalculationWindow
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;
        Project project = session.CurrentProject!;

        GuiCommon.SectionHeader("Reverse Calculation");
        GuiCommon.Spacer();
        GuiCommon.SectionHeader("Project");

        GuiCommon.LabelValue("Project", project.Name);
        GuiCommon.LabelValue("Material", session.SelectedMaterial!.Designation);

        GuiCommon.Spacer();
        GuiCommon.SectionHeader("Lift Geometry");
        GuiCommon.LabelValue("A1", project.A1, "mm");
        GuiCommon.LabelValue("A2", project.A2, "mm");
        if (project.NumberPoints >= 3) GuiCommon.LabelValue("B1", project.B1, "mm");
        if (project.NumberPoints >= 4) GuiCommon.LabelValue("B2", project.B2, "mm");

        GuiCommon.Spacer();
        GuiCommon.SectionHeader("Load");
        GuiCommon.LabelValue("Nominal weight", session.NominalWeightKg, "kg");
        GuiCommon.LabelValue("Working load", project.WLL, "kg");

        GuiCommon.Spacer();
        GuiCommon.SectionHeader("Calculation");
        ImGui.TextWrapped("The reverse calculation will evaluate the available lug types and select the smallest " +
                          "lug satisfying the required checks.");
    }
}
