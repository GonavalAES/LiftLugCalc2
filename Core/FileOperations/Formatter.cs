using LiftLugCalc2.Core.Models;

using System.Globalization;
using System.Text;

namespace LiftLugCalc2.Core.FileOperations;

public static class Formatter
{
    public static class ProjectFormatter
    {
        public static string ToText(Project project)
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
            sb.AppendLine($"WeightBasis={project.WeightBasis}");
            sb.AppendLine($"NominalWeightKg={project.NominalWeightKg.ToString(CultureInfo.InvariantCulture)}");
            sb.AppendLine($"WCFSelection={project.WcfSelection}");
            sb.AppendLine($"WLL={project.WLL}");
            sb.AppendLine($"NumberPoints={project.NumberPoints}");
            sb.AppendLine($"A1={project.A1}");
            sb.AppendLine($"A2={project.A2}");
            sb.AppendLine($"B1={project.B1}");
            sb.AppendLine($"B2={project.B2}");

            sb.AppendLine("[LUG & MATERIAL SELECTION]");
            sb.AppendLine($"UserLugID={project.UserLugID?.ToString() ?? ""}");
            sb.AppendLine($"UserMaterialID={project.UserMaterialID?.ToString() ?? ""}");

            return sb.ToString();
        }

        public static Project FromText(string content)
        {
            var project = new Project();

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
                    case "ProjectName":
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
                    case "WeightBasis":
                        project.WeightBasis = int.Parse(value);
                        break;
                    case "NominalWeightKg":
                        project.NominalWeightKg = double.Parse(value, CultureInfo.InvariantCulture);
                        break;
                    case "WCFSelection":
                        project.WcfSelection = int.Parse(value);
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
                        if (!string.IsNullOrWhiteSpace(value)) project.UserLugID = int.Parse(value);
                        break;
                    case "UserMaterialID":
                        if (!string.IsNullOrWhiteSpace(value)) project.UserMaterialID = int.Parse(value);
                        break;
                }
            }

            return project;
        }
    }

    public static string FormatDate(string dateString)
        => DateTime.TryParse(dateString, out var parsed) ? parsed.ToString("dd-MM-yyyy") : DateTime.Today.ToString("dd-MM-yyyy");
}
