using LiftLugCalc2.ConsoleFrontEnd;
using LiftLugCalc2.Core.FileOperations;

namespace LiftLugCalc2
{
    public static class Program
    {
        static void Main(string[] args)
        {
            Console.SetWindowSize(Math.Min(120, Console.LargestWindowWidth), 40);

            // Core initialization (UI-agnostic)
            UICommon.UISplash();
            UICommon.MessageLoadRefData();

            var lugList = TableLugLoader.LoadFromCsv();
            var materialList = MaterialLoader.LoadFromCsv();

            if (lugList.Count == 0)
            {
                UICommon.MessageError("No lug types were loaded. Please ensure that the 'LugTypes.csv' file is present and correctly formatted.");
                return;
            }
            else if (materialList.Count == 0)
            {
                UICommon.MessageError("No materials were loaded. Please ensure that the 'Materials.csv' file is present and correctly formatted.");
                return;
            }

            var consoleUI = new ConsoleUI(lugList, materialList);
            consoleUI.Run();

            UICommon.ExitApplication();
        }
    }
}
