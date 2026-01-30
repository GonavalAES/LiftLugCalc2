namespace LiftLugCalc2.Core.Models;

public sealed record ForwardInput //ForwardInput record bundles “project + chosen lug + chosen material” into one explicit input object for the forward calculation.
(
    Project Project,
    TableLug Lug,
    Material Material
);
