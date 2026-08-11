using ImGuiNET;

using LiftLugCalc2.GUI.Enums;

namespace LiftLugCalc2.GUI.Windows.Navigation;

public static class CommandBar
{
    private static bool confirmCancelNewProject;

    public static void Render(GuiController controller)
    {
        switch (controller.CurrentScreen)
        {
            case Screen.NewProject:
                if (GuiCommon.Button("Back"))
                {
                    if (controller.HasNewProjectData()) confirmCancelNewProject = true;
                    else controller.CancelNewProject();
                }
                AlignRight();
                bool newProjectEnabled = !string.IsNullOrWhiteSpace(controller.CurrentSession.ProjectName);
                if (GuiCommon.Button("Next", newProjectEnabled)) controller.CreateProject();
                break;

            case Screen.WeightDefinition:
                if (GuiCommon.Button("Back")) controller.CurrentSession.CurrentScreen = Screen.NewProject;
                AlignRight();
                bool weightEnabled = controller.CurrentSession.NominalWeightKg > 0.0;
                if (GuiCommon.Button("Next", weightEnabled)) controller.AcceptWeightDefinition();
                break;

            case Screen.LiftGeometry:
                if (GuiCommon.Button("Back")) controller.CurrentSession.CurrentScreen = Screen.WeightDefinition;
                AlignRight();
                bool geometryEnabled = controller.CurrentSession.NumberPoints >= 2;
                if (GuiCommon.Button("Next", geometryEnabled)) controller.AcceptLiftGeometry();
                break;

            case Screen.CalculationMode:
                if (GuiCommon.Button("Back")) controller.CurrentSession.CurrentScreen = Screen.LiftGeometry;
                AlignRight();
                bool calculationModeEnabled = controller.CurrentSession.CalculationMode != CalculationMode.None;
                if (GuiCommon.Button("Next", calculationModeEnabled)) controller.AcceptCalculationMode();
                break;

            case Screen.MaterialSelection:
                if (GuiCommon.Button("Back")) controller.CurrentSession.CurrentScreen = Screen.CalculationMode;
                AlignRight();
                bool materialEnabled = controller.CurrentSession.SelectedMaterial is not null;
                if (GuiCommon.Button("Next", materialEnabled)) controller.AcceptMaterialSelection();
                break;

            case Screen.LugGeometry:
                if (GuiCommon.Button("Back")) controller.CurrentSession.CurrentScreen = Screen.MaterialSelection;
                AlignRight();
                bool lugEnabled = controller.CurrentSession.SelectedLug is not null;
                if (GuiCommon.Button("Next", lugEnabled)) controller.AcceptLugSelection();
                break;

            case Screen.ForwardCalculation:
                if (GuiCommon.Button("Back")) controller.CurrentSession.CurrentScreen = Screen.LugGeometry;
                AlignRight();
                if (GuiCommon.Button("Run")) controller.RunForwardCalculation();
                break;

            case Screen.ReverseCalculation:
                if (GuiCommon.Button("Back")) controller.CurrentSession.CurrentScreen = Screen.MaterialSelection;
                AlignRight();
                if (GuiCommon.Button("Run")) controller.RunReverseCalculation();
                break;

            case Screen.Results:
                AlignRight();
                if (GuiCommon.Button("New Calculation")) controller.CurrentSession.CurrentScreen = Screen.MainMenu;
                break;

            default:
                break;
        }

        DrawCancelNewProjectPopup(controller);
    }

    private static void AlignRight()
    {
        float buttonX = ImGui.GetCursorPosX() + ImGui.GetContentRegionAvail().X - GuiLayout.ButtonWidth;
        ImGui.SameLine(buttonX);
    }

    private static void DrawCancelNewProjectPopup(GuiController controller)
    {
        if (confirmCancelNewProject)
        {
            ImGui.OpenPopup("Abandon New Project");
            confirmCancelNewProject = false;
        }
        if (ImGui.BeginPopupModal("Abandon New Project", ImGuiWindowFlags.AlwaysAutoResize))
        {
            ImGui.Text("The entered project information will be lost.");
            ImGui.Spacing();
            if (ImGui.Button("Cancel")) ImGui.CloseCurrentPopup();
            ImGui.SameLine();
            AlignRight();
            if (ImGui.Button("Abandon"))
            {
                controller.CancelNewProject();
                ImGui.CloseCurrentPopup();
            }
            ImGui.EndPopup();
        }
    }
}
