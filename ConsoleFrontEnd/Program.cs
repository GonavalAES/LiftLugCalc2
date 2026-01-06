using LiftLugCalc2.Core.Engine;
using LiftLugCalc2.Core.FileOperations;
using LiftLugCalc2.Core.Models;

using static LiftLugCalc2.Core.FileOperations.Formatter;

namespace LiftLugCalc2.ConsoleFrontEnd
{
    public static class Program
    {
        static void Main(string[] args)
        {
            UICommon.UISplash();

            UICommon.MessageLoadRefData();
            var lugList = TableLugLoader.LoadFromCsv();
            var materialList = MaterialLoader.LoadFromCsv();
            UICommon.MessageGood("-> Reference data loaded.");

            RunMainLoop(lugList, materialList);

            UICommon.ExitApplication();
        }

        private static void RunMainLoop(IReadOnlyList<TableLug> lugs, IReadOnlyList<Material> materials)
        {
            bool exit = false;
            while (!exit)
            {
                UICommon.UISplash();
                UICommon.UIMainMenu();

                Console.Write("\n> Select an option: ");
                var choice = (Console.ReadLine() ?? "").Trim();

                switch (choice)
                {
                    case "1":
                        CreateAndRunProject(lugs, materials);
                        break;
                    case "2":
                        OpenAndRunProject(lugs, materials);
                        break;
                    case "3":
                        UICommon.MessageWarning("Test Application not implemented yet.");
                        UICommon.PromptToContinue();
                        break;
                    case "4":
                    case "q":
                    case "Q":
                        exit = true;
                        break;
                    default:
                        UICommon.MessageWarning("-> Invalid option. Please try again.");
                        UICommon.PromptToContinue();
                        break;
                }
            }
        }

        private static void CreateAndRunProject(IReadOnlyList<TableLug> lugs, IReadOnlyList<Material> materials)
        {
            UICommon.NewProjectHeader();

            var project = ReadProjectHeader();

            double nominalWeightKg;
            double wcf;

            ApplyWeightAndWcf(project, out nominalWeightKg, out wcf);
            ReadLiftGeometry(project);
            ChooseCalculationTypeAndRun(project, lugs, materials);

            if (UICommon.PromptSaveProject())
            {
                SaveProject(project);
            }
        }

        private static void OpenAndRunProject(IReadOnlyList<TableLug> lugs, IReadOnlyList<Material> materials)
        {
            var projectNames = FilingSystem.GetAllProjectNames();

            if (projectNames.Count == 0)
            {
                UICommon.MessageWarning("-> No saved projects found.");
                UICommon.PromptToContinue();
                return;
            }

            Console.WriteLine("> Available projects:");
            for (int i = 0; i < projectNames.Count; i++)
                Console.WriteLine($"{i + 1}. {projectNames[i]}");

            int choice = UICommon.PromptInt("> Select project number:");

            if (choice < 1 || choice > projectNames.Count)
            {
                UICommon.MessageWarning("-> Invalid selection.");
                UICommon.PromptToContinue();
                return;
            }

            string selectedName = projectNames[choice - 1];
            string dataDir = FilingSystem.GetProjectSubDirectory(selectedName, "Data");
            string projectFile = Path.Combine(dataDir, "project.txt");

            string content = FilingSystem.LoadTextFile(projectFile);
            if (string.IsNullOrWhiteSpace(content))
            {
                UICommon.MessageError("-> Project file is empty or missing.");
                return;
            }

            var project = ProjectFormatter.FromText(content);

            if (project.UserLugID.HasValue || project.UserMaterialID.HasValue)
            {
                Console.WriteLine();
                Console.WriteLine("> Stored selection in project file:");

                if (project.UserMaterialID.HasValue)
                {
                    var mat = materials.FirstOrDefault(m => m.MaterialID == project.UserMaterialID.Value);
                    if (mat is not null) Console.WriteLine($" MaterialID : {mat.MaterialID}, {mat.Designation}");
                }

                if (project.UserLugID.HasValue)
                {
                    var lug = lugs.FirstOrDefault(l => l.LugID == project.UserLugID.Value);
                    if (lug is not null) Console.WriteLine($" LugID : {lug.LugID}, Type : {lug.LugType}, WLL : {lug.LugWLL} [kg]");
                }

                Console.WriteLine();
            }

            ChooseCalculationTypeAndRun(project, lugs, materials);
        }



