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
        //Console.WriteLine("║ 3. TEST APPLICATION  ║");
        Console.WriteLine("║ 3. Exit              ║");
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
        Console.WriteLine("║        (Representation viewed from above)           ║");
        Console.WriteLine("╚═════════════════════════════════════════════════════╝");
        Console.WriteLine();
    }
    public static void TwoPointLift()
    {
        Console.WriteLine("    1 <------> CoG <------> 2");
        Console.WriteLine("         A1           A2");
        Console.WriteLine();
        Console.WriteLine(" A1, A2: Longitudinal distances from COG to 1 and to 2, respectively");
        Console.WriteLine();
    }
    public static void ThreePointLift()
    {
        Console.WriteLine("    1 <------> CoG <------> 2");
        Console.WriteLine("         A1     |     A2");
        Console.WriteLine("                |");
        Console.WriteLine("             B1 |");
        Console.WriteLine("                |");
        Console.WriteLine("                3");
        Console.WriteLine();
        Console.WriteLine(" A1, A2: Longitudinal distances from COG to 1 and to 2, respectively");
        Console.WriteLine(" B1: Transverse distance from COG to 3");
        Console.WriteLine();
    }
    public static void FourPointLift()
    {
        Console.WriteLine("    1 --------- o --------- 2");
        Console.WriteLine("                |            ");
        Console.WriteLine("             B1 |            ");
        Console.WriteLine("                |            ");
        Console.WriteLine("    <-- A1 --> CoG <-- A2 -->");
        Console.WriteLine("                |            ");
        Console.WriteLine("             B2 |            ");
        Console.WriteLine("                |            ");
        Console.WriteLine("    3 --------- o --------- 4");
        Console.WriteLine();
        Console.WriteLine(" A1, A2: Longitudinal distances from COG to 1/3 and to 2/4, respectively");
        Console.WriteLine(" B1, B2: Transverse distances from COG to 1/2 and to 3/4, respectively");
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
        Console.WriteLine();
        Console.WriteLine("Select weight determination method:");
        Console.WriteLine("------------------------------------------------------------------------");
        Console.WriteLine($"> 1 -> Weighing / measured ±3%                           WCF = {Constants.WCF_MEASURED}");
        Console.WriteLine("------------------------------------------------------------------------");
        Console.WriteLine($"> 2 -> Detailed calc from updated drawings               WCF = {Constants.WCF_DETAILED_UPDATED}");
        Console.WriteLine("------------------------------------------------------------------------");
        Console.WriteLine($"> 3 -> Detailed calc from older/less accurate drawings   WCF = {Constants.WCF_DETAILED_OLDER}");
        Console.WriteLine("------------------------------------------------------------------------");
        Console.WriteLine($"> 4 -> Standard-mandated                                 WCF = {Constants.WCF_STANDARD}");
        Console.WriteLine("------------------------------------------------------------------------");
        Console.WriteLine();
        Console.Write("Choice: ... ");
    }
    public static void DrawLugType(int lugType)
    {
        Console.WriteLine();
        switch (lugType)
        {
            case 0:
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║           Type 0 - Direct connection lug               ║");
                Console.WriteLine("║   Rounded lug plate with central hole for hook/chain   ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════╣");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("║       RadiusLug -->   @@@@@@@@                         ║");
                Console.WriteLine("║                     @   @@@    @   <-- ThicknessPlate  ║");
                Console.WriteLine("║                   @@  @     @  @@                      ║");
                Console.WriteLine("║           ^---- @    @       @  @  <-- DiameterHole    ║");
                Console.WriteLine("║ HCentHole |    @    @@@@@@@@@    @                     ║");
                Console.WriteLine("║           v__  @@@@@@@@@@@@@@@@@@@ <-- Connected/Part  ║");
                Console.WriteLine("║                <---------------- >     of Structural   ║");
                Console.WriteLine("║                    LengthLug           Element         ║");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝");
                Console.WriteLine();
                break;
            case 1:
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                Type 1 - Single plate lug               ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════╣");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("║       RadiusLug -->   @@@@@@@@                         ║");
                Console.WriteLine("║                     @          @    <-- ThicknessPlate ║");
                Console.WriteLine("║                   @@    @@@    @@                      ║");
                Console.WriteLine("║           ^---- @     @   @     @  <-- DiameterHole    ║");
                Console.WriteLine("║ HCentHole |    @       @@@       @                     ║");
                Console.WriteLine("║           v__  @@@@@@@@@@@@@@@@@@@                     ║");
                Console.WriteLine("║                <---------------- >                     ║");
                Console.WriteLine("║                    LengthLug                           ║");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝");
                Console.WriteLine();
                break;
            case 2:
                Console.WriteLine("╔════════════════════════════════════════════════════════╗");
                Console.WriteLine("║                Type 2 - Cheek plate lug                ║");
                Console.WriteLine("╠════════════════════════════════════════════════════════╣");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("║       RadiusLug -->   @@@@@@@@                         ║");
                Console.WriteLine("║                     @ *******  @   <-- ThicknessPlate  ║");
                Console.WriteLine("║                   @@ ***@@@*** @@  <-- Cheek Thickness ║");
                Console.WriteLine("║           ^---- @  ***@@  @**** @  <-- DiameterHole    ║");
                Console.WriteLine("║ HCentHole |    @    ***@@@****   @                     ║");
                Console.WriteLine("║           v__  @@@@@@@@@@@@@@@@@@@                     ║");
                Console.WriteLine("║                <---------------- >                     ║");
                Console.WriteLine("║                    LengthLug                           ║");
                Console.WriteLine("║                                                        ║");
                Console.WriteLine("╚════════════════════════════════════════════════════════╝");
                Console.WriteLine();
                break;
            case 3:
                Console.WriteLine("╔═══════════════════════════════════════════════════════╗");
                Console.WriteLine("║                   Type 3 - Boss lug                   ║");
                Console.WriteLine("╠═══════════════════════════════════════════════════════╣");
                Console.WriteLine("║                                                       ║");
                Console.WriteLine("║       RadiusLug -->     @@@@                          ║");
                Console.WriteLine("║                       @@   @@@     <-- ThicknessPlate ║");
                Console.WriteLine("║                   @@**@@   @@**@@  <-- Boss Thickness ║");
                Console.WriteLine("║           ^---- @   **@   @***  @  <-- DiameterHole   ║");
                Console.WriteLine("║  HCentHol |    @     **@@@***    @                    ║");
                Console.WriteLine("║           v__  @@@@@@@@@@@@@@@@@@@                    ║");
                Console.WriteLine("║                <---------------- >                    ║");
                Console.WriteLine("║                    LengthLug                          ║");
                Console.WriteLine("║                                                       ║");
                Console.WriteLine("╚═══════════════════════════════════════════════════════╝");
                Console.WriteLine();
                break;
            default:
                Console.WriteLine("[Unknown Lug Type]");
                break;
        }
        Console.WriteLine();
    }
    public static void FinalChoicesMenu()
    {
        Console.WriteLine("╔════════════════════════════╗");
        Console.WriteLine("║       WHAT IS NEXT?        ║");
        Console.WriteLine("╠════════════════════════════╣");
        Console.WriteLine("║ 1. View detailed report    ║");
        Console.WriteLine("║ 2. Save report file        ║");
        Console.WriteLine("║ 3. Save project            ║");
        Console.WriteLine("║ 4. Perform new calculation ║");
        Console.WriteLine("║ 5. Back to Main Menu       ║");
        Console.WriteLine("╚════════════════════════════╝");
        Console.WriteLine();
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
        string message = "-> Loading reference data files...";
        MessageGood(message);
    }
    public static void MessageLoadProject()
    {
        string message = "-> Loading project file...";
        MessageGood(message);
    }



    public static void PromptToContinue()
    {
        Console.WriteLine("> Press any key to continue...");
        Console.ReadKey();
        Console.WriteLine();
    }
    public static bool PromptYesNo(string message)
    {
        DrawSeparator();
        Console.WriteLine(message);
        Console.Write("> Enter (Y)es or (N)o: ");

        string? input = Console.ReadLine()?.Trim().ToUpper();
        Console.WriteLine();

        if (input == "Y" || input == "YES") return true;
        if (input == "N" || input == "NO") return false;

        Console.ForegroundColor = ConsoleColor.Yellow;
        MessageWarning("Invalid input. Please enter Y or N.");
        Console.ResetColor();

        return PromptYesNo(message);  // Retry
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

            MessageWarning("This field is required. Please enter a value.");
        }
    }
    public static string PromptOptional(string prompt, string? currentValue = null)
    {
        string displayValue = !string.IsNullOrWhiteSpace(currentValue) ? $"[{currentValue}]" : "";

        Console.Write($"{prompt} {displayValue} ");
        string? input = Console.ReadLine();

        return string.IsNullOrWhiteSpace(input) ? currentValue ?? "" : input.Trim();
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

            MessageWarning("-> Please enter a valid non-negative number or leave blank to keep current.");
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

            MessageWarning("-> Please enter a valid non-negative integer or leave blank to keep current.");
            Console.WriteLine();
        }
    }
    public static void PromptForWeight()
    {
        Console.WriteLine("Enter lifting basis:");
        Console.WriteLine("1 -> I know ONLY the object's actual weight");
        Console.WriteLine("2 -> I know the object's Working Load Limit (WLL)");
        Console.WriteLine();
        Console.Write("> ... ");
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
