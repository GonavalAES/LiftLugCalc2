using LiftLugCalc2.Core.Models;
using LiftLugCalc2.Core.Utilities;

namespace LiftLugCalc2.GUI.Helpers;

public static class ExploratoryLugValidator
{
    public static ExploratoryValidationResult Validate(ExploratoryValidationInput input)
    {
        CustomLug lug = input.Lug;

        List<string> errors = new();
        List<string> warnings = new();

        ValidateLugType(lug, errors);
        ValidateGeneralDimensions(lug, errors);
        ValidateHoleGeometry(lug, errors, warnings);
        ValidateMainWeld(lug, input.CheckLugWeldThroat, errors, warnings);
        ValidateCheekOrBossGeometry(lug, input.CheckCheekBossWeldThroat, errors, warnings);

        bool isValid = errors.Count == 0;

        return new ExploratoryValidationResult
        (
            isValid,
            errors,
            warnings
        );
    }

    private static void ValidateLugType(CustomLug lug, List<string> errors)
    {
        if (lug.LugType is < 0 or > 3) errors.Add("Lug type must be 0, 1, 2, or 3.");
    }

    private static void ValidateGeneralDimensions(CustomLug lug, List<string> errors)
    {
        ValidateRange(lug.ThicknessPlate, Constants.EXPLORATORY_MIN_PLATE_THICKNESS, Constants.EXPLORATORY_MAX_PLATE_THICKNESS,
                      "Plate thickness", "mm", errors);
        ValidateRange(lug.DiameterHole, Constants.EXPLORATORY_MIN_HOLE_DIAMETER, Constants.EXPLORATORY_MAX_HOLE_DIAMETER,
                      "Hole diameter", "mm", errors);
        ValidateRange(lug.RadiusLug, Constants.EXPLORATORY_MIN_LUG_RADIUS, Constants.EXPLORATORY_MAX_LUG_RADIUS, "Lug radius",
                      "mm", errors);
        ValidateRange(lug.HeightCenterHole, Constants.EXPLORATORY_MIN_HOLE_CENTRE_HEIGHT, Constants.EXPLORATORY_MAX_HOLE_CENTRE_HEIGHT,
                      "Hole centre height", "mm", errors);
        ValidateRange(lug.LengthLug, Constants.EXPLORATORY_MIN_LUG_LENGTH, Constants.EXPLORATORY_MAX_LUG_LENGTH, "Lug length",
                      "mm", errors);
        ValidateRange(lug.HeightToe, Constants.EXPLORATORY_MIN_TOE_HEIGHT, Constants.EXPLORATORY_MAX_TOE_HEIGHT, "Toe height",
                      "mm", errors);
    }

    private static void ValidateHoleGeometry(CustomLug lug, List<string> errors, List<string> warnings)
    {
        double holeRadius = lug.DiameterHole * 0.5;
        double topEdgeDistance = lug.HeightCenterHole - holeRadius;

        if (lug.DiameterHole >= 2.0 * lug.RadiusLug) errors.Add("Hole diameter is equal to or greater than the lug diameter.");
        if (holeRadius >= lug.RadiusLug) errors.Add("Hole radius must be smaller than lug radius.");
        if (lug.HeightCenterHole <= holeRadius) errors.Add("Hole centre height must be greater than the hole radius.");
        if (topEdgeDistance <= 0.0) errors.Add("The hole reaches or exceeds the lug base/toe line.");
    }

    private static void ValidateMainWeld(CustomLug lug, bool checkLugWeldThroat, List<string> errors, List<string> warnings)
    {
        if (lug.LugType == 0) return;
        if (!checkLugWeldThroat) return;

        ValidateRange(lug.LugWeldThroat, Constants.EXPLORATORY_MIN_WELD_THROAT, Constants.EXPLORATORY_MAX_WELD_THROAT,
                      "Main weld throat", "mm", errors);

        double maximumWeldThroat = lug.ThicknessPlate * Constants.EXPLORATORY_MAX_WELD_TO_PLATE_RATIO;

        if (lug.LugWeldThroat > maximumWeldThroat)
            errors.Add($"Main weld throat is {lug.LugWeldThroat:F1} mm. It cannot exceed {maximumWeldThroat:F1} mm " +
                       $"for a {lug.ThicknessPlate:F1} mm plate.");

        if (lug.LugWeldThroat > lug.HeightToe) warnings.Add($"Main weld throat is greater than toe height " +
                                                            $"({lug.HeightToe:F1} mm). Verify the base geometry.");
    }

    private static void ValidateCheekOrBossGeometry(CustomLug lug, bool checkCheekBossWeldThroat, List<string> errors,
                                                    List<string> warnings)
    {
        if (lug.LugType is not 2 and not 3) return;

        ValidateRange(lug.RadiusCheekBoss, Constants.EXPLORATORY_MIN_CHEEK_BOSS_RADIUS, Constants.EXPLORATORY_MAX_CHEEK_BOSS_RADIUS,
                      "Cheek/boss radius", "mm", errors);
        ValidateRange(lug.ThicknessCheekBoss, Constants.EXPLORATORY_MIN_CHEEK_BOSS_THICKNESS,
                      Constants.EXPLORATORY_MAX_CHEEK_BOSS_THICKNESS, "Cheek/boss thickness", "mm", errors);

        if (lug.RadiusCheekBoss < lug.DiameterHole * 0.5) errors.Add("Cheek/boss radius must be greater than the hole radius.");
        if (lug.RadiusCheekBoss > lug.RadiusLug) warnings.Add("Cheek/boss radius is greater than lug radius. " +
                                                              "Verify that this geometry is intentional.");

        if (!checkCheekBossWeldThroat) return;

        ValidateRange(lug.WeldThroatCheek, Constants.EXPLORATORY_MIN_WELD_THROAT, Constants.EXPLORATORY_MAX_WELD_THROAT,
                      "Cheek/boss weld throat", "mm", errors);

        double maximumCheekWeldThroat = lug.ThicknessCheekBoss * Constants.EXPLORATORY_MAX_WELD_TO_CHEEK_RATIO;

        if (lug.WeldThroatCheek > maximumCheekWeldThroat)
            errors.Add($"Cheek/boss weld throat is {lug.WeldThroatCheek:F1} mm. It cannot exceed " +
                $"{maximumCheekWeldThroat:F1} mm for a {lug.ThicknessCheekBoss:F1} mm cheek/boss thickness.");
    }

    private static void ValidateRange(double value, double minimum, double maximum, string propertyName, string unit,
                                      List<string> errors)
    {
        if (value < minimum || value > maximum) errors.Add($"{propertyName} must be between  {minimum:F1} and {maximum:F1} {unit}. " +
                                                           $"Entered value: {value:F1} {unit}.");
    }
}
