using LiftLugCalc2.ConsoleFrontEnd;

using System.Globalization;

namespace LiftLugCalc2.Core.Utilities;

public static class InputValidator
{
    private static readonly CultureInfo Culture = new("pt-PT");

    public static bool ValidateRequired(string? input, string fieldName, bool showMessage = true)
    {
        if (!string.IsNullOrWhiteSpace(input)) return true;
        if (showMessage) UICommon.MessageWarning($"Field \"{fieldName}\" is required.");
        return false;
    }

    public static bool ValidatePositiveDouble(string? input, out double value, string fieldName, bool showMessage = true)
    {
        value = 0.0;

        if (string.IsNullOrWhiteSpace(input))
        {
            if (showMessage) UICommon.MessageWarning($"Field \"{fieldName}\" cannot be empty.");
            return false;
        }

        if (!double.TryParse(input, NumberStyles.Float, Culture, out var parsed))
        {
            if (showMessage) UICommon.MessageWarning($"Field \"{fieldName}\" must be a valid number.");
            return false;
        }

        if (parsed < 0)
        {
            if (showMessage) UICommon.MessageWarning($"Field \"{fieldName}\" must be non‑negative.");
            return false;
        }

        value = parsed;
        return true;
    }

    public static bool ValidatePositiveInt(string? input, out int value, string fieldName, bool showMessage = true)
    {
        value = 0;

        if (string.IsNullOrWhiteSpace(input))
        {
            if (showMessage) UICommon.MessageWarning($"Field \"{fieldName}\" cannot be empty.");
            return false;
        }

        if (!int.TryParse(input, NumberStyles.Integer, Culture, out var parsed))
        {
            if (showMessage) UICommon.MessageWarning($"Field \"{fieldName}\" must be a valid integer.");
            return false;
        }

        if (parsed < 0)
        {
            if (showMessage) UICommon.MessageWarning($"Field \"{fieldName}\" must be non‑negative.");
            return false;
        }

        value = parsed;
        return true;
    }
}
