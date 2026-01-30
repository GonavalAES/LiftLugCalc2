using LiftLugCalc2.ConsoleFrontEnd;
using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.Core.Engine;

public static class ForwardCalculator
{
    public static CalculationResult Run(ForwardInput input)
    {
        var project = input.Project;
        var lug = input.Lug;
        var material = input.Material;

        var results = new CalculationResult
        {
            CalculationType = "Forward Calculation"
        };

        // 1. Calculate PLP (Point Load with factors)
        double plp = PreliminaryCalculations.CalculatePLP(project);
        results.AppliedLoad = plp;

        // 2. Perform lug checks
        LugPassCheck(project, lug, material, results);

        // 3. Return results to MainController
        return results;
    }



    private static void LugPassCheck(Project project, TableLug lug, Material material, CalculationResult results)
    {
        switch (lug.LugType)
        {
            case 0: // type 0
                CheckType0(project, lug, material, results);
                break;
            case 1: // type 1
                CheckType1(project, lug, material, results);
                break;
            case 2: // type 2
                CheckType2(project, lug, material, results);
                break;
            case 3: // type 3
                CheckType3(project, lug, material, results);
                break;
            default:
                results.Pass = false;
                break;
        }

        // Global pass / fail combining FS and geometry
        results.Pass = results.MinimumFS >= Constants.MINIMUM_SAFETY_FACTOR && results.WeldGeometryOK;
    }

    private static void WeldGeometryCheck(TableLug lug, CalculationResult results)
    {
        bool isValid = true;
        double t = lug.ThicknessPlate; // Check that weld throats are reasonable relative to plate thickness

        if (lug.LugWeldThroat > 0.0)
        {
            if (lug.LugWeldThroat > Constants.WELD_THROAT_MAX_RATIO * t)
            {
                //UICommon.MessageWarning($"Main weld throat {lug.LugWeldThroat:F1}mm > {Constants.WELD_THROAT_MAX_RATIO:P0} × plate {t:F0}mm - (Too thick)");
                isValid = false;
            }

            if (lug.LugWeldThroat < Constants.WELD_THROAT_MIN)
            {
                UICommon.MessageError($"Main weld throat {lug.LugWeldThroat:F1}mm < minimum {Constants.WELD_THROAT_MIN:F1}mm - (Too thin)");
                isValid = false;
            }
        }
        if (lug.WeldThroatCheek > 0.0)
        {
            double tc = lug.ThicknessCheek_Boss;

            if (lug.WeldThroatCheek > Constants.WELD_THROAT_MAX_RATIO * tc)
            {
                //UICommon.MessageWarning($"Cheek weld throat {lug.WeldThroatCheek:F1}mm > {Constants.WELD_THROAT_MAX_RATIO:P0} × cheek {tc:F0}mm");
                isValid = false;
            }
            if (lug.WeldThroatCheek < Constants.WELD_THROAT_MIN)
            {
                //UICommon.MessageWarning($"Cheek weld throat {lug.WeldThroatCheek:F1}mm < minimum {Constants.WELD_THROAT_MIN:F1}mm");
                isValid = false;
            }
        }

        // Check hole diameter vs lug radius
        if (lug.DiameterHole >= 1.8 * lug.RadiusLug)
        {
            UICommon.MessageError($"Hole {lug.DiameterHole:F1}mm > 1.8× lug radius {lug.RadiusLug:F1}mm - (Hole too large, risk of brittle fracture)");
            isValid = false;
        }

        // Check edge distance (hole to lug edge)
        double minEdgeDistance = Constants.MIN_EDGE_DISTANCE_FACTOR * lug.DiameterHole;
        double edgeDistance = lug.RadiusLug - (lug.DiameterHole * 0.5);

        if (edgeDistance < minEdgeDistance)
        {
            UICommon.MessageError($"Edge distance {edgeDistance:F1}mm < min {minEdgeDistance:F1}mm - Critical");

            isValid = false;
        }
        else if (edgeDistance < 1.0 * lug.DiameterHole)
        {
            //UICommon.MessageWarning($"Edge distance {edgeDistance:F1}mm - Marginal (0.8-1.0×d)");
            //UICommon.MessageWarning("Consider increasing lug size.");
            Console.WriteLine();
        }

        results.WeldGeometryOK = isValid;
    }



    private static void CalculateFactorsOfSafety(CalculationResult results)
    {
        //This does all the rations against the applied load and finds the minimum FS
        double appliedLoad = results.AppliedLoad;

        results.FSTension = results.TensionCapacity / appliedLoad;
        results.FSBearing = results.BearingCapacity / appliedLoad;
        results.FSTearOut = results.TearOutCapacity / appliedLoad;

        if (results.ShearCapacity > 0) results.FSShear = results.ShearCapacity / appliedLoad;
        else results.FSShear = 0;

        if (results.WeldCapacity > 0) results.FSWeld = results.WeldCapacity / appliedLoad;
        else results.FSWeld = 0;

        // Determine minimum FS (excluding zeros)
        var factorsOfSafety = new List<double>
        {
            results.FSTension,
            results.FSBearing,
            results.FSTearOut
        };

        if (results.FSShear > 0) factorsOfSafety.Add(results.FSShear);
        if (results.FSWeld > 0) factorsOfSafety.Add(results.FSWeld);

        results.MinimumFS = factorsOfSafety.Min();

        // Check pass/fail
        results.Pass = results.MinimumFS >= Constants.MINIMUM_SAFETY_FACTOR && results.WeldGeometryOK;
    }



