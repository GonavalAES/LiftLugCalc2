using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.Core.Engine;

public static class ReverseCalculator
{
    public static ReverseSelection Run(ReverseInput input)
    {
        var project = input.Project;
        var material = input.Material;
        var lugTable = input.LugTable;

        // 1. Design load per lug (PLP)
        double plp = PreliminaryCalculations.CalculatePLP(project);

        var passed = new List<ReverseCandidate>();

        // 2. Evaluate all lugs in the table
        foreach (var lug in lugTable)
        {
            var forwardInput = new ForwardInput(project, lug, material);
            var results = ForwardCalculator.Run(forwardInput);

            if (results.Pass) passed.Add(new ReverseCandidate(lug, results));
        }

        // 3. Choose “best” lug among passing ones
        ReverseCandidate? best = null;

        if (passed.Count > 0)
        {
            // Example: minimize LugType then LugWLL, then LugID
            best = passed
                .OrderBy(c => c.Lug.LugWLL)
                .ThenBy(c => c.Lug.LugType)
                .ThenBy(c => c.Lug.LugID)
                .First();
        }

        return new ReverseSelection(
            AppliedLoad: plp,
            Passed: passed,
            Best: best
        );
    }
}

// --- Helper temporary records ---

// One candidate result per lug
public sealed record ReverseCandidate
(
    TableLug Lug,
    CalculationResult Result
);

// Final selection
public sealed record ReverseSelection
(
    double AppliedLoad,                     // PLP used (kN)
    IReadOnlyList<ReverseCandidate> Passed, // all passing lugs
    ReverseCandidate? Best                  // chosen “minimum” lug (e.g. lowest LugType/ID)
);
