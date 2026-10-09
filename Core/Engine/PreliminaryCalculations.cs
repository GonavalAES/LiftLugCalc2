using LiftLugCalc2.Core.Models;
using LiftLugCalc2.Core.Utilities;

namespace LiftLugCalc2.Core.Engine;

public static class PreliminaryCalculations
{
    public static double ChoosingWCF(int choice) =>
        choice switch
        {
            1 => Constants.WCF_MEASURED,
            2 => Constants.WCF_DETAILED_UPDATED,
            3 => Constants.WCF_DETAILED_OLDER,
            4 => Constants.WCF_STANDARD,
            _ => Constants.WCF_DEFAULT // default safe
        };

    public static double CalculatePLP(Project project)
    {
        double wllKN = project.WLL * Constants.KG_TO_KN;
        double pointLoadRatio = CalculatePointLoadRatio(project);

        return wllKN * pointLoadRatio;
    }

    public static double CalculateIndicativeMaximumWllKg(Project project, double allowablePointLoad)
    {
        if (allowablePointLoad <= 0.0) return 0.0;

        double pointLoadRatio = CalculatePointLoadRatio(project);

        if (pointLoadRatio <= 0.0) return 0.0;

        double allowableWllKN = allowablePointLoad / pointLoadRatio;

        return allowableWllKN / Constants.KG_TO_KN;
    }

    private static double CalculatePointLoadRatio(Project project)
    {
        int numberPoints = project.NumberPoints;

        double loadDistributionRatio = CalculateLoadDistributionRatio(project, numberPoints);

        return loadDistributionRatio * Constants.DAF;
    }

    private static double CalculateLoadDistributionRatio(Project project, int numberPoints)
    {
        if (numberPoints == 1) return 1.0;
        if (numberPoints == 2) return CalculateTwoPointLoadRatio(project);
        if (numberPoints == 3) return CalculateThreePointLoadRatio(project);
        if (numberPoints == 4) return CalculateFourPointLoadRatio(project);

        return 0.0;
    }

    private static double CalculateTwoPointLoadRatio(Project project)
    {
        double totalA = project.A1 + project.A2;

        if (totalA > 0.0)
            return Math.Max(project.A1, project.A2) / totalA;

        return 0.5;
    }

    private static double CalculateThreePointLoadRatio(Project project)
    {
        double sumA = project.A1 + project.A2;
        double sumB = project.B1 + project.B2;

        if (sumA > 0.0 && sumB > 0.0)
            return (project.A1 * project.B1) / (sumA * sumB);

        return 0.333333;
    }

    private static double CalculateFourPointLoadRatio(Project project)
    {
        double sumA = project.A1 + project.A2;
        double sumB = project.B1 + project.B2;

        if (sumA > 0.0 && sumB > 0.0) return (project.A1 * project.B1) / (sumA * sumB);

        return 0.25;
    }

    // ---------- Capacity calculations ----------
    public static double NetSectionArea(double lugRadius, double holeRadius, double thickness) =>
        2.0 * (lugRadius - holeRadius) * thickness;

    public static double TensionCapacity(double netArea, double fy) =>
        (fy / Constants.GAMMA_RM_STRUCTURAL) * netArea / 1000.0;

    public static double BearingCapacity(double holeDiameter, double thickness, double fy) =>
        (Constants.BEARING_FACTOR * fy * holeDiameter * thickness) / 1000.0;

    public static double TearOutCapacity(double edgeDistance, double thickness, double fy) =>
        (2.0 * edgeDistance * thickness * fy / (Math.Sqrt(3.0) * Constants.GAMMA_RM_STRUCTURAL)) / 1000.0;

    public static double ShearCapacity(double shearArea, double fy) =>
        (fy / (Math.Sqrt(3.0) * Constants.GAMMA_RM_STRUCTURAL)) * shearArea / 1000.0;

    public static double WeldCapacity(double weldArea, double fy) =>
        (fy / (Math.Sqrt(3.0) * Constants.GAMMA_RM_FILLET) * weldArea) / 1000.0;
}
