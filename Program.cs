using LiftLugCalc2.Core.FileOperations;
using LiftLugCalc2.Core.Models;
using LiftLugCalc2.GUI;

using Silk.NET.Input;
using Silk.NET.OpenGL;
using Silk.NET.OpenGL.Extensions.ImGui;
using Silk.NET.Windowing;

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

        private static IWindow window = null!;
        private static GL gl = null!;
        private static IInputContext inputContext = null!;
        private static ImGuiController imGuiController = null!;
        private static GuiController controller = null!;

        public static void Main(string[] args)
        {
            // 1. Configure Window Options
            var options = WindowOptions.Default;
            options.Size = new Silk.NET.Maths.Vector2D<int>(1280, 720);
            options.Title = "LiftLugCalc2 - Engineering Dashboard";
            options.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Default, new APIVersion(3, 3));

            window = Window.Create(options);

            // 2. Attach Window Lifecycle Events
            window.Load += OnLoad;
            window.Update += OnUpdate;
            window.Render += OnRender;
            window.FramebufferResize += OnFramebufferResize;
            window.Closing += OnClose;

            // 3. Start Application Loop
            window.Run();
        }

        private static void OnLoad()
        {
            // Initialize OpenGL and Input Contexts
            gl = window.CreateOpenGL();
            inputContext = window.CreateInput();

            // Initialize Native Silk.NET ImGui Controller
            imGuiController = new ImGuiController(gl, window, inputContext);

            // Load Engineering Reference CSVs
            AppState.Lugs = TableLugLoader.LoadFromCsv();
            AppState.Materials = MaterialLoader.LoadFromCsv();
            AppState.LugNames = AppState.Lugs.Select(l => $"ID {l.LugID} - Type {l.LugType} - {l.LugWLL:N0} kg").ToArray();
            AppState.MaterialNames = AppState.Materials.Select(m => $"{m.Designation}").ToArray();

            // Create application state
            Session currentSession = new();

            // Initialize UI Presenter
            controller = new GuiController(currentSession);
        }

        private static void OnUpdate(double delta) => imGuiController.Update((float)delta); // Update ImGui inputs per frame

        private static void OnRender(double delta)
        {
            // Clear background with dark engineering palette
            gl.ClearColor(0.1f, 0.1f, 0.13f, 1.0f);
            gl.Clear(ClearBufferMask.ColorBufferBit);

            // Render Application UI
            controller.Render();

            // Submit ImGui commands to OpenGL
            imGuiController.Render();
        }

        private static void OnFramebufferResize(Silk.NET.Maths.Vector2D<int> newSize) => gl.Viewport(newSize);

        private static void OnClose()
        {
            imGuiController?.Dispose();
            inputContext?.Dispose();
            gl?.Dispose();
        }
    }
}