        private static ProjectInput ReadProjectHeader()
        {
            var project = new ProjectInput
            {
                ProjectID = 0
            };

            project.Name = UICommon.PromptRequired("> Project name:");
            project.CreatedBy = UICommon.PromptOptional("> Created by:");
            project.Date = DateTime.Today.ToString("> Date (dd-MM-yyyy):");
            project.Revision = UICommon.PromptOptional("> Revision:");

            UICommon.DrawSeparator();
            return project;
        }

        private static void ApplyWeightAndWcf(ProjectInput project, out double nominalWeightKg, out double wcf)
        {
            while (true)
            {
                UICommon.PromptForWeight();
                string basisChoice = (Console.ReadLine() ?? "").Trim();

                if (basisChoice == "1")
                {
                    nominalWeightKg = UICommon.PromptDouble("> Actual object weight [kg]:");
                    break;
                }
                if (basisChoice == "2")
                {
                    nominalWeightKg = UICommon.PromptDouble("> Working Load Limit (WLL) [kg]:");
                    break;
                }

                UICommon.MessageWarning("-> Invalid choice. Please enter 1 or 2.");
            }

            while (true)
            {
                UICommon.WeightCalculationFactor();
                string wcfChoice = Console.ReadLine() ?? "";

                wcf = PreliminaryCalculations.ChoosingWCF(wcfChoice);

                // ChoosingWCF already maps unknown input to WCF_DEFAULT,
                // but better confirm with the user:
                if (wcfChoice is "1" or "2" or "3" or "4") break;

                UICommon.MessageWarning("-> Invalid choice. Using default WCF. Press Enter to accept or any key to choose again.");
                var key = Console.ReadKey(intercept: true);
                Console.WriteLine();

                if (key.Key == ConsoleKey.Enter) break;
            }

            double designWll = nominalWeightKg * wcf;
            project.WLL = designWll;
        }

        private static void ReadLiftGeometry(ProjectInput project)
        {
            while (true)
            {
                UICommon.NumberPointsHeader();
                project.NumberPoints = UICommon.PromptInt("> Number of lifting points (1–4):");

                if (project.NumberPoints is >= 1 and <= 4)
                    break;

                UICommon.MessageWarning("-> Number of points must be between 1 and 4.");
            }

            UICommon.LiftGeometryHeader();

            switch (project.NumberPoints)
            {
                case 1:
                    Console.WriteLine("> Single point lift - no geometry adjustment needed.");
                    project.A1 = 0.0;
                    project.A2 = 0.0;
                    project.B1 = 0.0;
                    project.B2 = 0.0;
                    break;

                case 2:
                    UICommon.TwoPointLift();
                    project.A1 = UICommon.PromptDouble("> Input value of A1 [m]:");
                    project.A2 = UICommon.PromptDouble("> Input value of A2 [m]:");
                    break;

                case 3:
                    UICommon.ThreePointLift();
                    project.A1 = UICommon.PromptDouble("> Input value of A1 [m]:");
                    project.A2 = UICommon.PromptDouble("> Input value of A2 [m]:");
                    project.B1 = UICommon.PromptDouble("> Input value of B1 [m]:");
                    break;

                case 4:
                    UICommon.FourPointLift();
                    project.A1 = UICommon.PromptDouble("> Input value of A1 [m]:");
                    project.A2 = UICommon.PromptDouble("> Input value of A2 [m]:");
                    project.B1 = UICommon.PromptDouble("> Input value of B1 [m]:");
                    project.B2 = UICommon.PromptDouble("> Input value of B2 [m]:");
                    break;
            }
        }

        private static void ChooseCalculationTypeAndRun(ProjectInput project, IReadOnlyList<TableLug> lugs, IReadOnlyList<Material> materials)
        {
            UICommon.PromptTypeCalculation();
            Console.Write("\n> Choice: ");
            var calcChoice = (Console.ReadLine() ?? "").Trim();

            if (calcChoice == "1") RunForwardCalculation(project, lugs, materials);
            else if (calcChoice == "2") RunReverseCalculation(project, lugs, materials);
            else
            {
                UICommon.MessageWarning("-> Invalid calculation type.");
                UICommon.PromptToContinue();
            }
        }

        private static Material SelectMaterial(IReadOnlyList<Material> materials)
        {
            UICommon.MaterialsHeader();
            foreach (var m in materials)
                Console.WriteLine($"{m.MaterialID,3} | {m.Designation,-12} | {m.YieldStrength,8:N0} | {m.TensileStrength,8:N0}");

            int matId = UICommon.PromptInt("> Select Material ID:");
            return materials.First(m => m.MaterialID == matId);
        }

