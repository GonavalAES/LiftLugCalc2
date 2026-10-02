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
        if (result.CalculationType == "Exploratory Calculation") DrawExploratoryLugSummary(session);
        else if (project.SelectedLug is not null)
        {
            ImGui.Text($"Lug: {LugPresentation.GetLugTypeName(project.SelectedLug.LugType)}");
            ImGui.Text($"Lug ID: {project.SelectedLug.LugID}");
            ImGui.Text($"Lug WLL: {project.SelectedLug.LugWLL:N0} kg");
        }

        GuiCommon.Spacer();
        ImGui.Text($"Applied Load : {result.AppliedLoad:N1} kN");
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

    private static void DrawExploratoryLugSummary(Session session)
    {
        TableLug? referenceLug = session.ExploratoryReferenceLug;
        ExploratoryLugEditor editor = session.ExploratoryLugEditor;

        GuiCommon.SectionHeader("Exploratory Lug Definition");
        ImGui.Text("Lug: User-defined exploratory lug");
        ImGui.Text($"Lug type: {LugPresentation.GetLugTypeName(editor.LugType)}");

        if (referenceLug is null)
        {
            ImGui.TextWrapped("Reference lug data is not available in the current session.");
            return;
        }

        ImGui.Text($"Reference lug: ID {referenceLug.LugID}");
        ImGui.Text($"Reference WLL: {referenceLug.LugWLL:N0} kg");
        GuiCommon.Spacer();

        DrawModifiedGeometry(referenceLug, editor);
    }

    private static void DrawModifiedGeometry(TableLug referenceLug, ExploratoryLugEditor editor)
    {
        bool hasModifiedGeometry = HasModifiedGeometry(editor);

        GuiCommon.SectionHeader("Modified Geometry");

        if (!hasModifiedGeometry)
        {
            ImGui.TextWrapped("No geometry properties were modified. The reference lug geometry was verified as entered.");
            return;
        }

        if (!ImGui.BeginTable("ModifiedExploratoryGeometryTable", 3, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg |
                              ImGuiTableFlags.SizingFixedFit)) return;

        ImGui.TableSetupColumn("Property");
        ImGui.TableSetupColumn("Reference");
        ImGui.TableSetupColumn("Final value");
        ImGui.TableHeadersRow();

        DrawModifiedGeometryRow("Plate thickness [mm]", referenceLug.ThicknessPlate, editor.ThicknessPlate,
                                editor.ChangeThicknessPlate);
        DrawModifiedGeometryRow("Hole diameter [mm]", referenceLug.DiameterHole, editor.DiameterHole, editor.ChangeDiameterHole);
        DrawModifiedGeometryRow("Lug radius [mm]", referenceLug.RadiusLug, editor.RadiusLug, editor.ChangeRadiusLug);
        DrawModifiedGeometryRow("Hole centre height [mm]", referenceLug.HeightCenterHole, editor.HeightCenterHole,
                                editor.ChangeHeightCenterHole);
        DrawModifiedGeometryRow("Lug length [mm]", referenceLug.LengthLug, editor.LengthLug, editor.ChangeLengthLug);
        DrawModifiedGeometryRow("Toe height [mm]", referenceLug.HeightToe, editor.HeightToe, editor.ChangeHeightToe);
        DrawCheekBossModifiedGeometryRows(referenceLug, editor);
        DrawWeldModifiedGeometryRows(referenceLug, editor);

        ImGui.EndTable();
    }

    private static void DrawModifiedGeometryRow(string propertyName, double referenceValue, double finalValue, bool isModified)
    {
        if (!isModified) return;

        ImGui.TableNextRow();
        ImGui.TableSetColumnIndex(0);
        ImGui.Text(propertyName);
        ImGui.TableSetColumnIndex(1);
        ImGui.Text($"{referenceValue:N2}");
        ImGui.TableSetColumnIndex(2);
        ImGui.Text($"{finalValue:N2}");
    }

    private static void DrawCheekBossModifiedGeometryRows(TableLug referenceLug, ExploratoryLugEditor editor)
    {
        if (editor.LugType is not 2 and not 3) return;

        string radiusName = editor.LugType == 2 ? "Cheek radius [mm]" : "Boss radius [mm]";
        string thicknessName = editor.LugType == 2 ? "Cheek thickness [mm]" : "Boss thickness [mm]";

        DrawModifiedGeometryRow(radiusName, referenceLug.RadiusCheek_Boss, editor.RadiusCheekBoss, editor.ChangeRadiusCheekBoss);
        DrawModifiedGeometryRow(thicknessName, referenceLug.ThicknessCheek_Boss, editor.ThicknessCheekBoss, editor.ChangeThicknessCheekBoss);
    }

    private static void DrawWeldModifiedGeometryRows(TableLug referenceLug, ExploratoryLugEditor editor)
    {
        if (editor.LugType is 1 or 2 or 3) DrawModifiedGeometryRow("Main weld throat [mm]", referenceLug.LugWeldThroat,
                                                                   editor.LugWeldThroat, editor.ChangeLugWeldThroat);
        if (editor.LugType is not 2 and not 3) return;

        string weldName = editor.LugType == 2 ? "Cheek weld throat [mm]" : "Boss weld throat [mm]";

        DrawModifiedGeometryRow(weldName, referenceLug.WeldThroatCheek, editor.WeldThroatCheek, editor.ChangeWeldThroatCheek);
    }

    private static bool HasModifiedGeometry(ExploratoryLugEditor editor)
    {
        return
            editor.ChangeThicknessPlate ||
            editor.ChangeDiameterHole ||
            editor.ChangeRadiusLug ||
            editor.ChangeHeightCenterHole ||
            editor.ChangeLengthLug ||
            editor.ChangeHeightToe ||
            editor.ChangeRadiusCheekBoss ||
            editor.ChangeThicknessCheekBoss ||
            editor.ChangeWeldThroatCheek ||
            editor.ChangeLugWeldThroat;
    }
}
