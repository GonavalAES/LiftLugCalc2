using LiftLugCalc2.Core.FileOperations;
using LiftLugCalc2.Core.Models;

using System.Text;

namespace LiftLugCalc2.Core.Utilities;

public static class ReportGenerator
{
    public static string GenerateDetailedReport(Project project, CalculationResult result)
    {
        var sb = new StringBuilder();

        sb.AppendLine("═══════════════════════════════════════════════════════════");
        sb.AppendLine("              LIFTING LUG CALCULATION REPORT");
        sb.AppendLine("═══════════════════════════════════════════════════════════");
        sb.AppendLine();
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine("PROJECT INFORMATION");
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine($"Project Name    : {project.Name}");
        sb.AppendLine($"Created By      : {project.CreatedBy}");
        sb.AppendLine($"Date            : {Formatter.FormatDate(project.Date)}");
        sb.AppendLine($"Revision        : {project.Revision}");
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine();
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine("LOAD CONFIGURATION");
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine($"Load            : {project.WLL:N1} kg");
        sb.AppendLine($"Number of Points: {project.NumberPoints}");
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine();
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine("CALCULATION RESULTS");
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine($"Analysis Type   : {result.CalculationType}");
        sb.AppendLine($"Applied Load    : {result.AppliedLoad:N1} kN");
        sb.AppendLine($"Overall Result  : {(result.Pass ? "PASSED" : "FAILED")}");
        sb.AppendLine($"Minimum FS      : {result.MinimumFS:N1}");
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine();

        if (project.SelectedLug is { } lug)
        {
            sb.AppendLine("-----------------------------------------------------------");
            sb.AppendLine("VERIFICATION SUMMARY");
            sb.AppendLine("-----------------------------------------------------------");
            sb.AppendLine($"Table WLL check : {lug.LugWLL * project.NumberPoints:N0} kg ≥ {project.WLL:N0} kg");
            sb.AppendLine($"Calculated FS check   : Min FS {result.MinimumFS:N1} ≥ {Constants.MINIMUM_SAFETY_FACTOR:N1}");
            sb.AppendLine("-----------------------------------------------------------");
            sb.AppendLine("LUG CHARACTERISTICS");
            sb.AppendLine("-----------------------------------------------------------");
            sb.AppendLine($"Lug ID               : {lug.LugID}");
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
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine("CAPACITY RESULTS");
        sb.AppendLine("-----------------------------------------------------------");
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
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine();

        sb.AppendLine($"Report generated: {DateTime.Now:dd-MM-yyyy HH:mm:ss}");

        return sb.ToString();
    }
}

