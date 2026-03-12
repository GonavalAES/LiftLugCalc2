namespace LiftLugCalc2.Core.Models;

public static class AppState
{
    // Data loaded from Core
    public static List<TableLug> Lugs = new();
    public static List<Material> Materials { get; set; } = new();
    public static string[] LugNames = Array.Empty<string>();
    public static string[] MaterialNames = Array.Empty<string>();

    public static string ProjectName = "New Project";
    public static double NominalWeightKg = 1000.0;
    public static double Wcf = 1.0; // Weight Contingency Facto

    // UI Selections
    public static int SelectedLugIndex = -1;
    public static int SelectedMaterialIndex = -1;
    public static CalculationResult? LatestResult;
}
