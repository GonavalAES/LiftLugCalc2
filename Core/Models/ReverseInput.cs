namespace LiftLugCalc2.Core.Models;

public sealed record ReverseInput
(
    ProjectInput Project,
    Material Material,
    IReadOnlyList<TableLug> LugTable
);
