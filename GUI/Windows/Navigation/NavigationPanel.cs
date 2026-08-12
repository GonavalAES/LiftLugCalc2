using ImGuiNET;

using LiftLugCalc2.GUI.Enums;

namespace LiftLugCalc2.GUI.Windows.Navigation;

public static class NavigationPanel
{
    public static void Render(GuiController controller)
    {
        GuiCommon.SectionHeader("Project");

        DrawStep(controller, "Project Setup", ScreenEnum.NewProject);
        DrawStep(controller, "Weight", ScreenEnum.WeightDefinition);
        DrawStep(controller, "Lift Geometry", ScreenEnum.LiftGeometry);
        DrawStep(controller, "Calculation Mode", ScreenEnum.CalculationMode);
        DrawStep(controller, "Material", ScreenEnum.MaterialSelection);

        if (controller.CurrentSession.CalculationMode == CalculationMode.Forward)
            DrawStep(controller, "Lug", ScreenEnum.LugGeometry);
        if (controller.CurrentSession.CalculationMode == CalculationMode.Forward && controller.CurrentSession.SelectedLug is not null)
            DrawStep(controller, "Run", ScreenEnum.ForwardCalculation);
        else if (controller.CurrentSession.CalculationMode == CalculationMode.Reverse && controller.CurrentSession.SelectedMaterial is not null)
            DrawStep(controller, "Run", ScreenEnum.ReverseCalculation);

        DrawStep(controller, "Results", ScreenEnum.Results);
    }

    private static void DrawStep(GuiController controller, string label, ScreenEnum screen)
    {
        bool enabled = controller.CanNavigateTo(screen);
        bool selected = controller.CurrentScreen == screen;
        bool complete = controller.IsStepComplete(screen);
        bool problem = controller.HasStepProblem(screen);

        string indicator;
        if (selected) indicator = ">";
        else if (problem) indicator = "!";
        else if (complete) indicator = "✓";
        else indicator = "·";

        string text = $"{indicator} {label}";

        ImGui.BeginDisabled(!enabled);
        if (ImGui.Selectable(text, selected)) controller.NavigateTo(screen);

        ImGui.EndDisabled();
    }
}
