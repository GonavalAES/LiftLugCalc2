using LiftLugCalc2.GUI.Enums;

namespace LiftLugCalc2.GUI.Helpers;

public static class CalculationModeHelper
{
    public static string StringChoice(CalculationMode mode)
    {
        return mode switch
        {
            CalculationMode.Forward => "1",
            CalculationMode.Reverse => "2",
            CalculationMode.Exploratory => "3",
            _ => throw new InvalidOperationException("A calculation mode must be selected.")
        };
    }
}
