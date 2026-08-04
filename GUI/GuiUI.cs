using LiftLugCalc2.GUI.Windows;

namespace LiftLugCalc2.GUI;

public sealed class GuiUI
{
    public void Render()
    {
        // Renders all active windows/panels for the current frame
        InputWindow.Render();
        ResultsWindow.Render();
    }
}
