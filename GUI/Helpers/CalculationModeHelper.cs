namespace LiftLugCalc2.GUI.Helpers;

public static class CalculationModeHelper
{
    public static string StringChoice(CalculationMode mode)
    {
        return mode switch
        {
            CalculationMode.Forward => "1",
            CalculationMode.Reverse => "2",
            _ => throw new InvalidOperationException()
        };
    }
}
