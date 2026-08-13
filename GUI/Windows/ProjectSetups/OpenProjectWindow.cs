using ImGuiNET;

using LiftLugCalc2.Core.FileOperations;

using System.Numerics;

namespace LiftLugCalc2.GUI.Windows.ProjectSetups;

public static class OpenProjectWindow
{
    private static string? selectedProject;
    private static List<string> projects = new();

    public static void Open()
    {
        selectedProject = null;
        projects = FilingSystem.GetAllProjectNames().Where(name => !string.IsNullOrWhiteSpace(name)).Select(name => name!)
                                                    .OrderBy(name => name).ToList();
        ImGui.OpenPopup("Open Project");
    }

    public static void Render(GuiController controller)
    {
        if (!ImGui.BeginPopupModal(
            "Open Project",
            ImGuiWindowFlags.AlwaysAutoResize))
        {
            return;
        }

        GuiCommon.SectionHeader("Open Project");
        GuiCommon.Spacer();

        if (projects.Count == 0)
        {
            ImGui.Text("No saved projects were found.");
        }
        else
        {
            ImGui.Text("Available Projects");
            GuiCommon.Spacer();

            if (ImGui.BeginListBox("##Projects", new Vector2(350, 150)))
            {
                foreach (string projectName in projects)
                {
                    bool selected = selectedProject == projectName;

                    if (ImGui.Selectable(projectName, selected))
                    {
                        selectedProject = projectName;
                    }

                    if (selected)
                        ImGui.SetItemDefaultFocus();
                }

                ImGui.EndListBox();
            }
        }

        GuiCommon.Spacer();
        ImGui.Separator();
        GuiCommon.Spacer();

        if (GuiCommon.Button("CANCEL"))
        {
            ImGui.CloseCurrentPopup();
        }

        ImGui.SameLine();
        GuiCommon.AlignRight();

        bool openEnabled =
            !string.IsNullOrWhiteSpace(selectedProject);

        if (GuiCommon.Button("OPEN", openEnabled))
        {
            string projectName = selectedProject!;

            ImGui.CloseCurrentPopup();
            selectedProject = null;

            controller.OpenProject(projectName);
        }

        ImGui.EndPopup();
    }
}
