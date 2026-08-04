using ImGuiNET;

using LiftLugCalc2.Core.Engine;
using LiftLugCalc2.Core.Models;

using System.Numerics;

namespace LiftLugCalc2.GUI.Windows;

public static class InputWindow
{
    public static void Render()
    {
        ImGui.SetNextWindowPos(new Vector2(10, 10), ImGuiCond.FirstUseEver);
        ImGui.SetNextWindowSize(new Vector2(450, 680), ImGuiCond.FirstUseEver);

        if (!ImGui.Begin("Engineering Inputs", ImGuiWindowFlags.AlwaysAutoResize))
        {
            ImGui.End();
            return;
        }

        if (ImGui.CollapsingHeader("1. Project Information", ImGuiTreeNodeFlags.DefaultOpen))
        {
            ImGui.InputText("Project Name", ref AppState.ProjectName, 100);
            ImGui.InputDouble("Nominal Weight (kg)", ref AppState.NominalWeightKg);
            ImGui.InputDouble("WCF Factor", ref AppState.Wcf);

            double calculatedWll = AppState.NominalWeightKg * AppState.Wcf;
            ImGui.TextColored(GuiCommon.SubtextColor, $"Resulting Design Load (WLL): {calculatedWll:N1} kg");
        }

        ImGui.Spacing();

        if (ImGui.CollapsingHeader("2. Selection", ImGuiTreeNodeFlags.DefaultOpen))
        {
            if (AppState.LugNames.Length > 0)
            {
                ImGui.Combo("Select Lug", ref AppState.SelectedLugIndex, AppState.LugNames, AppState.LugNames.Length);

                if (AppState.SelectedLugIndex >= 0 && AppState.SelectedLugIndex < AppState.Lugs.Count)
                {
                    var lug = AppState.Lugs[AppState.SelectedLugIndex];
                    ImGui.TextColored(GuiCommon.SubtextColor,
                        $"Plate: {lug.ThicknessPlate}mm | Hole Ø: {lug.DiameterHole}mm | Radius: {lug.RadiusLug}mm");
                }
            }

            ImGui.Spacing();

            if (AppState.MaterialNames.Length > 0)
            {
                ImGui.Combo("Select Material", ref AppState.SelectedMaterialIndex, AppState.MaterialNames, AppState.MaterialNames.Length);

                if (AppState.SelectedMaterialIndex >= 0 && AppState.SelectedMaterialIndex < AppState.Materials.Count)
                {
                    var mat = AppState.Materials[AppState.SelectedMaterialIndex];
                    ImGui.TextColored(GuiCommon.SubtextColor, $"Yield Strength: {mat.YieldStrength} MPa");
                }
            }
        }

        ImGui.Spacing();
        ImGui.Separator();
        ImGui.Spacing();

        if (ImGui.Button("Run Forward Calculation", new Vector2(-1, 38)))
        {
            ExecuteForwardCalculation();
        }

        ImGui.End();
    }

    private static void ExecuteForwardCalculation()
    {
        if (AppState.SelectedLugIndex < 0 || AppState.SelectedMaterialIndex < 0) return;

        var lug = AppState.Lugs[AppState.SelectedLugIndex];
        var material = AppState.Materials[AppState.SelectedMaterialIndex];

        var project = new Project
        {
            Name = AppState.ProjectName,
            WLL = AppState.NominalWeightKg * AppState.Wcf,
            NumberPoints = 1,
            SelectedLug = lug,
            SelectedMaterial = material
        };

        var input = new ForwardInput(project, lug, material);
        AppState.LatestResult = ForwardCalculator.Run(input, "1");
    }
}
