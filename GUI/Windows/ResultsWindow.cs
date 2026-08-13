using ImGuiNET;

using LiftLugCalc2.Core.Models;
using LiftLugCalc2.GUI.Helpers;

using System.Numerics;

namespace LiftLugCalc2.GUI.Windows;

public static class ResultsWindow
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;
        Project project = session.CurrentProject!;

        GuiCommon.SectionHeader("Calculation Results");
        GuiCommon.Spacer();

        // No result
        if (session.CurrentResult == null)
        {
            ImGui.Text("No suitable lug was found.");
            GuiCommon.Spacer();
            ImGui.TextWrapped("The calculation did not produce a valid lug selection.");
            return;
        }

        CalculationResult result = session.CurrentResult;

        ImGui.Text($"Calculation : {result.CalculationType}");
        ImGui.Text($"Project : {project.Name}");
        GuiCommon.Spacer();

        if (project.SelectedMaterial != null) ImGui.Text($"Material : {project.SelectedMaterial.Designation}");
        if (project.SelectedLug != null)
        {
            ImGui.Text($"Lug : {LugPresentation.GetLugTypeName(project.SelectedLug.LugType)}");
            ImGui.Text($"Lug ID : {project.SelectedLug.LugID}");
            ImGui.Text($"Lug WLL : {project.SelectedLug.LugWLL:N0} kg");
        }

        GuiCommon.Spacer();
        ImGui.Text($"Applied Load : {result.AppliedLoad:N1} kg");
        GuiCommon.Spacer();
        ImGui.Separator();
        GuiCommon.Spacer();

        ImGui.Text("Engineering Checks");
        GuiCommon.Spacer();
        if (ImGui.BeginTable("ResultsTable", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg))
        {
            ImGui.TableSetupColumn("Check");
            ImGui.TableSetupColumn("Capacity");
            ImGui.TableSetupColumn("Safety Factor");
            ImGui.TableSetupColumn("");

            ImGui.TableHeadersRow();
            ImGui.TableNextRow();

            ImGui.TableSetColumnIndex(0);
            ImGui.Text("Tension");

            ImGui.TableSetColumnIndex(1);
            ImGui.Text($"{result.TensionCapacity:N1} kN");

            ImGui.TableSetColumnIndex(2);
            ImGui.Text($"{result.FSTension:N1}");

            ImGui.TableSetColumnIndex(3);
            GuiCommon.SafetyFactorIndicator(result.FSTension);

            ImGui.TableNextRow();

            ImGui.TableSetColumnIndex(0);
            ImGui.Text("Shear");

            ImGui.TableSetColumnIndex(1);
            ImGui.Text($"{result.ShearCapacity:N1} kN");

            ImGui.TableSetColumnIndex(2);
            ImGui.Text($"{result.FSShear:N1}");

            ImGui.TableSetColumnIndex(3);
            GuiCommon.SafetyFactorIndicator(result.FSShear);

            ImGui.TableNextRow();

            ImGui.TableSetColumnIndex(0);
            ImGui.Text("Bearing");

            ImGui.TableSetColumnIndex(1);
            ImGui.Text($"{result.BearingCapacity:N1} kN");

            ImGui.TableSetColumnIndex(2);
            ImGui.Text($"{result.FSBearing:N1}");

            ImGui.TableSetColumnIndex(3);
            GuiCommon.SafetyFactorIndicator(result.FSBearing);

            ImGui.TableNextRow();

            ImGui.TableSetColumnIndex(0);
            ImGui.Text("Tear-out");

            ImGui.TableSetColumnIndex(1);
            ImGui.Text($"{result.TearOutCapacity:N1} kN");

            ImGui.TableSetColumnIndex(2);
            ImGui.Text($"{result.FSTearOut:N1}");

            ImGui.TableSetColumnIndex(3);
            GuiCommon.SafetyFactorIndicator(result.FSTearOut);

            ImGui.TableNextRow();

            ImGui.TableSetColumnIndex(0);
            ImGui.Text("Weld");

            ImGui.TableSetColumnIndex(1);
            ImGui.Text($"{result.WeldCapacity:N1} kN");

            ImGui.TableSetColumnIndex(2);
            ImGui.Text($"{result.FSWeld:N1}");

            ImGui.TableSetColumnIndex(3);
            GuiCommon.SafetyFactorIndicator(result.FSWeld);

            ImGui.EndTable();

            GuiCommon.Spacer();

            ImGui.Text("Safety Factor Indicator:");
            ImGui.TextColored(new Vector4(0.30f, 0.85f, 0.30f, 1.0f), "o Green");
            ImGui.SameLine();
            ImGui.Text($"FS >= {GuiConstants.RESULT_FS_YELLOW_LIMIT:F2}");
            ImGui.SameLine();
            ImGui.TextColored(new Vector4(1.00f, 0.80f, 0.20f, 1.0f), "o Yellow");
            ImGui.SameLine();
            ImGui.Text($"FS >= {GuiConstants.RESULT_FS_RED_LIMIT:F2}");
            ImGui.SameLine();
            ImGui.TextColored(new Vector4(1.00f, 0.35f, 0.35f, 1.0f), "o Red");
            ImGui.SameLine();
            ImGui.Text($"FS < {GuiConstants.RESULT_FS_RED_LIMIT:F2}");
        }

        GuiCommon.Spacer();

        ImGui.Text($"Minimum Safety Factor : {result.MinimumFS:N1}");
        ImGui.Text($"Weld Geometry : {(result.WeldGeometryOK ? "OK" : "NOT OK")}");
        GuiCommon.Spacer();
        ImGui.Separator();
        GuiCommon.Spacer();

        GuiCommon.ResultMessage(result.Pass);
    }
}
