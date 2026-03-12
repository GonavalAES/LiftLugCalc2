using LiftLugCalc2.Core.FileOperations;
using LiftLugCalc2.Core.Models;

using Veldrid;
using Veldrid.Sdl2;
using Veldrid.StartupUtilities;

namespace LiftLugCalc2
{
    public static class Program
    {
        /*
         * MAIN METHOD FOR CONSOLE UI (OLD)
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
        */

        private static Sdl2Window? sdl2Window;
        private static GraphicsDevice? graphicsDevice;
        private static CommandList? commandList;
        private static ImGuiRenderer? renderer;

        static void Main(string[] args)
        {
            // 1. Setup Window & Graphics (KISS approach)
            VeldridStartup.CreateWindowAndGraphicsDevice(
                new WindowCreateInfo(100, 100, 1280, 720, // Need to get the sizes to a Constants class. Or get it into a CSV.
                                     WindowState.Normal,
                                     "LiftLugCalc2 - Engineering Dashboard"),
                out sdl2Window, out graphicsDevice);

            commandList = graphicsDevice.ResourceFactory.CreateCommandList();
            renderer = new ImGuiRenderer(graphicsDevice,
                                         graphicsDevice.MainSwapchain.Framebuffer.OutputDescription,
                                         sdl2Window.Width,
                                         sdl2Window.Height);

            // 2. Load Reference Data
            AppState.Lugs = TableLugLoader.LoadFromCsv();
            AppState.Materials = MaterialLoader.LoadFromCsv();
            AppState.LugNames = AppState.Lugs.Select(l => $"ID {l.LugID}: {l.LugType}").ToArray();
            AppState.MaterialNames = AppState.Materials.Select(m => m.Designation).ToArray();

            // 3. The Main Loop
            while (sdl2Window.Exists)
            {
                InputSnapshot snapshot = sdl2Window.PumpEvents();
                if (!sdl2Window.Exists) break;

                // Feed input to ImGui
                renderer.Update(1f / 60f, snapshot);

                // Define the UI
                // Render the various windows/forms
                // This is in a class of its own in GUI folder, Program stays lean and clean!!!!!

                // Render
                commandList.Begin();
                commandList.SetFramebuffer(graphicsDevice.MainSwapchain.Framebuffer);
                commandList.ClearColorTarget(0, new RgbaFloat(0.1f, 0.1f, 0.13f, 1f)); // Dark gray background
                renderer.Render(graphicsDevice, commandList);
                commandList.End();

                graphicsDevice.SubmitCommands(commandList);
                graphicsDevice.SwapBuffers(graphicsDevice.MainSwapchain);
            }

            // Cleanup
            renderer.Dispose();
            commandList.Dispose();
            graphicsDevice.Dispose();
        }


    }
}
