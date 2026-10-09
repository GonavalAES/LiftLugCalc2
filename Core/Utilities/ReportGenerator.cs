using LiftLugCalc2.Core.FileOperations;
using LiftLugCalc2.Core.Models;
using LiftLugCalc2.GUI.Helpers;

using System.Text;

namespace LiftLugCalc2.Core.Utilities;

public static class ReportGenerator
{
    public static string GenerateDetailedReport(Project project, CalculationResult result,
                                                TableLug? exploratoryReferenceLug = null,
                                                ExploratoryLugEditor? exploratoryLugEditor = null)
    {
        var sb = new StringBuilder();

        sb.AppendLine("════════════════════════════════════════════════");
        sb.AppendLine("              LIFTING LUG CALCULATION REPORT");
        sb.AppendLine("════════════════════════════════════════════════");
        sb.AppendLine();
        sb.AppendLine("------------------------------------------------");
        sb.AppendLine("PROJECT INFORMATION");
        sb.AppendLine("------------------------------------------------");
        sb.AppendLine($"Project Name    : {project.Name}");
        sb.AppendLine($"Created By      : {project.CreatedBy}");
        sb.AppendLine($"Date            : {Formatter.FormatDate(project.Date)}");
        sb.AppendLine($"Revision        : {project.Revision}");
        sb.AppendLine("------------------------------------------------");
        sb.AppendLine();
        sb.AppendLine("------------------------------------------------");
        sb.AppendLine("LOAD CONFIGURATION");
        sb.AppendLine("------------------------------------------------");
        sb.AppendLine($"Load            : {project.WLL:N1} kg");
        sb.AppendLine($"Number of Points: {project.NumberPoints}");
        sb.AppendLine("------------------------------------------------");
        sb.AppendLine();
        sb.AppendLine("------------------------------------------------");
        sb.AppendLine("CALCULATION RESULTS");
        sb.AppendLine("------------------------------------------------");
        sb.AppendLine($"Analysis Type   : {result.CalculationType}");
        sb.AppendLine($"Applied Load    : {result.AppliedLoad:N1} kN");
        sb.AppendLine($"Overall Result  : {(result.Pass ? "PASSED" : "FAILED")}");
        sb.AppendLine($"Minimum FS      : {result.MinimumFS:N1}");
        sb.AppendLine("------------------------------------------------");
        sb.AppendLine();

        if (result.CalculationType == "Exploratory Calculation")
            AppendExploratoryDefinition(sb, exploratoryReferenceLug, exploratoryLugEditor);

        if (project.SelectedLug is { } lug)
        {
            sb.AppendLine("------------------------------------------------");
            sb.AppendLine("VERIFICATION SUMMARY");
            sb.AppendLine("------------------------------------------------");
            sb.AppendLine($"Table WLL check : {lug.LugWLL * project.NumberPoints:N0} kg ≥ {project.WLL:N0} kg");
            sb.AppendLine($"Calculated FS check   : Min FS {result.MinimumFS:N1} ≥ {Constants.MINIMUM_SAFETY_FACTOR:N1}");
            sb.AppendLine("------------------------------------------------");
            sb.AppendLine("LUG CHARACTERISTICS");
            sb.AppendLine("------------------------------------------------");

            string lugIdText = lug.LugID < 0 ? "User-defined exploratory lug" : lug.LugID.ToString();

            sb.AppendLine($"Lug ID               : {lugIdText}");
            sb.AppendLine($"Lug Type             : {lug.LugType}");
            sb.AppendLine($"Work Load Limit      : {lug.LugWLL} kg");
            sb.AppendLine();
            sb.AppendLine($"Plate Thickness      : {lug.ThicknessPlate} mm");
            sb.AppendLine($"Hole Diameter        : {lug.DiameterHole} mm");
            sb.AppendLine($"Lug Radius           : {lug.RadiusLug} mm");
            sb.AppendLine($"Center Hole Height   : {lug.HeightCenterHole} mm");
            sb.AppendLine($"Lug Length           : {lug.LengthLug} mm");
            if (lug.LugType == 3)
            {
                sb.AppendLine($"Cheek Boss Radius    : {lug.RadiusCheek_Boss} mm");
                sb.AppendLine($"Cheek Boss Thickness : {lug.ThicknessCheek_Boss} mm");
                sb.AppendLine($"Cheek Weld Throat    : {lug.WeldThroatCheek} mm");
            }
        }
        sb.AppendLine();
        sb.AppendLine("------------------------------------------------");
        sb.AppendLine("CAPACITY RESULTS");
        sb.AppendLine("------------------------------------------------");
        sb.AppendLine("Tension:");
        sb.AppendLine($"  Net Section Area : {result.NetSectionArea:N1} mm²");
        sb.AppendLine($"  Capacity         : {result.TensionCapacity:N1} kN");
        sb.AppendLine($"  Factor of Safety : {result.FSTension:N1}");
        sb.AppendLine();
        sb.AppendLine("Shear:");
        sb.AppendLine($"  Capacity         : {result.ShearCapacity:N1} kN");
        sb.AppendLine($"  Factor of Safety : {result.FSShear:N1}");
        sb.AppendLine();
        sb.AppendLine("Bearing:");
        sb.AppendLine($"  Capacity         : {result.BearingCapacity:N1} kN");
        sb.AppendLine($"  Factor of Safety : {result.FSBearing:N1}");
        sb.AppendLine();
        sb.AppendLine("Tear-Out:");
        sb.AppendLine($"  Capacity         : {result.TearOutCapacity:N1} kN");
        sb.AppendLine($"  Factor of Safety : {result.FSTearOut:N1}");
        sb.AppendLine();

        if (result.WeldCapacity > 0)
        {
            sb.AppendLine("Weld:");
            sb.AppendLine($"  Capacity         : {result.WeldCapacity:N1} kN");
            sb.AppendLine($"  Factor of Safety : {result.FSWeld:N1}");
            sb.AppendLine($"  Geometry Check   : {(result.WeldGeometryOK ? "OK" : "NOT OK")}");
            sb.AppendLine();
        }
        sb.AppendLine("------------------------------------------------");
        sb.AppendLine();

        sb.AppendLine($"Report generated: {DateTime.Now:dd-MM-yyyy HH:mm:ss}");

        return sb.ToString();
    }

