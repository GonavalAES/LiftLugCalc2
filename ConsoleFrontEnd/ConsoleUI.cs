using LiftLugCalc2.Core.Engine;
using LiftLugCalc2.Core.Models;

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
            Console.Clear();
            ShowMainMenu();
            var choice = (Console.ReadLine() ?? "").Trim();

            switch (choice)
            {
                case "1":
                    RunForwardCalculation();
                    break;
                case "2":
                    RunReverseCalculation();
                    break;
                case "q":
                case "Q":
                    return;
                default:
                    Console.WriteLine("Invalid option. Press any key...");
                    Console.ReadKey();
                    break;
            }
        }
    }

    private void ShowMainMenu()
    {
        Console.WriteLine("=== Lifting Lug Calculator ===");
        Console.WriteLine("1) Forward calculation (check selected lug)");
        Console.WriteLine("2) Reverse calculation (suggest lug)");
        Console.WriteLine("Q) Quit");
        Console.Write("Select option: ");
    }

    private void RunForwardCalculation()
    {
        // 1. Read ProjectInput from user
        var project = ReadProjectInputFromConsole();

        // 2. Let user pick material and lug by ID
        var material = SelectMaterial();
        var lug = SelectLug();

        // 3. Build ForwardInput and run core calculation
        var input = new ForwardInput(project, lug, material);
        var result = ForwardCalculator.Run(input);

        // 4. Show results
        PrintForwardResult(project, lug, material, result);

        Console.WriteLine("Press any key to return to menu...");
        Console.ReadKey();
    }

    private void RunReverseCalculation()
    {
        var project = ReadProjectInputFromConsole();
        var material = SelectMaterial();

        var input = new ReverseInput(project, material, _lugs);
        var selection = ReverseCalculator.Run(input);

        PrintReverseResult(project, material, selection);

        Console.WriteLine("Press any key to return to menu...");
        Console.ReadKey();
    }

    // --- Helpers below: reading input and printing ---

    private ProjectInput ReadProjectInputFromConsole()
    {
        var project = new ProjectInput();

        Console.Write("Project name: ");
        project.Name = Console.ReadLine() ?? string.Empty;

        Console.Write("WLL [kg]: ");
        project.WLL = double.Parse(Console.ReadLine() ?? "0");

        Console.Write("Number of lifting points (1–4): ");
        project.NumberPoints = int.Parse(Console.ReadLine() ?? "1");

        if (project.NumberPoints >= 2)
        {
            Console.Write("A1 [m]: ");
            project.A1 = double.Parse(Console.ReadLine() ?? "0");
            Console.Write("A2 [m]: ");
            project.A2 = double.Parse(Console.ReadLine() ?? "0");
        }
        if (project.NumberPoints >= 3)
        {
            Console.Write("B1 [m]: ");
            project.B1 = double.Parse(Console.ReadLine() ?? "0");
        }
        if (project.NumberPoints >= 4)
        {
            Console.Write("B2 [m]: ");
            project.B2 = double.Parse(Console.ReadLine() ?? "0");
        }

        return project;
    }

    private Material SelectMaterial()
    {
        Console.WriteLine("\nAvailable materials:");
        foreach (var m in _materials)
            Console.WriteLine($"{m.MaterialID}: {m.Designation} (fy={m.YieldStrength} MPa)");

        Console.Write("Select Material ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        return _materials.First(m => m.MaterialID == id);
    }

    private TableLug SelectLug()
    {
        Console.WriteLine("\nAvailable lugs:");
        foreach (var l in _lugs)
            Console.WriteLine($"{l.LugID}: Type {l.LugType}, WLL={l.LugWLL}");

        Console.Write("Select Lug ID: ");
        int id = int.Parse(Console.ReadLine() ?? "0");

        return _lugs.First(l => l.LugID == id);
    }

    private void PrintForwardResult(ProjectInput project, TableLug lug, Material material, CalculationResult result)
    {
        Console.WriteLine("\n--- Forward Calculation Result ---");
        Console.WriteLine($"Project   : {project.Name}");
        Console.WriteLine($"Lug       : ID {lug.LugID}, Type {lug.LugType}, WLL={lug.LugWLL}");
        Console.WriteLine($"Material  : {material.Designation} (fy = {material.YieldStrength} MPa)");
        Console.WriteLine($"Applied   : {result.AppliedLoad:N1} kN");
        Console.WriteLine($"FS (Tens) : {result.FSTension:N1}");
        Console.WriteLine($"FS (Shear): {result.FSShear:N1}");
        Console.WriteLine($"FS (Bear) : {result.FSBearing:N1}");
        Console.WriteLine($"FS (Tear) : {result.FSTearOut:N1}");
        Console.WriteLine($"FS (Weld) : {result.FSWeld:N1}");
        Console.WriteLine($"Min FS    : {result.MinimumFS:N1}");
        Console.WriteLine($"Result    : {(result.Pass ? "PASS" : "FAIL")}");
    }

    private void PrintReverseResult(ProjectInput project, Material material, ReverseSelection selection)
    {
        Console.WriteLine("\n--- Reverse Calculation Result ---");
        Console.WriteLine($"Project   : {project.Name}");
        Console.WriteLine($"WLL       : {project.WLL:N1} kg");
        Console.WriteLine($"Material  : {material.Designation} (fy={material.YieldStrength} MPa)");
        Console.WriteLine($"Applied   : {selection.AppliedLoad:N1} kN");

        if (selection.Best is null)
        {
            Console.WriteLine("No suitable lug found in table for this load and material.");
            return;
        }

        var best = selection.Best;
        Console.WriteLine($"Suggested Lug ID {best.Lug.LugID} (Type {best.Lug.LugType}, WLL={best.Lug.LugWLL})");
        Console.WriteLine($"Min FS    : {best.Result.MinimumFS:N1}");
        Console.WriteLine($"Result    : {(best.Result.Pass ? "PASS" : "FAIL")}");
    }
}
