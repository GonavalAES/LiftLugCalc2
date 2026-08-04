using ImGuiNET;

using LiftLugCalc2.Core.Models;
using LiftLugCalc2.Core.Utilities;

using System.Numerics;

namespace LiftLugCalc2.GUI.Windows;

public static class ResultsWindow
{
    public static void Render()
    {
        if (AppState.LatestResult == null) return;

        ImGui.SetNextWindowPos(new Vector2(470, 10), ImGuiCond.FirstUseEver);
        ImGui.SetNextWindowSize(new Vector2(750, 680), ImGuiCond.FirstUseEver);

        if (!ImGui.Begin("Calculation Results"))
        {
            ImGui.End();
            return;
        }

        var res = AppState.LatestResult;

        // Banner Status
        GuiCommon.Header("Status Overview");
        GuiCommon.StatusBanner(res.Pass, $"OVERALL STATUS: {(res.Pass ? "PASS" : "FAIL")}");
        ImGui.Text($"Applied Load (PLP): {res.AppliedLoad:N1} kN");
        ImGui.Text($"Governing Min. Factor of Safety: {res.MinimumFS:F2}");

        ImGui.Spacing();

        // Safety Factors Table
        GuiCommon.Header("Capacity & Safety Factors");
        if (ImGui.BeginTable("Results Grid", 3, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
        {
            ImGui.TableSetupColumn("Failure Mode");
            ImGui.TableSetupColumn("Capacity (kN)", ImGuiTableColumnFlags.WidthFixed, 120f);
            ImGui.TableSetupColumn("Factor of Safety", ImGuiTableColumnFlags.WidthFixed, 140f);
            ImGui.TableHeadersRow();

            RenderRow("Tension", res.TensionCapacity, res.FSTension);
            RenderRow("Shear", res.ShearCapacity, res.FSShear);
            RenderRow("Bearing", res.BearingCapacity, res.FSBearing);
            RenderRow("Tear-Out", res.TearOutCapacity, res.FSTearOut);

            if (res.WeldCapacity > 0)
            {
                RenderRow("Weld", res.WeldCapacity, res.FSWeld);
            }

            ImGui.EndTable();
        }

        ImGui.Spacing();

        // Technical Report Output
        if (ImGui.CollapsingHeader("Full Technical Report"))
        {
            if (AppState.Lugs.Count > AppState.SelectedLugIndex && AppState.SelectedLugIndex >= 0)
            {
                var lug = AppState.Lugs[AppState.SelectedLugIndex];
                var mat = AppState.Materials[AppState.SelectedMaterialIndex];

                var tempProj = new Project
                {
                    Name = AppState.ProjectName,
                    WLL = AppState.NominalWeightKg * AppState.Wcf,
                    SelectedLug = lug,
                    SelectedMaterial = mat
                };

                string report = ReportGenerator.GenerateDetailedReport(tempProj, res);
                ImGui.InputTextMultiline("##reportText",
                                          ref report,
                                          (uint)report.Length,
                                          new Vector2(-1, 200),
                                          ImGuiInputTextFlags.ReadOnly);

                if (ImGui.Button("Copy Report to Clipboard")) ImGui.SetClipboardText(report);
            }
        }

        ImGui.End();
    }

    private static void RenderRow(string label, double capacity, double fs)
    {
        ImGui.TableNextRow();
        ImGui.TableNextColumn();
        ImGui.Text(label);

        ImGui.TableNextColumn();
        ImGui.Text($"{capacity:N1}");

        ImGui.TableNextColumn();
        Vector4 color = fs >= Constants.MINIMUM_SAFETY_FACTOR ? GuiCommon.PassColor : GuiCommon.FailColor;
        ImGui.TextColored(color, $"{fs:F2}");
    }
}
