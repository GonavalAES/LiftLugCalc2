using LiftLugCalc2.Core.Models;

using System.Text;

namespace LiftLugCalc2.Core.Utilities;

public static class ReportFormatter
{
    public static string BuildCalculationReport(
        ProjectInput project,
        CalculationResult calcResult)
    {
        var sb = new StringBuilder();

        sb.AppendLine("╔═════════════════════════════════════════════════════════╗");
        sb.AppendLine("║             LIFTING LUG CALCULATION REPORT              ║");
        sb.AppendLine("╚═════════════════════════════════════════════════════════╝");
        sb.AppendLine();

        sb.AppendLine("PROJECT INFORMATION");
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine($"Project Name    : {project.Name}");
        sb.AppendLine($"Created By      : {project.CreatedBy}");
        sb.AppendLine($"Date            : {FormatDate(project.Date)}");
        sb.AppendLine($"Revision        : {project.Revision}");
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine();

        sb.AppendLine("LOAD CONFIGURATION");
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine($"Load            : {project.WLL:N1} kg");
        sb.AppendLine($"Number of Points: {project.NumberPoints}");
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine();

        // Add any lug/material info you decide

        sb.AppendLine("CALCULATION RESULTS");
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine($"Analysis Type   : {calcResult.CalculationType}");
        sb.AppendLine($"Applied Load    : {calcResult.AppliedLoad:N1} kN");
        sb.AppendLine($"Overall Result  : {(calcResult.Pass ? "PASSED" : "FAILED")}");
        sb.AppendLine($"Minimum FS      : {calcResult.MinimumFS:N1}");
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine();

        sb.AppendLine("CAPACITY RESULTS");
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine("Tension:");
        sb.AppendLine($"  Net Section Area : {calcResult.NetSectionArea:N1} mm²");
        sb.AppendLine($"  Capacity         : {calcResult.TensionCapacity:N1} kN");
        sb.AppendLine($"  Factor of Safety : {calcResult.FSTension:N1}");
        sb.AppendLine();

        sb.AppendLine("Shear:");
        sb.AppendLine($"  Capacity         : {calcResult.ShearCapacity:N1} kN");
        sb.AppendLine($"  Factor of Safety : {calcResult.FSShear:N1}");
        sb.AppendLine();

        sb.AppendLine("Bearing:");
        sb.AppendLine($"  Capacity         : {calcResult.BearingCapacity:N1} kN");
        sb.AppendLine($"  Factor of Safety : {calcResult.FSBearing:N1}");
        sb.AppendLine();

        sb.AppendLine("Tear-Out:");
        sb.AppendLine($"  Capacity         : {calcResult.TearOutCapacity:N1} kN");
        sb.AppendLine($"  Factor of Safety : {calcResult.FSTearOut:N1}");
        sb.AppendLine();

        if (calcResult.WeldCapacity > 0)
        {
            sb.AppendLine("Weld:");
            sb.AppendLine($"  Capacity         : {calcResult.WeldCapacity:N1} kN");
            sb.AppendLine($"  Factor of Safety : {calcResult.FSWeld:N1}");
            sb.AppendLine($"  Geometry Check   : {(calcResult.WeldGeometryOK ? "OK" : "NOT OK")}");
            sb.AppendLine();
        }
        sb.AppendLine("-----------------------------------------------------------");
        sb.AppendLine();

        sb.AppendLine("╔═════════════════");
        sb.AppendLine($"║Report generated: {DateTime.Now:dd-MM-yyyy HH:mm:ss}");
        sb.AppendLine("╚═════════════════");

        return sb.ToString();
    }

    public static string FormatDate(string dateString)
    {
        return DateTime.TryParse(dateString, out var parsed) ? parsed.ToString("dd-MM-yyyy") : DateTime.Today.ToString("dd-MM-yyyy");
    }
}
