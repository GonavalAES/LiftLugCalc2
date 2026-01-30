using LiftLugCalc2.Core.Engine;
using LiftLugCalc2.Core.FileOperations;
using LiftLugCalc2.Core.Models;
using LiftLugCalc2.Core.Utilities;

namespace LiftLugCalc2.ConsoleFrontEnd;

public sealed class ConsoleUI
{
    private readonly IReadOnlyList<TableLug> _lugs;
    private readonly IReadOnlyList<Material> _materials;

    public ConsoleUI(IReadOnlyList<TableLug> lugs, IReadOnlyList<Material> materials)
    {
        _lugs = lugs;
        _materials = materials;
    }

    public void Run()
    {
        while (true)
        {
            UICommon.UIMainMenu();
            var choice = (Console.ReadLine() ?? "").Trim();

            switch (choice)
            {
                case "1":
                    CreateAndRunProject();
                    break;
                case "2":
                    OpenAndRunProject();
                    break;
                /*
                                case "3":
                                    UICommon.MessageWarning("Test Application not implemented yet.");
                                    UICommon.PromptToContinue();
                                    break;
                */
                case "3":
                case "q":
                case "Q":
                    return;
                default:
                    UICommon.MessageWarning("-> Invalid option.");
                    UICommon.PromptToContinue();
                    break;
            }
        }
    }

    private void CreateAndRunProject()
    {
        UICommon.NewProjectHeader();

        var project = ReadInitialData();
        double nominalWeightKg, wcf;

        ApplyWeightAndWcf(project, out nominalWeightKg, out wcf);
        ReadLiftGeometry(project);
        var result = ChooseCalculationTypeAndRun(project);

        if (result == null)
        {
            UICommon.MessageWarning("No calculation was performed. Exiting calculations.");
            UICommon.PromptToContinue();
            return;
        }

        Console.WriteLine();
        UICommon.DrawSeparator();
    }

    private void OpenAndRunProject()
    {
        var projectNames = FilingSystem.GetAllProjectNames();
        if (projectNames.Count == 0)
        {
            UICommon.MessageWarning("No saved projects found.");
            UICommon.PromptToContinue();
            return;
        }

        Console.WriteLine("> Available projects:");
        for (int i = 0; i < projectNames.Count; i++)
            Console.WriteLine($"{i + 1}. {projectNames[i]}");

        int choice = UICommon.PromptInt("> Select project number:");
        if (choice < 1 || choice > projectNames.Count)
        {
            UICommon.MessageWarning("Invalid selection.");
            UICommon.PromptToContinue();
            return;
        }

        string? projectName = projectNames[choice - 1];
        var project = LoadProject(projectName!);
        if (project == null) return;

        ChooseCalculationTypeAndRun(project);
    }

    private Project? LoadProject(string projectName)
    {
        string dataDir = FilingSystem.GetProjectDirectory(projectName);
        string projectFile = Path.Combine(dataDir, "project.txt");

        string content = FilingSystem.LoadTextFile(projectFile);
        UICommon.MessageLoadProject();

        if (string.IsNullOrWhiteSpace(content))
        {
            UICommon.MessageError("Project file is empty or missing.");
            return null!;
        }

        var project = Formatter.ProjectFormatter.FromText(content);

        if (project.UserLugID.HasValue || project.UserMaterialID.HasValue)
        {
            Console.WriteLine();
            Console.WriteLine("> Stored selection in project file:");

            if (project.UserMaterialID.HasValue)
            {
                var mat = _materials.FirstOrDefault(m => m.MaterialID == project.UserMaterialID.Value);
                if (mat is not null) Console.WriteLine($" MaterialID : {mat.MaterialID}, {mat.Designation}");
            }

            if (project.UserLugID.HasValue)
            {
                var lug = _lugs.FirstOrDefault(l => l.LugID == project.UserLugID.Value);
                if (lug is not null) Console.WriteLine($" LugID : {lug.LugID}, Type : {lug.LugType}, WLL : {lug.LugWLL} [kg]");
            }

            Console.WriteLine();
        }

        return project;
    }

    private Project ReadInitialData()
    {
        var project = new Project { ProjectID = 0 };

        project.Name = UICommon.PromptRequired("> Project name:");
        project.CreatedBy = UICommon.PromptOptional("> Created by:");
        project.Revision = UICommon.PromptOptional("> Revision:");

        project.Date = DateTime.Today.ToString("dd-MM-yyyy");

        UICommon.DrawSeparator();

        return project;
    }

