using LiftLugCalc2.Core.Models;
using LiftLugCalc2.Core.Utilities;

namespace LiftLugCalc2.ConsoleFrontEnd;

public static class UICommon
{
    public static void UISplash()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════╗");
        Console.WriteLine("║               LIFT LUG CALCULATOR                  ║");
        Console.WriteLine("╠════════════════════════════════════════════════════╣");
        Console.WriteLine("║       Engineering lifting lug calculations         ║");
        Console.WriteLine("║           Version 2 - Raul Leite - 2025            ║");
        Console.WriteLine("╚════════════════════════════════════════════════════╝");
        Console.WriteLine();
    }
    public static void UIMainMenu()
    {
        Console.WriteLine("╔══════════════════════╗");
        Console.WriteLine("║      MAIN MENU       ║");
        Console.WriteLine("╠══════════════════════╣");
        Console.WriteLine("║ 1. Create Project    ║");
        Console.WriteLine("║ 2. Open Project      ║");
        Console.WriteLine("║ 3. TEST APPLICATION  ║");
        Console.WriteLine("║ 4. Exit              ║");
        Console.WriteLine("╚══════════════════════╝");
        Console.WriteLine();
    }
    public static void NewProjectHeader()
    {
        Console.WriteLine("╔═════════════════════════════════════════════════════════════╗");
        Console.WriteLine("║                     NEW PROJECT SETUP                       ║");
        Console.WriteLine("╚═════════════════════════════════════════════════════════════╝");
        Console.WriteLine();
    }
    public static void DrawSeparator()
    {
        Console.WriteLine("═══════════════════════════════════════════════════════════════");
        Console.WriteLine();
    }
    public static void NumberPointsHeader()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════╗");
        Console.WriteLine("║ Number of lifting points                           ║");
        Console.WriteLine("║ Valid options: 1, 2, 3, or 4                       ║");
        Console.WriteLine("║ (NORSOK recommends 2–3 points where possible)      ║");
        Console.WriteLine("╚════════════════════════════════════════════════════╝");
        Console.WriteLine();
    }
    public static void LiftGeometryHeader()
    {
        Console.WriteLine("╔═════════════════════════════════════════════════════╗");
        Console.WriteLine("║           LIFTING GEOMETRY CONFIGURATION            ║");
        Console.WriteLine("║                                                     ║");
        Console.WriteLine("║ (Indicate distances between CoG and Lifting Points) ║");
        Console.WriteLine("╚═════════════════════════════════════════════════════╝");
        Console.WriteLine();
    }
    public static void TwoPointLift()
    {
        Console.WriteLine("     1 ------- CoG ------- 2");
        Console.WriteLine("          A1          A2");
        Console.WriteLine();
    }
    public static void ThreePointLift()
    {
        Console.WriteLine("     1 ----- CoG ----- 2");
        Console.WriteLine("         A1   |    A2");
        Console.WriteLine("              |");
        Console.WriteLine("            B1|");
        Console.WriteLine("              |");
        Console.WriteLine("              3");
        Console.WriteLine();
    }
    public static void FourPointLift()
    {
        Console.WriteLine("               B1");
        Console.WriteLine("       1 ----[CoG]---- 2");
        Console.WriteLine("       |               |");
        Console.WriteLine("       |               |");
        Console.WriteLine("    A1[CoG]   CoG    [CoG]A2");
        Console.WriteLine("       |               |");
        Console.WriteLine("       |               |");
        Console.WriteLine("       3 ----[CoG]---- 4");
        Console.WriteLine("               B2");
        Console.WriteLine();
    }
    public static void PromptTypeCalculation()
    {
        Console.WriteLine("╔══════════════════════════════════════════════════════╗");
        Console.WriteLine("║            TYPE OF CALCULATION SELECTION             ║");
        Console.WriteLine("╠══════════════════════════════════════════════════════╣");
        Console.WriteLine("║ Please select the type of calculation to perform:    ║");
        Console.WriteLine("║ 1. Forward Calculation (given data, check if passes) ║");
        Console.WriteLine("║ 2. Reverse Calculation (given data, suggest lug)     ║");
        Console.WriteLine("╚══════════════════════════════════════════════════════╝");
        Console.WriteLine();
    }
    public static void MaterialsHeader()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════╗");
        Console.WriteLine("║                    AVAILABLE MATERIALS             ║");
        Console.WriteLine("╚════════════════════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("> ID  |  Designation  |  Yield (MPa)  |  Tensile (MPa)");
        Console.WriteLine("------------------------------------------------------");
    }
    public static void LugTableHeader()
    {
        Console.WriteLine("╔════════════════════════════════════════════════════╗");
        Console.WriteLine("║               AVAILABLE LUG TYPES                  ║");
        Console.WriteLine("╚════════════════════════════════════════════════════╝");
        Console.WriteLine();
        Console.WriteLine("> ID  |  Type  |  WLL (kg)");
        Console.WriteLine("----------------------------------------------------");
    }
    public static void WeightCalculationFactor()
    {

        Console.WriteLine("Select weight determination method:");
        Console.WriteLine($"> 1.: Weighing / measured ±3% →                         WCF = {Constants.WCF_MEASURED}");
        Console.WriteLine($"> 2.: Detailed calc from updated drawings →             WCF = {Constants.WCF_DETAILED_UPDATED}");
        Console.WriteLine($"> 3.: Detailed calc from older/less accurate drawings → WCF = {Constants.WCF_DETAILED_OLDER}");
        Console.WriteLine($"> 4.: Standard-mandated →                               WCF = {Constants.WCF_STANDARD}");
        Console.WriteLine();
        Console.Write("Choice: ... ");
    }
    public static void ResultsHeader()
    {
        Console.WriteLine("╔═════════════════════════════════════════════════════════╗");
        Console.WriteLine("║              CALCULATION RESULTS SUMMARY                ║");
        Console.WriteLine("╚═════════════════════════════════════════════════════════╝");
        Console.WriteLine();
    }
    public static void OverallResultPass()
    {
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║              OVERALL RESULT: PASS                         ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
    }
    public static void OverallResultFail()
    {
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine("╔═══════════════════════════════════════════════════════════╗");
        Console.WriteLine("║              OVERALL RESULT: FAIL                         ║");
        Console.WriteLine("╚═══════════════════════════════════════════════════════════╝");
        Console.ResetColor();
        Console.WriteLine();
    }



    public static void MessageLoadRefData()
    {
        Console.WriteLine("-> Loading reference data files...");
    }
    public static void MessageRefDataSummary(List<string> messages)
    {
        foreach (var msg in messages)
        {
            if (msg.StartsWith("[OK]"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                MessageGood(msg);
                Console.ResetColor();
            }
            else if (msg.StartsWith("[FAILED]"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                MessageGood(msg);
                Console.ResetColor();
            }
        }

        Console.WriteLine();

        if (!messages.Any(m => m.StartsWith("[FAILED]")))
        {
            string msg = "All reference data loaded successfully.";
            MessageGood(msg);
        }
    }
    public static void MessageLoadProject()
    {
        Console.WriteLine("-> Loading project file...");
    }
    public static void MessageProjectLoadSummary(List<string> messages)
    {
        foreach (var msg in messages)
        {
            if (msg.StartsWith("[OK]"))
            {
                Console.ForegroundColor = ConsoleColor.Green;
                MessageGood(msg);
                Console.ResetColor();
            }
            else if (msg.StartsWith("[FAILED]"))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                MessageGood(msg);
                Console.ResetColor();
            }
        }

        Console.WriteLine();

        if (!messages.Any(m => m.StartsWith("[FAILED]")))
        {
            Console.ForegroundColor = ConsoleColor.Green;
            string msg = "Project loaded successfully.";
            MessageGood(msg);
            Console.ResetColor();
        }
    }



    public static void PromptToContinue()
    {
        Console.WriteLine("> Press any key to continue...");
        Console.ReadKey();
        Console.WriteLine();
    }
    public static int PromptForChoice(string prompt, IList<string> options = null!)
    {
        Console.WriteLine(prompt);
        for (int i = 0; i < options.Count; i++) Console.WriteLine($"{i + 1}. {options[i]}");

        while (true)
        {
            string? input = Console.ReadLine();
            if (int.TryParse(input, out int choice) && choice >= 1 && choice <= options.Count) return choice; // 1-based index

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("> Invalid option. Please try again.");
            Console.ResetColor();
        }
    }
    public static bool PromptYesNo(string prompt)
    {
        while (true)
        {
            Console.Write($"{prompt} (Y/N): ");
            string? input = Console.ReadLine()?.Trim().ToUpper();

            if (input == "Y" || input == "YES") return true;
            if (input == "N" || input == "NO") return false;

            Console.ForegroundColor = ConsoleColor.Yellow;
            MessageWarning("-> Invalid input. Please enter Y or N.");
            Console.ResetColor();
        }
    }
    public static string PromptRequired(string prompt, string? currentValue = null)
    {
        while (true)
        {
            string displayValue = !string.IsNullOrWhiteSpace(currentValue) ? $"[{currentValue}]" : "";

            Console.Write($"{prompt} {displayValue} ");
            string? input = Console.ReadLine();

            if (InputValidator.ValidateRequired(input, prompt, showMessage: false)) return input!.Trim();
            if (!string.IsNullOrWhiteSpace(currentValue)) return currentValue;

            Console.ForegroundColor = ConsoleColor.Yellow;
            Console.WriteLine("[!] This field is required. Please enter a value.");
            Console.ResetColor();
        }
    }
    public static string PromptOptional(string prompt, string? currentValue = null)
    {
        string displayValue = !string.IsNullOrWhiteSpace(currentValue) ? $"[{currentValue}]" : "";

        Console.Write($"{prompt} {displayValue} ");
        string? input = Console.ReadLine();

        return string.IsNullOrWhiteSpace(input) ? currentValue ?? "" : input.Trim();
    }
    public static string PromptDate(string prompt, string defaultDate)
    {
        while (true)
        {
            Console.Write($"{prompt} ");
            string? input = Console.ReadLine();

            if (string.IsNullOrWhiteSpace(input)) return defaultDate;
            if (InputValidator.ValidateDate(input, out DateTime dt, prompt, showMessage: false))
                return dt.ToString("dd-MM-yyyy");

            Console.ForegroundColor = ConsoleColor.Yellow;
            MessageWarning("-> Invalid date format. Please use DD-MM-YYYY.");
            Console.ResetColor();
        }
    }
    public static double PromptDouble(string prompt, double? currentValue = null)
    {
        while (true)
        {
            // If there's a current value, show it in brackets as a hint
            string displayValue = currentValue.HasValue && currentValue.Value != 0 ? $"[{currentValue.Value:N1}]" : "";

            Console.Write($"{prompt} {displayValue} ");
            string? input = Console.ReadLine();

            // If user just presses Enter and there's a current value, keep it
            if (string.IsNullOrWhiteSpace(input) && currentValue.HasValue) return currentValue.Value;
            if (InputValidator.ValidatePositiveDouble(input, out double result, prompt, showMessage: false)) return result;

            Console.ForegroundColor = ConsoleColor.Yellow;
            MessageWarning("-> Please enter a valid non-negative number or leave blank to keep current.");
            Console.ResetColor();
        }
    }
    public static int PromptInt(string prompt, int? currentValue = null)
    {
        while (true)
        {
            // If there's a current value, show it in brackets as a hint
            string displayValue = currentValue.HasValue && currentValue.Value != 0 ? $"[{currentValue.Value}]" : "";

            Console.Write($"{prompt} {displayValue} ");
            string? input = Console.ReadLine();

            // If user just presses Enter and there's a current value, keep it
            if (string.IsNullOrWhiteSpace(input) && currentValue.HasValue) return currentValue.Value;
            if (InputValidator.ValidatePositiveInt(input, out int result, prompt, showMessage: false)) return result;

            Console.ForegroundColor = ConsoleColor.Yellow;
            MessageWarning("-> Please enter a valid non-negative integer or leave blank to keep current.");
            Console.ResetColor();

            Console.WriteLine();
        }
    }
    public static void PromptForWeight()
    {
        Console.WriteLine("Enter lifting basis:");
        Console.WriteLine("1. I know ONLY the object's actual weight");
        Console.WriteLine("2. I know the object's Working Load Limit (WLL)");
        Console.WriteLine();
        Console.Write("> ... ");
    }
    public static bool PromptSaveProject()
    {
        DrawSeparator();
        Console.WriteLine("Do you want to save this project?");
        Console.Write("> Enter (Y)es or (N)o: ");

        string? input = Console.ReadLine()?.Trim().ToUpper();
        Console.WriteLine();

        return input == "Y" || input == "YES";
    }



    public static void MessageGood(string message)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"[OK] {message}");
        Console.ResetColor();
    }
    public static void MessageWarning(string message)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Yellow;
        Console.WriteLine($"[WARNING] {message}");
        Console.ResetColor();
    }
    public static void MessageError(string message)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"[ERROR] {message}");
        Console.ResetColor();
        MessagePause();
    }
    public static void MessagePause(string message = "> Press any key to continue...")
    {
        Console.WriteLine();
        Console.WriteLine(message);
        Console.ReadKey();
    }
    public static void MessageSuccess(string message)
    {
        Console.WriteLine();
        Console.ForegroundColor = ConsoleColor.Cyan;
        Console.WriteLine($"[SUCCESS] {message}");
        Console.ResetColor();
    }



    public static void ExitApplication()
    {
        Console.Clear();
        Console.WriteLine("Exiting application...");
    }

}