        private static TableLug SelectLug(IReadOnlyList<TableLug> lugs)
        {
            UICommon.LugTableHeader();
            foreach (var l in lugs)
                Console.WriteLine($"{l.LugID,3} | {l.LugType,4} | {l.LugWLL,10:N1}");

            int lugId = UICommon.PromptInt("> Select Lug ID:");
            return lugs.First(l => l.LugID == lugId);
        }



        private static void RunForwardCalculation(ProjectInput project, IReadOnlyList<TableLug> lugs, IReadOnlyList<Material> materials)
        {
            var material = SelectMaterial(materials);
            var lug = SelectLug(lugs);

            project.UserLugID = lug.LugID;
            project.UserMaterialID = material.MaterialID;

            var input = new ForwardInput(project, lug, material);
            var result = ForwardCalculator.Run(input);

            ShowForwardResult(project, lug, material, result);

            if (UICommon.PromptSaveProject()) SaveProject(project);

            if (UICommon.PromptYesNo("> Do you want to save a calculation report?")) SaveForwardReport(project, lug, material, result);

        }

        private static void ShowForwardResult(ProjectInput project, TableLug lug, Material material, CalculationResult result)
        {
            UICommon.ResultsHeader();

            Console.WriteLine($"> Project  : {project.Name}");
            Console.WriteLine($"> WLL      : {project.WLL:N1} kg (design)");
            Console.WriteLine($"> Lug      : ID {lug.LugID}, Type {lug.LugType}, WLL = {lug.LugWLL} kg");
            Console.WriteLine($"> Material : {material.Designation} (fy = {material.YieldStrength} MPa)");
            Console.WriteLine($"> Applied  : {result.AppliedLoad:N1} kN");
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
        }



        private static void RunReverseCalculation(ProjectInput project, IReadOnlyList<TableLug> lugs, IReadOnlyList<Material> materials)
        {
            var material = SelectMaterial(materials);
            var input = new ReverseInput(project, material, lugs);
            var selection = ReverseCalculator.Run(input);

            if (selection.Best is not null)
            {
                project.UserLugID = selection.Best.Lug.LugID;
                project.UserMaterialID = material.MaterialID;
            }

            ShowReverseResult(project, material, selection);

            if (UICommon.PromptSaveProject()) SaveProject(project);
            UICommon.PromptToContinue();
        }

        private static void ShowReverseResult(ProjectInput project, Material material, ReverseSelection selection)
        {
            UICommon.ResultsHeader();

            Console.WriteLine($"> Project : {project.Name}");
            Console.WriteLine($"> WLL     : {project.WLL:N1} kg (design)");
            Console.WriteLine($"> Material: {material.Designation} (fy = {material.YieldStrength} MPa)");
            Console.WriteLine($"> Applied : {selection.AppliedLoad:N1} kN");
            Console.WriteLine();

            if (selection.Best is null)
            {
                UICommon.MessageError("-> No suitable lug found in table for this load/material.");
                return;
            }

            var best = selection.Best;
            Console.WriteLine($"> Suggested Lug ID: {best.Lug.LugID}, Type {best.Lug.LugType}, WLL = {best.Lug.LugWLL} kg");
            Console.WriteLine($"> Minimum FS      : {best.Result.MinimumFS:N1}");
            Console.WriteLine($"> Result          : {(best.Result.Pass ? "PASS" : "FAIL")}");
            Console.WriteLine();

            if (best.Result.Pass) UICommon.OverallResultPass();
            else UICommon.OverallResultFail();
        }



        private static void SaveProject(ProjectInput project)
        {
            FilingSystem.EnsureProjectDirectoriesExist(project.Name);
            string dataDir = FilingSystem.GetProjectSubDirectory(project.Name, "Data");
            string projectFile = Path.Combine(dataDir, "project.txt");

            string text = ProjectFormatter.ToText(project);
            FilingSystem.SaveTextFile(projectFile, text);

            UICommon.MessageSuccess($"Project saved to: {projectFile}");
        }

        private static void SaveForwardReport(ProjectInput project, TableLug lug, Material material, CalculationResult result)
        {
            FilingSystem.EnsureProjectDirectoriesExist(project.Name);
            string resultsDir = FilingSystem.GetProjectSubDirectory(project.Name, "Results");

            string timestamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            string reportFile = Path.Combine(resultsDir, $"ForwardReport_{timestamp}.txt");

            string reportText = ReportFormatter.ForwardReport(project, lug, material, result);
            FilingSystem.SaveTextFile(reportFile, reportText);

            UICommon.MessageSuccess($"-> Report saved to: {reportFile}");
        }


    }
}
