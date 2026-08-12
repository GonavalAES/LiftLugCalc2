using ImGuiNET;

using LiftLugCalc2.Core.Models;
using LiftLugCalc2.GUI.Enums;

namespace LiftLugCalc2.GUI.Windows.Calculations;

public static class ForwardCalculationWindow
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;
        Project project = session.CurrentProject!;

        GuiCommon.SectionHeader("Forward Calculation");
        GuiCommon.Spacer();
        GuiCommon.SectionHeader("Project");

        GuiCommon.LabelValue("Project", project.Name);
        GuiCommon.LabelValue("Material", project.SelectedMaterial!.Designation);
        GuiCommon.LabelValue("Lug", $"ID {project.SelectedLug!.LugID} - {project.SelectedLug.LugType}");

        GuiCommon.Spacer();
        GuiCommon.SectionHeader("Lift Geometry");
        GuiCommon.LabelValue("A1", project.A1, "mm");
        GuiCommon.LabelValue("A2", project.A2, "mm");
        if (project.NumberPoints >= 3) GuiCommon.LabelValue("B1", project.B1, "mm");
        if (project.NumberPoints >= 4) GuiCommon.LabelValue("B2", project.B2, "mm");

        string warning = controller.GetLiftGeometryWarning();
        if (!string.IsNullOrWhiteSpace(warning))
        {
            GuiCommon.Spacer();
            GuiCommon.StatusMessage(StatusType.Warning, warning);
        }

        GuiCommon.Spacer();
        GuiCommon.SectionHeader("Load");

        GuiCommon.LabelValue("Nominal weight", session.NominalWeightKg, "kg");
        GuiCommon.LabelValue("WCF", project.WLL / session.NominalWeightKg);
        GuiCommon.LabelValue("Working load", project.WLL, "kg");

        GuiCommon.Spacer();

        GuiCommon.SectionHeader("Calculation");

        ImGui.TextWrapped("The forward calculation will perform the required lifting lug checks using the selected project, " +
                          "material and lug data.");
    }
}
