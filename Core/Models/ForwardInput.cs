namespace LiftLugCalc2.Core.Models;

public sealed record ForwardInput //ForwardInput record is conceptually the right way to bundle “project + chosen lug + chosen material” into one explicit input object for the forward calculation.
(
    ProjectInput Project,
    TableLug Lug,
    Material Material
);
