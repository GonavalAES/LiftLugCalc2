using LiftLugCalc2.Core.Engine;
using LiftLugCalc2.Core.FileOperations;
using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.ConsoleFrontEnd
{
    public static class Program
    {
        static void Main(string[] args)
        {
            // Splash screen
            UICommon.UISplash();

            // Load reference data
            UICommon.MessageLoadRefData();

            var lugList = TableLugLoader.LoadFromCsv();
            var materialList = MaterialLoader.LoadFromCsv();

            UICommon.MessageGood("Reference data loaded.");

            // Main loop
            bool exit = false;
            while (!exit)
            {
                //Console.Clear();

                UICommon.UISplash();
                UICommon.UIMainMenu();

                Console.Write("\n> Select an option: ");

                var choice = (Console.ReadLine() ?? "").Trim();
                switch (choice)
                {
                    case "1":
                        CreateAndRunProject(lugList, materialList);
                        break;

                    case "2":
                        // TODO: Load existing project (later)
                        UICommon.MessageWarning("Open Project not implemented yet.");
                        UICommon.PromptToContinue();
                        break;

                    case "3":
                        // Simple test hook: later you can call your Tests here if you want
                        UICommon.MessageWarning("Test Application not implemented yet.");
                        UICommon.PromptToContinue();
                        break;

                    case "4":
                    case "q":
                    case "Q":
                        exit = true;
                        break;

                    default:
                        UICommon.MessageWarning("Invalid option. Please try again.");
                        UICommon.PromptToContinue();
                        break;
                }
            }

            UICommon.ExitApplication();
        }

        private static void CreateAndRunProject(IReadOnlyList<TableLug> lugs, IReadOnlyList<Material> materials)
        {
            //Console.Clear();
            UICommon.NewProjectHeader();

            // 1. Gather project input (minimal version for now)
            var project = new ProjectInput
            {
                ProjectID = 0
            };

            project.Name = UICommon.PromptRequired("Project name");
            project.CreatedBy = UICommon.PromptOptional("Created by");
            project.Date = DateTime.Today.ToString("dd-MM-yyyy");
            project.Revision = UICommon.PromptOptional("Revision");

            UICommon.DrawSeparator();
            UICommon.PromptForWeight();

            var basisChoice = (Console.ReadLine() ?? "").Trim();

            // For now, just ask WLL in kg directly
            project.WLL = UICommon.PromptDouble("Working Load Limit [kg]");

            UICommon.NumberPointsHeader();

            project.NumberPoints = UICommon.PromptInt("Number of lifting points (1–4)");

            if (project.NumberPoints >= 2)
            {
                UICommon.LiftGeometryHeader();
                UICommon.TwoPointLift();

                project.A1 = UICommon.PromptDouble("A1 [m]");
                project.A2 = UICommon.PromptDouble("A2 [m]");
            }
            if (project.NumberPoints >= 3)
            {
                UICommon.ThreePointLift();

                project.B1 = UICommon.PromptDouble("B1 [m]");
            }
            if (project.NumberPoints >= 4)
            {
                UICommon.FourPointLift();

                project.B2 = UICommon.PromptDouble("B2 [m]");
            }

            // 2. Select calculation type (forward / reverse)
            UICommon.PromptTypeCalculation();

            Console.Write("\n> Choice: ");
            var calcChoice = (Console.ReadLine() ?? "").Trim();

            if (calcChoice == "1") RunForwardCalculation(project, lugs, materials);
            else if (calcChoice == "2") RunReverseCalculation(project, lugs, materials);
            else
            {
                UICommon.MessageWarning("Invalid calculation type.");
                UICommon.PromptToContinue();
            }
        }

        private static void RunForwardCalculation(ProjectInput project, IReadOnlyList<TableLug> lugs, IReadOnlyList<Material> materials)
        {
            // 1. Select material
            UICommon.MaterialsHeader();

            foreach (var m in materials)
                Console.WriteLine($"{m.MaterialID,3} | {m.Designation,-12} | {m.YieldStrength,8:N0} | {m.TensileStrength,8:N0}");

            int matId = UICommon.PromptInt("Select Material ID");
            var material = materials.First(m => m.MaterialID == matId);

            // 2. Select lug
            UICommon.LugTableHeader();

            foreach (var l in lugs) Console.WriteLine($"{l.LugID,3} | {l.LugType,4} | {l.LugWLL,10:N1}");

            int lugId = UICommon.PromptInt("Select Lug ID");
            var lug = lugs.First(l => l.LugID == lugId);

            // 3. Weight correction factor
            UICommon.WeightCalculationFactor();

            string wcfChoice = Console.ReadLine() ?? "";
            double wcf = PreliminaryCalculations.ChoosingWCF(wcfChoice);

            double designWll = project.WLL * wcf;

            // 4. Run forward calc
            var projectForCalc = project with { WLL = designWll };
            var input = new ForwardInput(projectForCalc, lug, material);
            var result = ForwardCalculator.Run(input);

            // 5. Show results
            UICommon.ResultsHeader();

            Console.WriteLine($"> Project : {project.Name}");
            Console.WriteLine($"> Lug     : ID {lug.LugID}, Type {lug.LugType}, WLL={lug.LugWLL} kg");
            Console.WriteLine($"> Material: {material.Designation} (fy={material.YieldStrength} MPa)");
            Console.WriteLine($"> Applied : {result.AppliedLoad:N1} kN");
            Console.WriteLine();
            Console.WriteLine($"FS Tension : {result.FSTension:N2}");
            Console.WriteLine($"FS Shear   : {result.FSShear:N2}");
            Console.WriteLine($"FS Bearing : {result.FSBearing:N2}");
            Console.WriteLine($"FS Tearout : {result.FSTearOut:N2}");
            Console.WriteLine($"FS Weld    : {result.FSWeld:N2}");
            Console.WriteLine($"Min FS     : {result.MinimumFS:N2}");
            Console.WriteLine();

            if (result.Pass) UICommon.OverallResultPass();
            else UICommon.OverallResultFail();

            UICommon.PromptToContinue();
        }

        private static void RunReverseCalculation(ProjectInput project, IReadOnlyList<TableLug> lugs, IReadOnlyList<Material> materials)
        {
            // 1. Select material
            UICommon.MaterialsHeader();

            foreach (var m in materials)
                Console.WriteLine($"{m.MaterialID,3} | {m.Designation,-12} | {m.YieldStrength,8:N0} | {m.TensileStrength,8:N0}");

            int matId = UICommon.PromptInt("Select Material ID");
            var material = materials.First(m => m.MaterialID == matId);

            // 2. Weight correction factor
            UICommon.WeightCalculationFactor();

            string wcfChoice = Console.ReadLine() ?? "";
            double wcf = PreliminaryCalculations.ChoosingWCF(wcfChoice);

            double designWll = project.WLL * wcf;

            var projectForCalc = project with { WLL = designWll };

            // 3. Run reverse calc
            var input = new ReverseInput(projectForCalc, material, lugs);
            var selection = ReverseCalculator.Run(input);

            // 4. Show result
            UICommon.ResultsHeader();

            Console.WriteLine($"> Project : {project.Name}");
            Console.WriteLine($"> WLL     : {project.WLL:N1} kg (design WLL = {designWll:N1} kg)");
            Console.WriteLine($"> Material: {material.Designation} (fy={material.YieldStrength} MPa)");
            Console.WriteLine($"> Applied : {selection.AppliedLoad:N1} kN");
            Console.WriteLine();

            if (selection.Best is null)
            {
                UICommon.MessageError("No suitable lug found in table for this load/material.");
                UICommon.PromptToContinue();
                return;
            }

            var best = selection.Best;

            Console.WriteLine($"Suggested Lug ID: {best.Lug.LugID}, Type {best.Lug.LugType}, WLL = {best.Lug.LugWLL} kg");
            Console.WriteLine($"Minimum FS      : {best.Result.MinimumFS:N2}");
            Console.WriteLine($"Result          : {(best.Result.Pass ? "PASS" : "FAIL")}");
            Console.WriteLine();

            if (best.Result.Pass) UICommon.OverallResultPass();
            else UICommon.OverallResultFail();

            UICommon.PromptToContinue();
        }
    }
}
