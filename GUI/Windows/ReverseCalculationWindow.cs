using ImGuiNET;

using LiftLugCalc2.Core.Engine;
using LiftLugCalc2.Core.Models;
using LiftLugCalc2.GUI.Helpers;

namespace LiftLugCalc2.GUI.Windows;

public static class ReverseCalculationWindow
{
    public static void Render(Session session)
    {
        Project project = session.CurrentProject!;
        Material material = session.SelectedMaterial!;

        ImGui.Begin("Reverse Calculation");

        // Review

        ImGui.SeparatorText("Review Input Data");

        ImGui.Text($"Project : {project.Name}");

        ImGui.Text($"Material : {material.Designation}");

        ImGui.Text($"Design WLL : {project.WLL:F1} kg");

        ImGui.Text($"Lift Points : {project.NumberPoints}");

        ImGui.Separator();

        ImGui.TextWrapped(
            "The program will search the available lifting lugs " +
            "and automatically select the smallest lug that satisfies " +
            "all engineering checks.");

        ImGui.Separator();

        // Navigation

        if (ImGui.Button("Back"))
        {
            session.CurrentScreen = Screen.MaterialSelection;
        }

        ImGui.SameLine();

        if (ImGui.Button("Run Calculation"))
        {
            ReverseInput input = new(
                project,
                material,
                AppState.Lugs);

            string choice = CalculationModeHelper.StringChoice(session.CalculationMode);

            ReverseSelection selection =
                ReverseCalculator.Run(input, choice);

            // No suitable lug

            if (selection.Best is null)
            {
                session.StatusType = StatusType.Warning;
                session.StatusMessage =
                    "No suitable lifting lug satisfies the specified loading.";

                ImGui.End();
                return;
            }

            // Store engineering result

            project.SelectedMaterial = material;
            project.SelectedLug = selection.Best.Lug;

            session.SelectedMaterial = material;
            session.SelectedLug = selection.Best.Lug;
            session.CurrentResult = selection.Best.Result;

            session.StatusType = StatusType.Information;
            session.StatusMessage =
                "Reverse calculation completed successfully.";

            session.CurrentScreen = Screen.Results;
        }

        ImGui.End();
    }
}
