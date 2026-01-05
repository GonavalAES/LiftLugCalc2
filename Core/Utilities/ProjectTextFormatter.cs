using LiftLugCalc2.Core.Models;

using System.Text;

namespace LiftLugCalc2.Core.Utilities;

public static class ProjectTextFormatter
{
    public static string BuildProjectDataText(ProjectInput project)
    {
        var sb = new StringBuilder();

        sb.AppendLine("╔═════════════════════════════════════════════════════════╗");
        sb.AppendLine("║             LIFTING LUG PROJECT DATA FILE               ║");
        sb.AppendLine("╚═════════════════════════════════════════════════════════╝");
        sb.AppendLine();

        sb.AppendLine("[PROJECT INFORMATION]");
        sb.AppendLine($"Project ID   : {project.ProjectID}");
        sb.AppendLine($"Project Name : {project.Name}");
        sb.AppendLine($"Created by   : {project.CreatedBy}");
        sb.AppendLine($"Date         : {project.Date}");
        sb.AppendLine($"Revision     : {project.Revision}");
        sb.AppendLine();

        sb.AppendLine("[LOAD DATA]");
        sb.AppendLine($"Load to be Lifted : {project.WLL}");
        sb.AppendLine($"Number of Points  : {project.NumberPoints}");
        sb.AppendLine("Lifting Geometry:");
        if (project.NumberPoints == 1)
        {
            sb.AppendLine(" -> Single vertical lifting point.");
        }
        if (project.NumberPoints >= 2)
        {
            sb.AppendLine($" -> A1 : {project.A1}");
            sb.AppendLine($" -> A2 : {project.A2}");
        }
        if (project.NumberPoints >= 3)
        {
            sb.AppendLine($" -> B1 : {project.B1}");
        }
        if (project.NumberPoints == 4)
        {
            sb.AppendLine($" -> B2 : {project.B2}");
        }
        sb.AppendLine();

        sb.AppendLine("╔═════════════════");
        sb.AppendLine($"║File saved at: {DateTime.Now:dd-MM-yyyy HH:mm:ss}");
        sb.AppendLine("╚═════════════════");

        return sb.ToString();
    }

    public static ProjectInput ParseProjectDataText(string content)
    {
        var project = new ProjectInput();
        var culture = new System.Globalization.CultureInfo("pt-PT");

        var lines = content.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

        foreach (var line in lines)
        {
            if (!line.Contains('=')) continue;

            var parts = line.Split('=');
            if (parts.Length != 2) continue;

            string key = parts[0].Trim();
            string value = parts[1].Trim();

            switch (key)
            {
                case "Project ID":
                    project.ProjectID = int.Parse(value);
                    break;
                case "Project Name":
                    project.Name = value;
                    break;
                case "Created by":
                    project.CreatedBy = value;
                    break;
                case "Date":
                    project.Date = value;
                    break;
                case "Revision":
                    project.Revision = value;
                    break;
                case "Load to be Lifted":
                    project.WLL = double.Parse(value, culture);
                    break;
                case "Number of Points":
                    project.NumberPoints = int.Parse(value);
                    break;
                case "A1":
                    project.A1 = double.Parse(value, culture);
                    break;
                case "A2":
                    project.A2 = double.Parse(value, culture);
                    break;
                case "B1":
                    project.B1 = double.Parse(value, culture);
                    break;
                case "B2":
                    project.B2 = double.Parse(value, culture);
                    break;
            }
        }

        return project;
    }
}
