using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.Core.Engine;

public static class PreliminaryCalculations
{
    public static double ChoosingWCF(string choice) =>
        choice switch
        {
            "1" => Constants.WCF_MEASURED,
            "2" => Constants.WCF_DETAILED_UPDATED,
            "3" => Constants.WCF_DETAILED_OLDER,
            "4" => Constants.WCF_STANDARD,
            _ => Constants.WCF_DEFAULT // default safe
        };

    public static double CalculatePLP(Project project)
    {
        // Convert WLL from kg to kN
        double wllKN = project.WLL * Constants.KG_TO_KN;

        int numPoints = project.NumberPoints;
        double plp = wllKN;

        // Distribute load based on number of points (NORSOK R-002)
        if (numPoints == 1) plp = wllKN;

        else if (numPoints == 2) // For 2 points, consider geometry
        {
            double totalA = project.A1 + project.A2;

            if (totalA > 0) plp = wllKN * Math.Max(project.A1, project.A2) / totalA; // Worst case - higher loaded point
            else plp = wllKN * 0.5;
        }

        else if (numPoints == 3)
        {
            double sumA = project.A1 + project.A2;
            double sumB = project.B1 + project.B2;

            if (sumA > 0 && sumB > 0) plp = wllKN * (project.A1 * project.B1) / (sumA * sumB);
            else plp = wllKN * 0.333333;
        }

        else if (numPoints == 4)
        {
            double sumA = project.A1 + project.A2;
            double sumB = project.B1 + project.B2;

            if (sumA > 0 && sumB > 0) plp = wllKN * (project.A1 * project.B1) / (sumA * sumB);
            else plp = wllKN * 0.25;
        }

        plp *= Constants.DAF; // Apply dynamic amplification factor (DAF) per NORSOK

        return plp;
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
