using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.GUI;

/// <summary>
/// Represents one engineering calculation session. Holds only the transient data required while the user is working on a project.
/// It's the live state of the application, continuously changing every frame.
/// </summary>
public sealed class Session
{
    public Screen CurrentScreen { get; set; } = Screen.MainMenu;
    public Project? CurrentProject { get; set; }
    public Material? SelectedMaterial { get; set; }
    public TableLug? SelectedLug { get; set; }
    public CalculationResult? CurrentResult { get; set; }
    public string StatusMessage { get; set; } = string.Empty;
    public StatusType StatusType { get; set; } = StatusType.Information;

    public int NumberPoints { get; set; }
    public double A1 { get; set; }
    public double A2 { get; set; }
    public double B1 { get; set; }
    public double B2 { get; set; }

    // Temporary fields used while creating a new project.
    public string ProjectName = string.Empty;
    public string CreatedBy = string.Empty;
    public string Revision = string.Empty;

    // Temporary fields used while creating a weight definition
    public int WeightBasis = 0;             // 1 = Actual Weight, 2 = WLL
    public double NominalWeightKg = 0.0;    // Weight in kg, either actual weight or WLL depending on WeightBasis
    public int WcfSelection = 1;            // 1..4

    // Temporary fields used while creating the mode of calculation
    public CalculationMode CalculationMode = CalculationMode.Forward;
}

public enum StatusType
{
    Information,
    Warning,
    Error
}

public enum CalculationMode
{
    Forward,
    Reverse
}
