namespace LiftLugCalc2.GUI.Helpers;

public static class LugPresentation
{
    public static string GetLugTypeName(int lugType)
    {
        return lugType switch
        {
            0 => "Type 0",
            1 => "Type 1",
            2 => "Type 2",
            3 => "Type 3",
            _ => $"Unknown ({lugType})"
        };
    }
}
