using ImGuiNET;

using LiftLugCalc2.GUI.Enums;
using LiftLugCalc2.GUI.Windows.ProjectSetups;

namespace LiftLugCalc2.GUI.Windows.Navigation;

public static class CommandBar
{
    private static bool confirmCancelNewProject;

    public static void Render(GuiController controller)
    {
        //
        // ROW 1 - Workflow commands
        //

        switch (controller.CurrentScreen)
        {
            case ScreenEnum.NewProject:
                if (GuiCommon.Button("BACK"))
                {
                    if (controller.HasNewProjectData()) confirmCancelNewProject = true;
                    else controller.CancelNewProject();
                }
                GuiCommon.AlignRight();
                bool newProjectEnabled = !string.IsNullOrWhiteSpace(controller.CurrentSession.ProjectName);
                if (GuiCommon.Button("NEXT", newProjectEnabled)) controller.CreateProject();
                break;

            case ScreenEnum.WeightDefinition:
                if (GuiCommon.Button("BACK")) controller.CurrentScreen = ScreenEnum.NewProject;
                GuiCommon.AlignRight();
                bool weightEnabled = controller.CurrentSession.NominalWeightKg > 0.0;
                if (GuiCommon.Button("NEXT", weightEnabled)) controller.AcceptWeightDefinition();
                break;

            case ScreenEnum.LiftGeometry:
                if (GuiCommon.Button("BACK")) controller.CurrentScreen = ScreenEnum.WeightDefinition;
                GuiCommon.AlignRight();
                bool geometryEnabled = controller.CurrentSession.NumberPoints > 0;
                if (GuiCommon.Button("NEXT", geometryEnabled)) controller.AcceptLiftGeometry();
                break;

            case ScreenEnum.CalculationMode:
                if (GuiCommon.Button("BACK")) controller.CurrentScreen = ScreenEnum.LiftGeometry;
                GuiCommon.AlignRight();
                if (controller.CurrentSession.CalculationMode == CalculationMode.None) GuiCommon.Button("NEXT", false);
                else if (GuiCommon.Button("NEXT")) controller.AcceptCalculationMode();
                break;

            case ScreenEnum.MaterialSelection:
                if (GuiCommon.Button("BACK")) controller.CurrentScreen = ScreenEnum.CalculationMode;
                GuiCommon.AlignRight();
                bool materialEnabled = controller.CurrentSession.SelectedMaterial is not null;
                if (GuiCommon.Button("NEXT", materialEnabled)) controller.AcceptMaterialSelection();
                break;

            case ScreenEnum.LugGeometry:

                if (GuiCommon.Button("BACK"))
                    controller.CurrentScreen = ScreenEnum.MaterialSelection;

                GuiCommon.AlignRight();

                bool lugEnabled =
                    controller.CurrentSession.SelectedLug is not null;

                if (GuiCommon.Button("NEXT", lugEnabled))
                    controller.AcceptLugSelection();

                break;


            case ScreenEnum.ForwardCalculation:

                if (GuiCommon.Button("BACK"))
                    controller.CurrentScreen = ScreenEnum.LugGeometry;

                GuiCommon.AlignRight();

                if (GuiCommon.Button("RUN"))
                    controller.RunForwardCalculation();

                break;


            case ScreenEnum.ReverseCalculation:

                if (GuiCommon.Button("BACK"))
                    controller.CurrentScreen = ScreenEnum.MaterialSelection;

                GuiCommon.AlignRight();

                if (GuiCommon.Button("RUN"))
                    controller.RunReverseCalculation();

                break;


            case ScreenEnum.Results:

                if (GuiCommon.Button("BACK"))
                    controller.CurrentScreen = ScreenEnum.CalculationMode;

                ImGui.SameLine();
                GuiCommon.AlignCenter();

                if (GuiCommon.Button("MODIFY / RE-RUN"))
                    controller.ModifyCalculation();

                break;


            default:
                break;
        }


        //
        // ROW 2 - Project commands
        //

        ImGui.Separator();

        if (GuiCommon.Button("NEW"))
        {
            if (controller.CurrentSession.CurrentProject is not null ||
                controller.HasNewProjectData())
            {
                ImGui.OpenPopup("New Project Confirmation");
            }
            else
            {
                controller.StartNewProject();
            }
        }

        ImGui.SameLine();

        if (GuiCommon.Button("OPEN"))
        {
            OpenProjectWindow.Open();
        }

        ImGui.SameLine();

        if (GuiCommon.Button("SAVE"))
        {
            controller.SaveProject();
        }

        ImGui.SameLine();

        bool reportEnabled = controller.CurrentSession.CurrentProject is not null && controller.CurrentSession.CurrentResult is not null;

        if (GuiCommon.Button("REPORT", reportEnabled))
        {
            controller.GenerateReport();
        }


        // NEW PROJECT confirmation
        if (ImGui.BeginPopupModal(
            "New Project Confirmation",
            ImGuiWindowFlags.AlwaysAutoResize))
        {
            ImGui.Text(
                "Start a new project?\n\n" +
                "The current project and calculation result will be discarded.");

            GuiCommon.Spacer();

            if (GuiCommon.Button("CANCEL"))
            {
                ImGui.CloseCurrentPopup();
            }

            ImGui.SameLine();
            GuiCommon.AlignRight();

            if (GuiCommon.Button("NEW PROJECT"))
            {
                ImGui.CloseCurrentPopup();
                controller.StartNewProject();
            }

            ImGui.EndPopup();
        }

        // OPEN PROJECT window
        OpenProjectWindow.Render(controller);

        // ABANDON NEW PROJECT confirmation
        DrawCancelNewProjectPopup(controller);
    }


    private static void DrawCancelNewProjectPopup(
        GuiController controller)
    {
        if (confirmCancelNewProject)
        {
            ImGui.OpenPopup("Abandon New Project");
            confirmCancelNewProject = false;
        }

        if (ImGui.BeginPopupModal(
            "Abandon New Project",
            ImGuiWindowFlags.AlwaysAutoResize))
        {
            ImGui.Text(
                "The entered project information will be lost.");

            ImGui.Spacing();

            if (ImGui.Button("CANCEL"))
                ImGui.CloseCurrentPopup();

            ImGui.SameLine();
            GuiCommon.AlignRight();

            if (ImGui.Button("ABANDON"))
            {
                controller.CancelNewProject();
                ImGui.CloseCurrentPopup();
            }

            ImGui.EndPopup();
        }
    }
}
