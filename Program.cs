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

        private static IWindow _window = null!;
        private static GL _gl = null!;
        private static IInputContext _inputContext = null!;
        private static ImGuiController _imGuiController = null!;
        private static GuiUI _gui = null!;

        public static void Main(string[] args)
        {
            // 1. Configure Window Options
            var options = WindowOptions.Default;
            options.Size = new Silk.NET.Maths.Vector2D<int>(1280, 720);
            options.Title = "LiftLugCalc2 - Engineering Dashboard";
            options.API = new GraphicsAPI(ContextAPI.OpenGL, ContextProfile.Core, ContextFlags.Default, new APIVersion(3, 3));

            _window = Window.Create(options);

            // 2. Attach Window Lifecycle Events
            _window.Load += OnLoad;
            _window.Update += OnUpdate;
            _window.Render += OnRender;
            _window.FramebufferResize += OnFramebufferResize;
            _window.Closing += OnClose;

            // 3. Start Application Loop
            _window.Run();
        }

        private static void OnLoad()
        {
            // Initialize OpenGL and Input Contexts
            _gl = _window.CreateOpenGL();
            _inputContext = _window.CreateInput();

            // Initialize Native Silk.NET ImGui Controller
            _imGuiController = new ImGuiController(
                _gl,
                _window,
                _inputContext
            );

            // Load Engineering Reference CSVs
            AppState.Lugs = TableLugLoader.LoadFromCsv();
            AppState.Materials = MaterialLoader.LoadFromCsv();
            AppState.LugNames = AppState.Lugs.Select(l => $"ID {l.LugID}: Type {l.LugType} ({l.LugWLL}kg)").ToArray();
            AppState.MaterialNames = AppState.Materials.Select(m => m.Designation).ToArray();

            // Initialize UI Presenter
            _gui = new GuiUI();
        }

        private static void OnUpdate(double delta)
        {
            // Update ImGui inputs per frame
            _imGuiController.Update((float)delta);
        }

        private static void OnRender(double delta)
        {
            // Clear background with dark engineering palette
            _gl.ClearColor(0.1f, 0.1f, 0.13f, 1.0f);
            _gl.Clear(ClearBufferMask.ColorBufferBit);

            // Render Application UI
            _gui.Render();

            // Submit ImGui commands to OpenGL
            _imGuiController.Render();
        }

        private static void OnFramebufferResize(Silk.NET.Maths.Vector2D<int> newSize)
        {
            _gl.Viewport(newSize);
        }

        private static void OnClose()
        {
            _imGuiController?.Dispose();
            _inputContext?.Dispose();
            _gl?.Dispose();
        }
    }
}
