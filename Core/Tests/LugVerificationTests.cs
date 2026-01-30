using LiftLugCalc2.Core.Engine;
using LiftLugCalc2.Core.FileOperations;
using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.Core.Core.Tests;

public static class LugVerificationTests
{
    public static void RunAll()
    {
        Console.WriteLine(" --- Running Lug Verification Tests... --- ");
        Console.WriteLine(new string('=', 60));

        TestNetSectionArea();
        TestTensionCapacity();
        TestBearingCapacity();
        TestTearOutCapacity();
        TestForwardCalculatorType0();
        TestReverseCalculator();

        Console.WriteLine(new string('=', 60));
        Console.WriteLine("[OK]] All tests completed.");
    }

    private static void TestNetSectionArea()
    {
        // Known case: R=50mm, d=30mm hole, t=20mm plate
        double lugRadius = 50.0;
        double holeRadius = 15.0;  // d/2
        double thickness = 20.0;

        double expected = 700.0;  // 2*(50-15)*20 = 700 mm²

        double actual = PreliminaryCalculations.NetSectionArea(lugRadius, holeRadius, thickness);

        Assert.NearlyEqual(actual, expected, 0.1,
            $"Net section: R={lugRadius}, hole={holeRadius * 2}, t={thickness} → {expected} mm²");
    }

    private static void TestTensionCapacity()
    {
        double netArea = 700.0;     // mm²
        double fy = 355.0;          // MPa (S355)

        double expected = 216.5;    // (355/1.15)*700/1000 ≈ 216 kN

        double actual = PreliminaryCalculations.TensionCapacity(netArea, fy);

        Assert.NearlyEqual(actual, expected, 0.1,
            $"Tension: A={netArea}mm², fy={fy}MPa → {expected} kN");
    }

    private static void TestBearingCapacity()
    {
        double d = 30.0;            // mm
        double t = 20.0;            // mm
        double fy = 355.0;          // MPa

        double expected = 357.0;    // 1.5*fy*d*t/1000 ≈ 357 kN

        double actual = PreliminaryCalculations.BearingCapacity(d, t, fy);

        Assert.NearlyEqual(actual, expected, 0.1,
            $"Bearing: d={d}, t={t}, fy={fy} → {expected} kN");
    }

    private static void TestTearOutCapacity()
    {
        double edgeDist = 20.0;     // mm
        double t = 20.0;            // mm
        double fy = 355.0;          // MPa

        double expected = 141.3;    // 2*e*t*fy/(√3*1.15)/1000 ≈ 141 kN

        double actual = PreliminaryCalculations.TearOutCapacity(edgeDist, t, fy);

        Assert.NearlyEqual(actual, expected, 0.1,
            $"Tear-out: e={edgeDist}, t={t}, fy={fy} → {expected} kN");
    }

    private static void TestForwardCalculatorType0()
    {
        // Test data: simple 4-point lift, Type 0 lug, S355
        var project = new Project
        {
            ProjectID = 999,
            WLL = 10000.0,      // 10 tonnes
            NumberPoints = 4,
            A1 = 1.0,
            A2 = 1.0,
            B1 = 1.0,
            B2 = 1.0
        };

        var lug = new TableLug(
            LugID: 1,
            LugType: 0,                 // Type 0
            LugWLL: 1000.0,
            ThicknessPlate: 20.0,
            DiameterHole: 30.0,
            RadiusLug: 50.0,
            HeightCenterHole: 65.0,     // edge dist = 65-15 = 50mm
            LengthLug: 80.0,
            HeightToe: 15.0,
            RadiusCheek_Boss: 0.0,
            ThicknessCheek_Boss: 0.0,
            WeldThroatCheek: 0.0,
            LugWeldThroat: 0.0,
            Bracket: 0
        );

        var material = new Material(
            MaterialID: 1,
            Designation: "S355",
            YieldStrength: 355.0,
            TensileStrength: 510.0,
            YoungModulus: 210000.0,
            PoissonRatio: 0.3
        );

        var input = new ForwardInput(project, lug, material);
        var result = ForwardCalculator.Run(input);

        Assert.Greater(result.FSTension, 2.0, "Type 0 tension FS > 2.0");
        Assert.Greater(result.FSBearing, 2.0, "Type 0 bearing FS > 2.0");
        Assert.True(result.Pass, "Type 0 lug passes verification");
        Assert.True(result.WeldGeometryOK, "Type 0 weld geometry OK");
    }

    private static void TestReverseCalculator()
    {
        // Load real data for reverse test
        var lugs = TableLugLoader.LoadFromCsv();
        var materials = MaterialLoader.LoadFromCsv();

        var project = new Project
        {
            ProjectID = 888,
            WLL = 15000.0,      // 15 tonnes
            NumberPoints = 4,
            A1 = 1.2,
            A2 = 1.2,
            B1 = 1.0,
            B2 = 1.0
        };

        var material = materials.First(m => m.MaterialID == 1); // S355

        var input = new ReverseInput(project, material, lugs);
        var selection = ReverseCalculator.Run(input);

        Assert.True(selection.Best is not null, "Reverse calculator finds at least one suitable lug");
        Assert.True(selection.Best!.Lug.LugWLL >= project.WLL, $"Best lug WLL {selection.Best.Lug.LugWLL} >= required {project.WLL}");
        Assert.True(selection.Best.Result.Pass, "Best lug passes verification");
    }
}
