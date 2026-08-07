using LiftLugCalc2.Core.Engine;
using LiftLugCalc2.Core.Models;
using LiftLugCalc2.GUI.Windows;
using LiftLugCalc2.GUI.Windows.Newones;

namespace LiftLugCalc2.GUI;

public sealed class GuiController
{
    //------------------------------------------------------------
    // Current engineering session
    //------------------------------------------------------------

    public Session CurrentSession { get; }

    //------------------------------------------------------------
    // Constructor
    //------------------------------------------------------------

    public GuiController(Session session)
    {
        CurrentSession = session;
    }

    public Screen CurrentScreen
    {
        get => CurrentSession.CurrentScreen;
        set => CurrentSession.CurrentScreen = value;
    }

    //------------------------------------------------------------
    // Render application
    //------------------------------------------------------------

    public void Render()
    {
        MainWindow.Render(this);
    }

    public void RenderCurrentPage()
    {
        switch (CurrentSession.CurrentScreen)
        {
            case Screen.MainMenu:
                MainMenuWindow.Render(this);
                break;

            case Screen.NewProject:
                ProjectSetupWindow.Render(this);
                break;

            case Screen.OpenProject:
                OpenProjectWindow.Render(this);
                break;

            case Screen.WeightDefinition:
                WeightDefinitionWindow.Render(this);
                break;

            case Screen.LiftGeometry:
                LiftGeometryWindow.Render(this);
                break;

            case Screen.CalculationMode:
                CalculationModeWindow.Render(this);
                break;

            case Screen.MaterialSelection:
                MaterialSelectionWindow.Render(this);
                break;

            case Screen.LugGeometry:
                LugSelectionWindow.Render(this);
                break;

            case Screen.ForwardCalculation:
                ForwardCalculationWindow.Render(this);
                break;

            case Screen.ReverseCalculation:
                ReverseCalculationWindow.Render(this);
                break;

            case Screen.Results:
                ResultsWindow.Render(this);
                break;
        }
    }

    //------------------------------------------------------------
    // Workflow
    //------------------------------------------------------------

    public void CreateProject()
    {
        CurrentSession.CurrentProject = new Project
        {
            ProjectID = 0,

            Name = CurrentSession.ProjectName,

            CreatedBy = CurrentSession.CreatedBy,

            Revision = CurrentSession.Revision,

            Date = DateTime.Today.ToString(Constants.DATE_FORMAT)
        };

        CurrentSession.CurrentScreen = Screen.WeightDefinition;
    }

    public void AcceptWeightDefinition()
    {
        double wcf =
    PreliminaryCalculations.ChoosingWCF(
        CurrentSession.WcfSelection);

        CurrentSession.CurrentProject!.WLL =
            CurrentSession.NominalWeightKg * wcf;

        CurrentSession.CurrentScreen =
            Screen.LiftGeometry;
    }

    public void AcceptLiftGeometry()
    {
        Project project = CurrentSession.CurrentProject!;

        project.NumberPoints = CurrentSession.NumberPoints;

        project.A1 = CurrentSession.A1;

        project.A2 = CurrentSession.A2;

        project.B1 = CurrentSession.B1;

        project.B2 = CurrentSession.B2;

        CurrentScreen = Screen.CalculationMode;
    }

    public void AcceptCalculationMode()
    {
        if (CurrentSession.CalculationMode == CalculationMode.Forward)
        {
            CurrentScreen = Screen.MaterialSelection;
        }
        else
        {
            CurrentScreen = Screen.MaterialSelection;
        }
    }

    public void AcceptMaterialSelection()
    {
        CurrentSession.CurrentProject!.SelectedMaterial =
            CurrentSession.SelectedMaterial;

        if (CurrentSession.CalculationMode == CalculationMode.Forward)
        {
            CurrentScreen = Screen.LugGeometry;
        }
        else
        {
            CurrentScreen = Screen.ReverseCalculation;
        }
    }

    public void AcceptLugSelection()
    {
        CurrentSession.CurrentProject!.SelectedLug =
            CurrentSession.SelectedLug;

        CurrentScreen = Screen.ForwardCalculation;
    }
}
