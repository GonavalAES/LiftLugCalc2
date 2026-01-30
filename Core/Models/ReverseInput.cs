namespace LiftLugCalc2.Core.Models;

public sealed record ReverseInput
(
    Project Project,
    Material Material,
    IReadOnlyList<TableLug> LugTable
);
