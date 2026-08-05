using ImGuiNET;

using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.GUI.Windows;

public static class ProjectSetupWindow
{
    public static void Render(Session session)
    {
        ImGui.Begin("New Project");

        ImGui.SeparatorText("Project Information");

        ImGui.InputText("Project Name", ref session.ProjectName, 100);

        ImGui.InputText("Created By", ref session.CreatedBy, 100);

        ImGui.InputText("Revision", ref session.Revision, 30);

        ImGui.Spacing();

        ImGui.Text($"Date : {DateTime.Today:dd-MM-yyyy}");

        ImGui.Spacing();
        ImGui.Separator();

        if (ImGui.Button("Back"))
        {
            session.CurrentScreen = Screen.MainMenu;
        }

        ImGui.SameLine();

        if (ImGui.Button("Next"))
        {
            if (string.IsNullOrWhiteSpace(session.ProjectName))
            {
                session.StatusMessage = "Project name is required.";
                session.StatusType = StatusType.Error;
            }
            else
            {
                session.CurrentProject = new Project
                {
                    ProjectID = 0,
                    Name = session.ProjectName.Trim(),
                    CreatedBy = session.CreatedBy.Trim(),
                    Revision = session.Revision.Trim(),
                    Date = DateTime.Today.ToString("dd-MM-yyyy")
                };

                session.StatusMessage = "Project created.";
                session.StatusType = StatusType.Information;

                // Next screen (to be implemented next)
                session.CurrentScreen = Screen.WeightDefinition;
            }
        }

        ImGui.End();
    }
}