    private void ApplyWeightAndWcf(Project project, out double nominalWeightKg, out double wcf)
    {
        while (true)
        {
            UICommon.PromptForWeight();

            string basisChoice = Console.ReadLine()?.Trim() ?? "";

            switch (basisChoice)
            {
                case "1":
                    nominalWeightKg = UICommon.PromptDouble("> Actual object weight [kg]:");
                    break; // out of the 'switch' to the next step
                case "2":
                    nominalWeightKg = UICommon.PromptDouble("> Working Load Limit (WLL) [kg]:");
                    break; // out of the 'switch' to the next step
                default:
                    UICommon.MessageWarning("Invalid choice (1 or 2).");
                    continue; // back to the beginning of the 'while' loop
            }

            Console.WriteLine();
            UICommon.DrawSeparator();

            break; // out of the 'while' loop
        }

        while (true)
        {
            UICommon.WeightCalculationFactor();

            string wcfChoice = Console.ReadLine() ?? "";

            wcf = PreliminaryCalculations.ChoosingWCF(wcfChoice);

            if (wcfChoice is "1" or "2" or "3" or "4") break;

            UICommon.MessageWarning("Invalid. Press Enter to accept default:");
            if (Console.ReadKey(intercept: true).Key == ConsoleKey.Enter) break;
        }

        Console.WriteLine();
        project.WLL = nominalWeightKg * wcf;
    }

    private void ReadLiftGeometry(Project project)
    {
        while (true)
        {
            UICommon.NumberPointsHeader();

            project.NumberPoints = UICommon.PromptInt("> Number of lifting points (1–4):");
            if (project.NumberPoints is >= 1 and <= 4) break;

            UICommon.MessageWarning("Must be 1-4.");
        }

        Console.WriteLine();
        UICommon.LiftGeometryHeader();
        switch (project.NumberPoints)
        {
            case 1:
                Console.WriteLine("> Single point lift - no geometry adjustment needed.");
                project.A1 = 0.0;
                project.A2 = 0.0;
                project.B1 = 0.0;
                project.B2 = 0.0;
                Console.WriteLine();
                break;

            case 2:
                UICommon.TwoPointLift();
                project.A1 = UICommon.PromptDouble("> Input value of A1 [m]");
                project.A2 = UICommon.PromptDouble("> Input value of A2 [m]");
                Console.WriteLine();
                break;

            case 3:
                UICommon.ThreePointLift();
                project.A1 = UICommon.PromptDouble("> Input value of A1 [m]");
                project.A2 = UICommon.PromptDouble("> Input value of A2 [m]");
                project.B1 = UICommon.PromptDouble("> Input value of B1 [m]");
                Console.WriteLine();
                break;

            case 4:
                UICommon.FourPointLift();
                project.A1 = UICommon.PromptDouble("> Input value of A1 [m]");
                project.A2 = UICommon.PromptDouble("> Input value of A2 [m]");
                project.B1 = UICommon.PromptDouble("> Input value of B1 [m]");
                project.B2 = UICommon.PromptDouble("> Input value of B2 [m]");
                Console.WriteLine();
                break;
        }
    }

    private CalculationResult? ChooseCalculationTypeAndRun(Project project)
    {
        UICommon.PromptTypeCalculation();

        var choice = Console.ReadLine()?.Trim();
        Console.WriteLine();

        if (choice == "1") return RunForwardCalculation(project);
        else if (choice == "2") return RunReverseCalculation(project, _lugs);
        else
        {
            UICommon.MessageWarning("-> Invalid type.");
            return null;
        }
    }

    private CalculationResult RunForwardCalculation(Project project)
    {
        var material = SelectMaterial();
        var lug = SelectLug();

        project.SelectedLug = lug;
        project.SelectedMaterial = material;

        var input = new ForwardInput(project, lug, material);
        var result = ForwardCalculator.Run(input);

        ShowForwardResult(project, lug, material, result);
        ShowPostCalculationMenu(project, result);

        Console.WriteLine();
        return result;
    }

    private CalculationResult RunReverseCalculation(Project project, IReadOnlyList<TableLug> lugs)
    {
        var material = SelectMaterial();
        var input = new ReverseInput(project, material, lugs);
        var selection = ReverseCalculator.Run(input);

        if (selection.Best is not null)
        {
            project.SelectedLug = selection.Best.Lug;
            project.SelectedMaterial = material;
        }

        var result = selection.Best?.Result;

        ShowReverseResult(project, material, selection);
        ShowPostCalculationMenu(project, result!);

        Console.WriteLine();
        return result!;
    }

    private Material SelectMaterial()
    {
        UICommon.MaterialsHeader();
        foreach (var m in _materials)
            Console.WriteLine($"{m.MaterialID,3} | {m.Designation,-12} | {m.YieldStrength,8:N0} | {m.TensileStrength,8:N0}");

        while (true)
        {
            int matId = UICommon.PromptInt("Select Material ID");
            Console.WriteLine();

            var material = _materials.FirstOrDefault(m => m.MaterialID == matId);
            if (material is not null) return material;

            UICommon.MessageWarning("> Invalid Material ID. Please select one of the listed IDs.");
        }
    }