    // --- CheckType0 --- Type 0: Direct connection - machined from structure, no welds
    // Computes only capacities/resistances related to lug plate
    public static void CheckType0(Project project, TableLug lug, Material material, CalculationResult results)
    {
        // 0. Parameters
        double fy = material.YieldStrength;         // MPa
        double t = lug.ThicknessPlate;              // mm
        double d = lug.DiameterHole;                // mm
        double R = lug.RadiusLug;                   // mm
        double h = lug.HeightCenterHole;            // mm

        // 1. Calculations
        double edgeDistance = h - (d * 0.5);

        // 2. Net section area (tension check)
        results.NetSectionArea = PreliminaryCalculations.NetSectionArea(R, d * 0.5, t);

        // 3. Tension capacity
        results.TensionCapacity = PreliminaryCalculations.TensionCapacity(results.NetSectionArea, fy);
        results.BearingCapacity = PreliminaryCalculations.BearingCapacity(d, t, fy);

        // 4. Bearing capacity
        results.TearOutCapacity = PreliminaryCalculations.TearOutCapacity(edgeDistance, t, fy);

        // 5. Type 0 has no welds or shear checks
        results.ShearCapacity = 0;
        results.WeldCapacity = 0;
        results.WeldGeometryOK = true; // N/A for Type 0

        // 6. Determine minimum FS amd return pass/fail
        CalculateFactorsOfSafety(results);
    }

    // --- CheckType1 --- Type 1: Single plate lug with fillet welds at base
    // Computes only capacities/resistances related to lug plate
    public static void CheckType1(Project project, TableLug lug, Material material, CalculationResult results)
    {
        // 0. Parameters
        double fy = material.YieldStrength;         // MPa
        double t = lug.ThicknessPlate;              // mm
        double d = lug.DiameterHole;                // mm
        double R = lug.RadiusLug;                   // mm
        double a = lug.LugWeldThroat;               // mm (weld throat)
        double L = lug.LengthLug;                   // mm (weld length on each side)
        double h = lug.HeightCenterHole;            // mm (height to hole center)

        // 1. Weld geometry check
        WeldGeometryCheck(lug, results);
        if (!results.WeldGeometryOK)
        {
            results.Pass = false;
            return; // Cannot proceed with invalid geometry
        }

        // 2. Calculations
        double shearArea = L * t * 2;
        double edgeDistance = h - (d * 0.5); // Edge distance from hole to top
        double weldArea = 2 * a * L; // Two welds, each with throat 'a' and length 'L'

        // 3. Net section area (tension check at hole)
        results.NetSectionArea = PreliminaryCalculations.NetSectionArea(R, d * 0.5, t);

        // 4. Tension capacity at net section
        results.TensionCapacity = PreliminaryCalculations.TensionCapacity(results.NetSectionArea, fy); // kN

        // 5. Shear capacity at base (through plate thickness, two welds)            
        results.ShearCapacity = PreliminaryCalculations.ShearCapacity(shearArea, fy); // kN

        // 6. Bearing capacity at pin hole
        results.BearingCapacity = PreliminaryCalculations.BearingCapacity(d, t, fy); // kN

        // 7. Tear-out capacity
        results.TearOutCapacity = PreliminaryCalculations.TearOutCapacity(edgeDistance, t, fy); // kN

        // 8. Weld capacity (fillet welds at base)
        results.WeldCapacity = PreliminaryCalculations.WeldCapacity(weldArea, fy); // kN

        // 9. Calculate factors of safety and check pass/fail
        CalculateFactorsOfSafety(results);
    }

