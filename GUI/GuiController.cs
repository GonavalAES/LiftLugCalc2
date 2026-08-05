using LiftLugCalc2.GUI.Windows;

namespace LiftLugCalc2.GUI;

public sealed class GuiController
{
    private readonly Session currentSession = new();

    public void Render()
    {
        switch (currentSession.CurrentScreen)
        {
            case Screen.MainMenu:
                MainMenuWindow.Render(currentSession);
                break;
            case Screen.NewProject:
                ProjectSetupWindow.Render(currentSession);
                break;
            case Screen.OpenProject:
                OpenProjectWindow.Render(currentSession);
                break;
            case Screen.WeightDefinition:
                WeightWindow.Render(currentSession);
                break;
            case Screen.LiftGeometry:
                LiftGeometryWindow.Render(currentSession);
                break;
            case Screen.MaterialSelection:
                MaterialWindow.Render(currentSession);
                break;
            case Screen.LugGeometry:
                LugGeometryWindow.Render(currentSession);
                break;
            case Screen.ForwardCalculation:
                ForwardCalculationWindow.Render(currentSession);
                break;
            case Screen.ReverseCalculation:
                ReverseCalculationWindow.Render(currentSession);
                break;
            case Screen.Results:
                ResultsWindow.Render(currentSession);
                break;
        }
    }
}
