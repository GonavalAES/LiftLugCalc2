using LiftLugCalc2.Core.FileOperations;

namespace LiftLugCalc2.ConsoleFrontEnd
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

            if (lugList.Count == 0 || materialList.Count == 0)
            {
                UICommon.MessageError($"> Ref data failed. Lugs: {lugList.Count}, Mats: {materialList.Count}");
                UICommon.PromptToContinue();
                return;
            }

            var consoleUI = new ConsoleUI(lugList, materialList);
            consoleUI.Run();

            UICommon.ExitApplication();
        }
    }
}