    private static void AppendExploratoryDefinition(StringBuilder sb, TableLug? referenceLug, ExploratoryLugEditor? editor)
    {
        sb.AppendLine();
        sb.AppendLine("------------------------------------------------");
        sb.AppendLine("         EXPLORATORY LUG DEFINITION");
        sb.AppendLine("------------------------------------------------");

        if (referenceLug is null || editor is null)
        {
            sb.AppendLine("Exploratory reference geometry is not available in the current session.");
            return;
        }

        sb.AppendLine($"Reference Lug ID : {referenceLug.LugID}");
        sb.AppendLine($"Reference Lug Type : {referenceLug.LugType}");
        sb.AppendLine($"Reference Lug WLL : {referenceLug.LugWLL:N0} kg");
        sb.AppendLine();
        sb.AppendLine("MODIFIED PROPERTIES");

        if (!HasModifiedGeometry(editor))
        {
            sb.AppendLine("No properties were modified.");
            sb.AppendLine("Reference lug geometry was verified as entered.");
            return;
        }

        sb.AppendLine("Property                         Reference     Final Value");
        sb.AppendLine("------------------------------------------------------------");

        AppendModifiedGeometryLine(sb, "Plate thickness [mm]", referenceLug.ThicknessPlate, editor.ThicknessPlate, editor.ChangeThicknessPlate);
        AppendModifiedGeometryLine(sb, "Hole diameter [mm]", referenceLug.DiameterHole, editor.DiameterHole, editor.ChangeDiameterHole);
        AppendModifiedGeometryLine(sb, "Lug radius [mm]", referenceLug.RadiusLug, editor.RadiusLug, editor.ChangeRadiusLug);
        AppendModifiedGeometryLine(sb, "Hole centre height [mm]", referenceLug.HeightCenterHole, editor.HeightCenterHole, editor.ChangeHeightCenterHole);
        AppendModifiedGeometryLine(sb, "Lug length [mm]", referenceLug.LengthLug, editor.LengthLug, editor.ChangeLengthLug);
        AppendModifiedGeometryLine(sb, "Toe height [mm]", referenceLug.HeightToe, editor.HeightToe, editor.ChangeHeightToe);

        AppendCheekBossModifiedLines(sb, referenceLug, editor);
        AppendWeldModifiedLines(sb, referenceLug, editor);
    }

    private static void AppendModifiedGeometryLine(StringBuilder sb, string propertyName, double referenceValue, double finalValue, bool isModified)
    {
        if (!isModified) return;
        sb.AppendLine($"{propertyName,-30} {referenceValue,12:N2} {finalValue,16:N2}");
    }

    private static void AppendCheekBossModifiedLines(StringBuilder sb, TableLug referenceLug, ExploratoryLugEditor editor)
    {
        if (editor.LugType is not 2 and not 3) return;

        string radiusName = editor.LugType == 2 ? "Cheek radius [mm]" : "Boss radius [mm]";
        string thicknessName = editor.LugType == 2 ? "Cheek thickness [mm]" : "Boss thickness [mm]";

        AppendModifiedGeometryLine(sb, radiusName, referenceLug.RadiusCheek_Boss, editor.RadiusCheekBoss, editor.ChangeRadiusCheekBoss);
        AppendModifiedGeometryLine(sb, thicknessName, referenceLug.ThicknessCheek_Boss, editor.ThicknessCheekBoss, editor.ChangeThicknessCheekBoss);
    }

    private static void AppendWeldModifiedLines(StringBuilder sb, TableLug referenceLug, ExploratoryLugEditor editor)
    {
        if (editor.LugType is 1 or 2 or 3)
            AppendModifiedGeometryLine(sb, "Main weld throat [mm]", referenceLug.LugWeldThroat, editor.LugWeldThroat, editor.ChangeLugWeldThroat);
        if (editor.LugType is not 2 and not 3) return;

        string weldName = editor.LugType == 2 ? "Cheek weld throat [mm]" : "Boss weld throat [mm]";

        AppendModifiedGeometryLine(sb, weldName, referenceLug.WeldThroatCheek, editor.WeldThroatCheek, editor.ChangeWeldThroatCheek);
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

