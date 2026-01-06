using LiftLugCalc2.Core.Models;

using System.Globalization;
using System.Text;

namespace LiftLugCalc2.Core.FileOperations;

public static class Formatter
{
    public static class ProjectFormatter
    {
        public static string ToText(ProjectInput project)
        {
            var sb = new StringBuilder();

            sb.AppendLine("LIFTING LUG PROJECT DATA");
            sb.AppendLine("[PROJECT]");
            sb.AppendLine($"ProjectID={project.ProjectID}");
            sb.AppendLine($"ProjectName={project.Name}");
            sb.AppendLine($"CreatedBy={project.CreatedBy}");
            sb.AppendLine($"Date={project.Date}");
            sb.AppendLine($"Revision={project.Revision}");
            sb.AppendLine();

            sb.AppendLine("[LOAD]");
            sb.AppendLine($"WLL={project.WLL}");
            sb.AppendLine($"NumberPoints={project.NumberPoints}");
            sb.AppendLine($"A1={project.A1}");
            sb.AppendLine($"A2={project.A2}");
            sb.AppendLine($"B1={project.B1}");
            sb.AppendLine($"B2= {project.B2}");

            sb.AppendLine("[LUG & MATERIAL SELECTION]");
            sb.AppendLine($"UserLugID={project.UserLugID?.ToString() ?? ""}");
            sb.AppendLine($"UserMaterialID={project.UserMaterialID?.ToString() ?? ""}");

            return sb.ToString();
        }

        public static ProjectInput FromText(string content)
        {
            var project = new ProjectInput();

            var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                if (!line.Contains('=')) continue;

                var parts = line.Split('=', 2);
                string key = parts[0].Trim();
                string value = parts[1].Trim();

                switch (key)
                {
                    case "ProjectID":
                        project.ProjectID = int.Parse(value);
                        break;
                    case "Name":
                        project.Name = value;
                        break;
                    case "CreatedBy":
                        project.CreatedBy = value;
                        break;
                    case "Date":
                        project.Date = value;
                        break;
                    case "Revision":
                        project.Revision = value;
                        break;
                    case "WLL":
                        project.WLL = double.Parse(value, CultureInfo.InvariantCulture);
                        break;
                    case "NumberPoints":
                        project.NumberPoints = int.Parse(value);
                        break;
                    case "A1":
                        project.A1 = double.Parse(value, CultureInfo.InvariantCulture);
                        break;
                    case "A2":
                        project.A2 = double.Parse(value, CultureInfo.InvariantCulture);
                        break;
                    case "B1":
                        project.B1 = double.Parse(value, CultureInfo.InvariantCulture);
                        break;
                    case "B2":
                        project.B2 = double.Parse(value, CultureInfo.InvariantCulture);
                        break;
                    case "UserLugID":
                        if (!string.IsNullOrWhiteSpace(value))
                            project.UserLugID = int.Parse(value);
                        break;
                    case "UserMaterialID":
                        if (!string.IsNullOrWhiteSpace(value))
                            project.UserMaterialID = int.Parse(value);
                        break;
                }
            }

            return project;
        }

    }

    public static class ReportFormatter
    {
        public static string ForwardReport(ProjectInput project, TableLug lug, Material material, CalculationResult result)
        {
            var sb = new StringBuilder();

            sb.AppendLine("LIFTING LUG CALCULATION REPORT");
            sb.AppendLine("[PROJECT]");
            sb.AppendLine($"Name    : {project.Name}");
            sb.AppendLine($"WLL     : {project.WLL:N1} kg (design)");
            sb.AppendLine();

            sb.AppendLine("[CONFIGURATION]");
            sb.AppendLine($"Lug ID      : {lug.LugID}");
            sb.AppendLine($"Lug Type    : {lug.LugType}");
            sb.AppendLine($"Lug WLL     : {lug.LugWLL} kg");
            sb.AppendLine($"Material    : {material.Designation} (fy = {material.YieldStrength} MPa)");
            sb.AppendLine();

            sb.AppendLine("[RESULTS]");
            sb.AppendLine($"Applied Load : {result.AppliedLoad:N1} kN");
            sb.AppendLine($"FS Tension   : {result.FSTension:N1}");
            sb.AppendLine($"FS Shear     : {result.FSShear:N1}");
            sb.AppendLine($"FS Bearing   : {result.FSBearing:N1}");
            sb.AppendLine($"FS Tear-Out  : {result.FSTearOut:N1}");
            sb.AppendLine($"FS Weld      : {result.FSWeld:N1}");
            sb.AppendLine($"Min FS       : {result.MinimumFS:N1}");
            sb.AppendLine($"Pass         : {(result.Pass ? "YES" : "NO")}");

            return sb.ToString();
        }
    }
}
