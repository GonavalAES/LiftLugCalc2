namespace LiftLugCalc2.Core.Models;

public sealed record ExploratoryValidationResult
(
    bool IsValid,
    IReadOnlyList<string> Errors,
    IReadOnlyList<string> Warnings
);
