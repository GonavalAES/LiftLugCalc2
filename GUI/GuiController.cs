using LiftLugCalc2.Core.Engine;
using LiftLugCalc2.Core.Models;
using LiftLugCalc2.GUI.Enums;
using LiftLugCalc2.GUI.Windows;
using LiftLugCalc2.GUI.Windows.Calculations;
using LiftLugCalc2.GUI.Windows.Navigation;
using LiftLugCalc2.GUI.Windows.ProjectSetups;
using LiftLugCalc2.GUI.Windows.Selections;

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
                MenuWindow.Render(this);
                break;

            case Screen.NewProject:
                ProjectSetupWindow.Render(this);
                break;
            /*
        case Screen.OpenProject:
            OpenProjectWindow.Render(this);
            break;
            */
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

        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage = "Project created. Define the weight.";

        CurrentSession.CurrentScreen = Screen.WeightDefinition;
    }

    public void CancelNewProject()
    {
        CurrentSession.ProjectName = string.Empty;
        CurrentSession.CreatedBy = string.Empty;
        CurrentSession.Revision = string.Empty;

        CurrentSession.CurrentProject = null;
        CurrentSession.CurrentScreen = Screen.MainMenu;

        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage = "New project cancelled.";
    }

    public void AcceptWeightDefinition()
    {
        double wcf = PreliminaryCalculations.ChoosingWCF(CurrentSession.WcfSelection);
        CurrentSession.CurrentProject!.WLL = CurrentSession.NominalWeightKg * wcf;

        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage = "Weight definition accepted. Define lift geometry.";

        CurrentSession.CurrentScreen = Screen.LiftGeometry;
    }

    public void AcceptLiftGeometry()
    {
        Project project = CurrentSession.CurrentProject!;

        project.NumberPoints = CurrentSession.NumberPoints;
        project.A1 = CurrentSession.A1;
        project.A2 = CurrentSession.A2;
        project.B1 = CurrentSession.B1;
        project.B2 = CurrentSession.B2;

        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage = "Lift geometry defined. Define calculation mode.";

        CurrentScreen = Screen.CalculationMode;
    }

    public void AcceptCalculationMode()
    {
        if (CurrentSession.CalculationMode == CalculationMode.Forward) CurrentScreen = Screen.MaterialSelection;
        else CurrentScreen = Screen.MaterialSelection;
    }

    public void AcceptMaterialSelection()
    {
        CurrentSession.CurrentProject!.SelectedMaterial = CurrentSession.SelectedMaterial;

        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage = "Material selected. Choose calculation mode";

        if (CurrentSession.CalculationMode == CalculationMode.Forward) CurrentScreen = Screen.LugGeometry;
        else CurrentScreen = Screen.ReverseCalculation;
    }

    public void AcceptLugSelection()
    {
        CurrentSession.CurrentProject!.SelectedLug = CurrentSession.SelectedLug;

        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage = "Lug selected. Running forward calculation.";

        CurrentScreen = Screen.ForwardCalculation;
    }

    public void RunForwardCalculation()
    {
        ForwardInput input = new(CurrentSession.CurrentProject!,
                                 CurrentSession.CurrentProject!.SelectedLug!,
                                 CurrentSession.CurrentProject!.SelectedMaterial!);

        CurrentSession.CurrentResult = ForwardCalculator.Run(input, "1");
        CurrentScreen = Screen.Results;
    }

    public void RunReverseCalculation()
    {
        Project project = CurrentSession.CurrentProject!;
        Material material = CurrentSession.SelectedMaterial!;

        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage = "Material selected. Running reverse calculation.";

        ReverseInput input = new(project, material, AppState.Lugs);
        ReverseSelection selection = ReverseCalculator.Run(input, "2");

        if (selection.Best is not null)
        {
            project.SelectedLug = selection.Best.Lug;
            project.SelectedMaterial = material;
        }

        CurrentSession.CurrentResult = selection.Best?.Result;
        CurrentScreen = Screen.Results;
    }


    //------------------------------------------------------------
    // Helper Methods
    //------------------------------------------------------------
    public bool HasNewProjectData()
        => !string.IsNullOrWhiteSpace(CurrentSession.ProjectName) ||
           !string.IsNullOrWhiteSpace(CurrentSession.CreatedBy) ||
           !string.IsNullOrWhiteSpace(CurrentSession.Revision);

    public void NavigateTo(Screen screen)
    {
        if (!CanNavigateTo(screen)) return;
        CurrentSession.CurrentScreen = screen;
    }

    public bool CanNavigateTo(Screen screen)
    {
        bool hasProject = CurrentSession.CurrentProject is not null;
        bool hasWeight = hasProject && CurrentSession.CurrentProject!.WLL > 0.0;
        bool hasLiftGeometry = hasWeight && CurrentSession.NumberPoints >= 2;
        bool hasCalculationMode = hasLiftGeometry && CurrentSession.CalculationMode != CalculationMode.None;
        bool hasMaterial = hasCalculationMode && CurrentSession.SelectedMaterial is not null;
        bool hasLug = hasMaterial && CurrentSession.SelectedLug is not null;

        return screen switch
        {
            Screen.NewProject => true,
            Screen.WeightDefinition => hasProject,
            Screen.LiftGeometry => hasWeight,
            Screen.CalculationMode => hasLiftGeometry,
            Screen.MaterialSelection => hasCalculationMode,
            Screen.LugGeometry => CurrentSession.CalculationMode == CalculationMode.Forward && hasMaterial,
            Screen.ForwardCalculation => CurrentSession.CalculationMode == CalculationMode.Forward && hasLug,
            Screen.ReverseCalculation => CurrentSession.CalculationMode == CalculationMode.Reverse && hasMaterial,
            Screen.Results => CurrentSession.CurrentResult is not null,
            _ => false
        };
    }

    public bool IsStepComplete(Screen screen)
    {


        switch (screen)
        {
            case Screen.NewProject:
                return CurrentSession.CurrentProject is not null;

            case Screen.WeightDefinition:
                return
                    CurrentSession.CurrentProject is not null &&
                    CurrentSession.CurrentProject.WLL > 0.0;

            case Screen.LiftGeometry:
                return
                    CurrentSession.CurrentProject is not null &&
                    CurrentSession.NumberPoints >= 2;

            case Screen.CalculationMode:
                return
                    CurrentSession.CalculationMode != CalculationMode.None;

            case Screen.MaterialSelection:
                return
                    CurrentSession.SelectedMaterial is not null;

            case Screen.LugGeometry:
                return
                    CurrentSession.CalculationMode == CalculationMode.Forward &&
                    CurrentSession.SelectedLug is not null;

            case Screen.ForwardCalculation:
            case Screen.ReverseCalculation:
                return CurrentSession.CurrentResult is not null;

            case Screen.Results:
                return CurrentSession.CurrentResult is not null;

            default:
                return false;
        }
    }

    public bool HasStepProblem(Screen screen)
    {
        bool hasProject = CurrentSession.CurrentProject is not null && !string.IsNullOrWhiteSpace(CurrentSession.ProjectName);
        bool hasNoWeight = CurrentSession.NominalWeightKg <= 0.0;
        bool numberPointsGTZero = CurrentSession.NumberPoints > 0;
        bool numberPointsLTTwo = CurrentSession.NumberPoints < 2;

        return screen switch
        {
            Screen.NewProject => hasProject,
            Screen.WeightDefinition => hasProject && hasNoWeight,
            Screen.LiftGeometry => hasProject && numberPointsGTZero && numberPointsLTTwo,
            _ => false
        };
    }
}
