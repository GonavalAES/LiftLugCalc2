using LiftLugCalc2.Core.Engine;
using LiftLugCalc2.Core.FileOperations;
using LiftLugCalc2.Core.Models;
using LiftLugCalc2.Core.Utilities;
using LiftLugCalc2.GUI.Enums;
using LiftLugCalc2.GUI.Windows;
using LiftLugCalc2.GUI.Windows.Calculations;
using LiftLugCalc2.GUI.Windows.Navigation;
using LiftLugCalc2.GUI.Windows.ProjectSetups;
using LiftLugCalc2.GUI.Windows.Selections;

namespace LiftLugCalc2.GUI;

public sealed class GuiController
{
    // Current engineering session
    public Session CurrentSession { get; }

    // Constructor
    public GuiController(Session session)
    {
        CurrentSession = session;
    }

    public ScreenEnum CurrentScreen
    {
        get => CurrentSession.CurrentScreen;
        set => CurrentSession.CurrentScreen = value;
    }

    // Render application
    public void Render()
    {
        MainWindow.Render(this);
    }

    public void RenderCurrentPage()
    {
        switch (CurrentSession.CurrentScreen)
        {
            case ScreenEnum.MainMenu:
                MenuWindow.Render(this);
                break;

            case ScreenEnum.NewProject:
                ProjectSetupWindow.Render(this);
                break;
            /*
        case ScreenEnum.OpenProject:
            OpenProjectWindow.Render(this);
            break;
            */
            case ScreenEnum.WeightDefinition:
                WeightDefinitionWindow.Render(this);
                break;

            case ScreenEnum.LiftGeometry:
                LiftGeometryWindow.Render(this);
                break;

            case ScreenEnum.CalculationMode:
                CalculationModeWindow.Render(this);
                break;

            case ScreenEnum.MaterialSelection:
                MaterialSelectionWindow.Render(this);
                break;

            case ScreenEnum.LugGeometry:
                LugSelectionWindow.Render(this);
                break;

            case ScreenEnum.ForwardCalculation:
                ForwardCalculationWindow.Render(this);
                break;

            case ScreenEnum.ReverseCalculation:
                ReverseCalculationWindow.Render(this);
                break;

            case ScreenEnum.Results:
                ResultsWindow.Render(this);
                break;
        }
    }

    // Workflow Methods
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

