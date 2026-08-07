using ImGuiNET;

using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.GUI.Windows.Newones;

public static class ProjectSetupWindow
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;

        //--------------------------------------------------------
        // Page title
        //--------------------------------------------------------

        GuiCommon.SectionHeader("New Project");

        GuiCommon.Spacer();

        //--------------------------------------------------------
        // Project Name
        //--------------------------------------------------------

        string projectName = session.ProjectName;

        if (ImGui.InputText(
            "Project Name",
            ref projectName,
            100))
        {
            session.ProjectName = projectName;
        }

        //--------------------------------------------------------
        // Created By
        //--------------------------------------------------------

        string createdBy = session.CreatedBy;

        if (ImGui.InputText(
            "Created By",
            ref createdBy,
            100))
        {
            session.CreatedBy = createdBy;
        }

        //--------------------------------------------------------
        // Revision
        //--------------------------------------------------------

        string revision = session.Revision;

        if (ImGui.InputText(
            "Revision",
            ref revision,
            30))
        {
            session.Revision = revision;
        }

        GuiCommon.Spacer();

        ImGui.Text($"Date : {DateTime.Today.ToString(Constants.DATE_FORMAT)}");
    }
}