    private TableLug SelectLug()
    {
        UICommon.LugTableHeader();
        foreach (var l in _lugs)
            Console.WriteLine($"{l.LugID,3} | {l.LugType,4} | {l.LugWLL,10:N1}");

        while (true)
        {
            int lugId = UICommon.PromptInt("Select Lug ID");
            Console.WriteLine();

            var lug = _lugs.FirstOrDefault(l => l.LugID == lugId);
            if (lug is not null)
            {
                UICommon.DrawLugType(lug.LugType);
                return lug;
            }

            UICommon.MessageWarning("> Invalid Lug ID. Please select one of the listed IDs.");
        }
    }

    private void ShowPostCalculationMenu(Project project, CalculationResult result)
    {
        while (true)
        {
            UICommon.FinalChoicesMenu();

            var choice = Console.ReadLine()?.Trim();

            switch (choice)
            {
                case "1":
                    ShowDetailedReport(project, result);
                    break;
                case "2":
                    SaveReport(project, result);
                    break;
                case "3":
                    SaveProject(project);
                    break;
                case "4":
                    CreateAndRunProject();
                    break;
                case "q":
                case "Q":
                    return;
                default:
                    UICommon.MessageWarning("Invalid choice. Try again.");
                    break;
            }
        }
    }

    private void ShowCommonInput(Project project, Material material, CalculationResult result)
    {
        Console.WriteLine($"> Project  : {project.Name}");
        Console.WriteLine($"> WLL      : {project.WLL:N1} kg (design)");
        Console.WriteLine($"> Material : {material.Designation} (fy = {material.YieldStrength} MPa)");
        Console.WriteLine($"> Applied  : {result.AppliedLoad:N1} kN");
        Console.WriteLine();
    }


    private void ShowForwardResult(Project project, TableLug lug, Material material, CalculationResult result)
    {
        UICommon.ResultsHeader();
        ShowCommonInput(project, material, result);

        Console.WriteLine($"> Lug      : ID {lug.LugID}, Type {lug.LugType}, WLL = {lug.LugWLL} kg");
        UICommon.DrawLugType(lug.LugType);

        Console.WriteLine();
        Console.WriteLine($"> FS Tension : {result.FSTension:N1}");
        Console.WriteLine($"> FS Shear   : {result.FSShear:N1}");
        Console.WriteLine($"> FS Bearing : {result.FSBearing:N1}");
        Console.WriteLine($"> FS Tearout : {result.FSTearOut:N1}");
        Console.WriteLine($"> FS Weld    : {result.FSWeld:N1}");
        Console.WriteLine($"> Min FS     : {result.MinimumFS:N1}");
        Console.WriteLine();

        if (result.Pass) UICommon.OverallResultPass();
        else UICommon.OverallResultFail();
        Console.WriteLine();
    }

    private void ShowReverseResult(Project project, Material material, ReverseSelection selection)
    {
        UICommon.ResultsHeader();
        ShowCommonInput(project, material, selection.Best?.Result!);
        Console.WriteLine();

        if (selection.Best is null)
        {
            UICommon.MessageError("-> No suitable lug found in table for this load/material.");
            return;
        }

        var best = selection.Best;

        Console.WriteLine($"> Suggested Lug ID: {best.Lug.LugID}, Type {best.Lug.LugType}, WLL = {best.Lug.LugWLL} kg");
        UICommon.DrawLugType(best.Lug.LugType);

        Console.WriteLine();
        Console.WriteLine($"> Minimum FS      : {best.Result.MinimumFS:N1}");
        Console.WriteLine($"> Result          : {(best.Result.Pass ? "PASS" : "FAIL")}");
        Console.WriteLine();

        if (best.Result.Pass) UICommon.OverallResultPass();
        else UICommon.OverallResultFail();
        Console.WriteLine();
    }

    private void ShowDetailedReport(Project project, CalculationResult result)
    {
        UICommon.DrawSeparator();
        Console.WriteLine(ReportGenerator.GenerateDetailedReport(project, result));

        Console.WriteLine("\nPress any key to return to menu...");
        Console.ReadKey();
    }

    private void SaveReport(Project project, CalculationResult result)
    {
        FilingSystem.EnsureProjectDirectory(project.Name);

        string resultsDir = FilingSystem.GetProjectDirectory(project.Name);
        string timestamp = DateTime.Now.ToString("dd-MM-yyyy");
        string reportFile = Path.Combine(resultsDir, $"Report_{timestamp}.txt");
        string reportText = ReportGenerator.GenerateDetailedReport(project, result);

        FilingSystem.SaveTextFile(reportFile, reportText);

        UICommon.MessageSuccess($"-> Report saved to: {reportFile}");
        Console.WriteLine();
    }

    private void SaveProject(Project project)
    {
        FilingSystem.EnsureProjectDirectory(project.Name);

        string dataDir = FilingSystem.GetProjectDirectory(project.Name);
        string projectFile = Path.Combine(dataDir, "project.txt");
        string text = Formatter.ProjectFormatter.ToText(project);

        FilingSystem.SaveTextFile(projectFile, text);

        UICommon.MessageSuccess($"Project saved to: {projectFile}");
        Console.WriteLine();
    }
}