        CurrentSession.CurrentScreen = ScreenEnum.WeightDefinition;
    }

    public void StartNewProject()
    {
        CurrentSession.ProjectName = string.Empty;
        CurrentSession.CreatedBy = string.Empty;
        CurrentSession.Revision = string.Empty;
        CurrentSession.CurrentProject = null;

        CurrentSession.NominalWeightKg = 0.0;
        CurrentSession.WcfSelection = 0;

        CurrentSession.NumberPoints = 0;
        CurrentSession.A1 = 0.0;
        CurrentSession.A2 = 0.0;
        CurrentSession.B1 = 0.0;
        CurrentSession.B2 = 0.0;

        CurrentSession.CalculationMode = CalculationMode.None;
        CurrentSession.SelectedMaterial = null;
        CurrentSession.SelectedLug = null;

        CurrentSession.CurrentResult = null;

        CurrentScreen = ScreenEnum.NewProject;

        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage = "Enter the new project information.";
    }

    public void OpenProject(string projectName)
    {
        if (string.IsNullOrWhiteSpace(projectName))
        {
            CurrentSession.StatusType = StatusType.Warning;
            CurrentSession.StatusMessage = "No project was selected.";
            return;
        }

        string projectDir = FilingSystem.GetProjectDirectory(projectName);
        string projectFile = Path.Combine(projectDir, "project.txt");

        if (!File.Exists(projectFile))
        {
            CurrentSession.StatusType = StatusType.Error;
            CurrentSession.StatusMessage = "Project file was not found.";
            return;
        }

        string text = File.ReadAllText(projectFile);

        Project project = Formatter.ProjectFormatter.FromText(text);

        project.SelectedLug = null;
        project.SelectedMaterial = null;

        if (project.UserLugID.HasValue)
        {
            project.SelectedLug =
                AppState.Lugs.FirstOrDefault(
                    lug => lug.LugID == project.UserLugID.Value);
        }

        if (project.UserMaterialID.HasValue)
        {
            project.SelectedMaterial =
                AppState.Materials.FirstOrDefault(
                    material => material.MaterialID == project.UserMaterialID.Value);
        }

        CurrentSession.CurrentProject = project;

        CurrentSession.ProjectName = project.Name;
        CurrentSession.CreatedBy = project.CreatedBy;
        CurrentSession.Revision = project.Revision;

        CurrentSession.NumberPoints = project.NumberPoints;
        CurrentSession.A1 = project.A1;
        CurrentSession.A2 = project.A2;
        CurrentSession.B1 = project.B1;
        CurrentSession.B2 = project.B2;

        CurrentSession.SelectedLug = project.SelectedLug;
        CurrentSession.SelectedMaterial = project.SelectedMaterial;

        CurrentSession.CurrentResult = null;

        CurrentSession.CurrentScreen = ScreenEnum.NewProject;

        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage =
            $"Project loaded: {project.Name}";
    }

    public void SaveProject()
    {
        Project? project = CurrentSession.CurrentProject;

        if (project is null)
        {
            if (string.IsNullOrWhiteSpace(CurrentSession.ProjectName))
            {
                CurrentSession.StatusType = StatusType.Warning;
                CurrentSession.StatusMessage = "Project name is required before saving.";
                return;
            }

            project = new Project
            {
                ProjectID = 0,
                Name = CurrentSession.ProjectName,
                CreatedBy = CurrentSession.CreatedBy,
                Revision = CurrentSession.Revision,
                Date = DateTime.Today.ToString(Constants.DATE_FORMAT)
            };

            CurrentSession.CurrentProject = project;
        }

        if (project.SelectedLug is not null) project.UserLugID = project.SelectedLug.LugID;
        if (project.SelectedMaterial is not null) project.UserMaterialID = project.SelectedMaterial.MaterialID;

        string projectDir = FilingSystem.GetProjectDirectory(project.Name);
        string projectFile = Path.Combine(projectDir, "project.txt");
        string text = Formatter.ProjectFormatter.ToText(project);

        FilingSystem.SaveTextFile(projectFile, text);

        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage = "Project saved successfully.";
    }

    public void CancelNewProject()
    {
        CurrentSession.ProjectName = string.Empty;
        CurrentSession.CreatedBy = string.Empty;
        CurrentSession.Revision = string.Empty;

        CurrentSession.CurrentProject = null;
        CurrentSession.CurrentScreen = ScreenEnum.MainMenu;

        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage = "New project cancelled.";
    }

    public void AcceptWeightDefinition()
    {
        double wcf = PreliminaryCalculations.ChoosingWCF(CurrentSession.WcfSelection);
        CurrentSession.CurrentProject!.WLL = CurrentSession.NominalWeightKg * wcf;

        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage = "Weight definition accepted. Define lift geometry.";

        CurrentSession.CurrentScreen = ScreenEnum.LiftGeometry;
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

        CurrentScreen = ScreenEnum.CalculationMode;
    }

    public void AcceptCalculationMode()
    {
        if (CurrentSession.CalculationMode == CalculationMode.Forward) CurrentScreen = ScreenEnum.MaterialSelection;
        else CurrentScreen = ScreenEnum.MaterialSelection;
    }

    public void AcceptMaterialSelection()
    {
        CurrentSession.CurrentProject!.SelectedMaterial = CurrentSession.SelectedMaterial;

        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage = "Material selected. Choose calculation mode";

        if (CurrentSession.CalculationMode == CalculationMode.Forward) CurrentScreen = ScreenEnum.LugGeometry;
        else CurrentScreen = ScreenEnum.ReverseCalculation;
    }

    public void AcceptLugSelection()
    {
        CurrentSession.CurrentProject!.SelectedLug = CurrentSession.SelectedLug;

        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage = "Lug selected. Running forward calculation.";

        CurrentScreen = ScreenEnum.ForwardCalculation;
    }

    public void RunForwardCalculation()
    {
        ForwardInput input = new(CurrentSession.CurrentProject!,
                                 CurrentSession.CurrentProject!.SelectedLug!,
                                 CurrentSession.CurrentProject!.SelectedMaterial!);

        CurrentSession.CurrentResult = ForwardCalculator.Run(input, "1");
        CurrentScreen = ScreenEnum.Results;
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
        CurrentScreen = ScreenEnum.Results;
    }

    public void ModifyCalculation()
    {
        CurrentSession.CurrentResult = null;
        CurrentScreen = ScreenEnum.CalculationMode;
        CurrentSession.StatusType = StatusType.Information;
        CurrentSession.StatusMessage = "Modify the calculation data and run again.";
    }

    // Helper Methods
    public bool HasNewProjectData()
        => !string.IsNullOrWhiteSpace(CurrentSession.ProjectName) ||
           !string.IsNullOrWhiteSpace(CurrentSession.CreatedBy) ||
           !string.IsNullOrWhiteSpace(CurrentSession.Revision);

    public void NavigateTo(ScreenEnum screen)
    {
        if (!CanNavigateTo(screen)) return;
        CurrentSession.CurrentScreen = screen;
    }



    public bool HasStepProblem(ScreenEnum screen)
    {
        bool hasProject = CurrentSession.CurrentProject is not null && !string.IsNullOrWhiteSpace(CurrentSession.ProjectName);
        bool hasNoWeight = CurrentSession.NominalWeightKg <= 0.0;
        bool numberPointsGTZero = CurrentSession.NumberPoints > 0;
        bool numberPointsLTTwo = CurrentSession.NumberPoints < 2;

        return screen switch
        {
            ScreenEnum.NewProject => hasProject,
            ScreenEnum.WeightDefinition => hasProject && hasNoWeight,
            ScreenEnum.LiftGeometry => hasProject && numberPointsGTZero && numberPointsLTTwo,
            _ => false
        };
    }

    public bool CanNavigateTo(ScreenEnum screen)
    {
        bool hasProject = CurrentSession.CurrentProject is not null;
        bool hasWeight = hasProject && CurrentSession.CurrentProject!.WLL > 0.0;
        bool hasLiftGeometry = hasWeight && CurrentSession.NumberPoints > 0;
        bool hasCalculationMode = hasLiftGeometry && CurrentSession.CalculationMode != CalculationMode.None;
        bool hasMaterial = hasCalculationMode && CurrentSession.SelectedMaterial is not null;
        bool hasLug = hasMaterial && CurrentSession.SelectedLug is not null;

        return screen switch
        {
            ScreenEnum.NewProject => true,
            ScreenEnum.WeightDefinition => hasProject,
            ScreenEnum.LiftGeometry => hasWeight,
            ScreenEnum.CalculationMode => hasLiftGeometry,
            ScreenEnum.MaterialSelection => hasCalculationMode,
            ScreenEnum.LugGeometry => CurrentSession.CalculationMode == CalculationMode.Forward && hasMaterial,
            ScreenEnum.ForwardCalculation => CurrentSession.CalculationMode == CalculationMode.Forward && hasLug,
            ScreenEnum.ReverseCalculation => CurrentSession.CalculationMode == CalculationMode.Reverse && hasMaterial,
            ScreenEnum.Results => CurrentSession.CurrentResult is not null,
            _ => false
        };
    }

    public bool IsStepComplete(ScreenEnum screen)
    {
        bool hasProject = CurrentSession.CurrentProject is not null;
        bool hasWeight = hasProject && CurrentSession.CurrentProject!.WLL > 0.0;
        bool hasNumberPoints = hasProject && CurrentSession.NumberPoints > 0;
        bool hasCalculationMode = CurrentSession.CalculationMode != CalculationMode.None;
        bool hasMaterial = CurrentSession.SelectedMaterial is not null;
        bool hasLug = CurrentSession.CalculationMode == CalculationMode.Forward && CurrentSession.SelectedLug is not null;
        bool hasResult = CurrentSession.CurrentResult is not null;

        return screen switch
        {
            ScreenEnum.NewProject => hasProject,
            ScreenEnum.WeightDefinition => hasWeight,
            ScreenEnum.LiftGeometry => hasNumberPoints,
            ScreenEnum.CalculationMode => hasCalculationMode,
            ScreenEnum.MaterialSelection => hasMaterial,
            ScreenEnum.LugGeometry => hasLug,
            ScreenEnum.ForwardCalculation => hasResult,
            ScreenEnum.ReverseCalculation => hasResult,
            ScreenEnum.Results => hasResult,
            _ => false
        };
    }

    public string GetLiftGeometryWarning()
    {
        Session session = CurrentSession;
        if (session.NumberPoints < 2) return string.Empty;
        double ratio = AsymmetryRatio(session.A1, session.A2);
        if (ratio > Constants.LIFT_POINT_SYMMETRY_TOLERANCE)
            return $"Warning: A1 and A2 differ by {ratio * 100.0:F1} percent. Verify the lifting arrangement and load distribution.";
        if (session.NumberPoints == 4)
        {
            ratio = AsymmetryRatio(session.B1, session.B2);
            if (ratio > Constants.LIFT_POINT_SYMMETRY_TOLERANCE)
                return $"Warning: B1 and B2 differ by {ratio * 100.0:F1} percent. Verify the lifting arrangement and load distribution.";
        }
        return string.Empty;
    }

    private static double AsymmetryRatio(double first, double second)
    {
        double reference = Math.Max(Math.Abs(first), Math.Abs(second));
        if (reference <= 0.0) return 0.0;
        return Math.Abs(first - second) / reference;
    }
}
