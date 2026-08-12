using ImGuiNET;

using LiftLugCalc2.Core.Utilities;

namespace LiftLugCalc2.GUI.Windows.ProjectSetups;

public static class ProjectSetupWindow
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;

        GuiCommon.SectionHeader("New Project");

        GuiCommon.Spacer();

        ImGui.Text("Project Name");
        ImGui.SameLine(GuiConstants.ProjectFieldLabelWidth);

        string projectName = session.ProjectName;

        ImGui.SetNextItemWidth(
            GuiConstants.ProjectNameFieldWidth);

        if (ImGui.InputText(
                "##ProjectName",
                ref projectName,
                100))
        {
            session.ProjectName = projectName;
        }

        ImGui.Text("Created By");
        ImGui.SameLine(GuiConstants.ProjectFieldLabelWidth);

        string createdBy = session.CreatedBy;

        ImGui.SetNextItemWidth(
            GuiConstants.CreatedByFieldWidth);

        if (ImGui.InputText(
                "##CreatedBy",
                ref createdBy,
                100))
        {
            session.CreatedBy = createdBy;
        }

        ImGui.Text("Revision");
        ImGui.SameLine(GuiConstants.ProjectFieldLabelWidth);

        string revision = session.Revision;

        ImGui.SetNextItemWidth(
            GuiConstants.RevisionFieldWidth);

        if (ImGui.InputText(
                "##Revision",
                ref revision,
                30))
        {
            session.Revision = revision;
        }

        GuiCommon.Spacer();

        ImGui.Text(
            $"Date : {DateTime.Today.ToString(Constants.DATE_FORMAT)}");
    }
}