    // --- CheckType2 --- Type 2: Cheek plate lug - main plate with two side cheek plates
    // Computes only capacities/resistances related to lug plate
    public static void CheckType2(Project project, TableLug lug, Material material, CalculationResult results)
    {
        // 0. Parameters
        double fy = material.YieldStrength;         // MPa
        double t = lug.ThicknessPlate;              // mm (main plate)
        double tc = lug.ThicknessCheek_Boss;        // mm (cheek plate thickness)
        double d = lug.DiameterHole;                // mm
        double R = lug.RadiusLug;                   // mm
        double Rc = lug.RadiusCheek_Boss;           // mm (cheek plate radius)
        double aMain = lug.LugWeldThroat;           // mm (main plate weld throat)
        double aCheek = lug.WeldThroatCheek;        // mm (cheek plate weld throat)
        double L = lug.LengthLug;                   // mm
        double h = lug.HeightCenterHole;            // mm

        // 1. Weld geometry check
        WeldGeometryCheck(lug, results);
        if (!results.WeldGeometryOK)
        {
            results.Pass = false;
            return;
        }

        // 2. Calculations
        double netAreaMain = PreliminaryCalculations.NetSectionArea(R, d * 0.5, t);
        double netAreaCheeks = PreliminaryCalculations.NetSectionArea(Rc, d * 0.5, tc) * 2;
        double shearArea = L * (t + 2 * tc) * 2;
        double totalThickness = t + 2 * tc;
        double edgeDistance = h - (d * 0.5);
        double weldAreaMain = 2 * aMain * L;
        double weldAreaCheeks = 2 * aCheek * L * 2;
        double totalWeldArea = weldAreaMain + weldAreaCheeks;

        // 3. Net section area (main plate + 2 cheek plates)
        results.NetSectionArea = netAreaMain + netAreaCheeks;

        // 4. Tension capacity
        results.TensionCapacity = PreliminaryCalculations.TensionCapacity(results.NetSectionArea, fy);

        // 5. Shear capacity (main plate + cheek plates)
        results.ShearCapacity = PreliminaryCalculations.ShearCapacity(shearArea, fy);

        // 6. Bearing capacity (distributed across all three plates)
        results.BearingCapacity = PreliminaryCalculations.BearingCapacity(d, totalThickness, fy);

        // 7. Tear-out capacity (consider all plates)
        results.TearOutCapacity = PreliminaryCalculations.TearOutCapacity(edgeDistance, totalThickness, fy);

        // 8. Weld capacity (main plate welds + cheek plate welds)
        results.WeldCapacity = PreliminaryCalculations.WeldCapacity(totalWeldArea, fy);

        // 9.Calculate factors of safety and check pass/ fail
        CalculateFactorsOfSafety(results);
    }

    // --- CheckType3 --- Type 3: Boss lug - cylindrical boss with lug plate on top
    // Computes only capacities/resistances related to lug plate
    public static void CheckType3(Project project, TableLug lug, Material material, CalculationResult results)
    {
        // 0. Parameters
        double fy = material.YieldStrength;             // MPa
        double t = lug.ThicknessPlate;              // mm (lug plate)
        double tb = lug.ThicknessCheek_Boss;        // mm (boss wall thickness)
        double d = lug.DiameterHole;                // mm
        double R = lug.RadiusLug;                   // mm (lug radius)
        double Rb = lug.RadiusCheek_Boss;           // mm (boss outer radius)
        double aLug = lug.LugWeldThroat;            // mm (lug-to-boss weld)
        double aBoss = lug.WeldThroatCheek;         // mm (boss-to-structure weld)
        double L = lug.LengthLug;                   // mm
        double h = lug.HeightCenterHole;            // mm

        // 1. Weld geometry check
        WeldGeometryCheck(lug, results);
        if (!results.WeldGeometryOK)
        {
            results.Pass = false;
            return;
        }

        // 2. Calculations
        double shearAreaLug = L * t * 2;
        double bossCircumference = 2 * Math.PI * (Rb - (tb * 0.5));
        double shearAreaBoss = bossCircumference * tb * 0.5;
        double totalShearArea = shearAreaLug + shearAreaBoss;
        double edgeDistance = h - d * 0.5;
        double lugPerimeter = 2 * (L + t); // Approximate perimeter
        double weldAreaLug = aLug * lugPerimeter; // Lug-to-boss weld (fillet around lug perimeter)            
        double bossWeldCircumference = 2 * Math.PI * Rb;
        double weldAreaBoss = aBoss * bossWeldCircumference; // Boss-to-structure weld (circumferential)

        // 3. Net section area (lug plate only, boss provides support)
        results.NetSectionArea = PreliminaryCalculations.NetSectionArea(R, d * 0.5, t);

        // 4. Tension capacity
        results.TensionCapacity = PreliminaryCalculations.TensionCapacity(results.NetSectionArea, fy);

        // 5. Shear capacity (through lug plate and boss wall)
        results.ShearCapacity = PreliminaryCalculations.ShearCapacity(totalShearArea, fy);

        // 6. Bearing capacity (lug plate thickness)
        results.BearingCapacity = PreliminaryCalculations.BearingCapacity(d, t, fy);

        // 7. Tear-out capacity            
        results.TearOutCapacity = PreliminaryCalculations.TearOutCapacity(edgeDistance, t, fy);

        // 8. Weld capacity
        // Take the weaker of the two welds as governing
        double weldCapacityLug = PreliminaryCalculations.WeldCapacity(weldAreaLug, fy);
        double weldCapacityBoss = PreliminaryCalculations.WeldCapacity(weldAreaBoss, fy);
        results.WeldCapacity = Math.Min(weldCapacityLug, weldCapacityBoss);

        // 9.Calculate factors of safety and check pass/ fail
        CalculateFactorsOfSafety(results);
    }
}
