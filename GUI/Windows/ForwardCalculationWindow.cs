using ImGuiNET;

using LiftLugCalc2.Core.Engine;
using LiftLugCalc2.Core.Models;
using LiftLugCalc2.GUI.Helpers;

namespace LiftLugCalc2.GUI.Windows;

public static class ForwardCalculationWindow
{
    public static void Render(Session session)
    {
        Project project = session.CurrentProject!;
        Material material = session.SelectedMaterial!;
        TableLug lug = session.SelectedLug!;

        ImGui.Begin("Forward Calculation");

        ImGui.SeparatorText("Review Input Data");

        ImGui.Text($"Project : {project.Name}");
        ImGui.Text($"Material: {material.Designation}");
        ImGui.Text($"Lug ID  : {lug.LugID}");
        ImGui.Text($"Lug Type: {LugPresentation.GetLugTypeName(lug.LugType)}");
        ImGui.Text($"Lug WLL : {lug.LugWLL:F0} kg");

        ImGui.Separator();

        ImGui.Text("Design Load");

        ImGui.BulletText($"Nominal Weight : {session.NominalWeightKg:F1} kg");
        ImGui.BulletText($"Design WLL     : {project.WLL:F1} kg");

        ImGui.Separator();

        ImGui.Text("Lift Arrangement");

        ImGui.BulletText($"Lift Points : {project.NumberPoints}");

        switch (project.NumberPoints)
        {
            case 2:
                ImGui.BulletText($"A1 = {project.A1:F3} m");
                ImGui.BulletText($"A2 = {project.A2:F3} m");
                break;

            case 3:
                ImGui.BulletText($"A1 = {project.A1:F3} m");
                ImGui.BulletText($"A2 = {project.A2:F3} m");
                ImGui.BulletText($"B1 = {project.B1:F3} m");
                break;

            case 4:
                ImGui.BulletText($"A1 = {project.A1:F3} m");
                ImGui.BulletText($"A2 = {project.A2:F3} m");
                ImGui.BulletText($"B1 = {project.B1:F3} m");
                ImGui.BulletText($"B2 = {project.B2:F3} m");
                break;
        }

        ImGui.Separator();

        if (ImGui.Button("Back"))
        {
            session.CurrentScreen = Screen.LugGeometry;
        }

        ImGui.SameLine();

        if (ImGui.Button("Run Calculation"))
        {
            ForwardInput input = new(
                project,
                lug,
                material);

            string choice = CalculationModeHelper.StringChoice(session.CalculationMode);

            session.CurrentResult =
                ForwardCalculator.Run(input, choice);

            session.CurrentScreen = Screen.Results;
        }

        ImGui.End();
    }
}
