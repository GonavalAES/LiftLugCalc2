namespace LiftLugCalc2.Core.Core.Tests;

public static class Assert
{
    public static void NearlyEqual(
        double actual,
        double expected,
        double tolerance,
        string message)
    {
        if (Math.Abs(actual - expected) > tolerance) throw new Exception($"{message}: expected {expected:F2}, got {actual:F2}");

        Console.WriteLine($"[OK] {message}");
    }

    public static void Greater(double actual, double minimum, string message)
    {
        if (actual <= minimum) throw new Exception($"{message}: {actual:F2} <= {minimum}");

        Console.WriteLine($"[OK] {message}");
    }

    public static void True(bool condition, string message)
    {
        if (!condition) throw new Exception($"{message}");

        Console.WriteLine($"[OK] {message}");
    }
}
