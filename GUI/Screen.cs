namespace LiftLugCalc2.GUI;

public enum Screen
{
    MainMenu,
    NewProject,
    OpenProject,
    WeightDefinition,
    LiftGeometry,
    CalculationMode,
    MaterialSelection,
    LugGeometry,
    ForwardCalculation,
    ReverseCalculation,
    Results
}

public record CommandButton(
    string Text,
    Action OnClick,
    bool Enabled = true);