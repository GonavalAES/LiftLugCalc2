namespace LiftLugCalc2.Core.Models;

public sealed record UserLugAndMaterial //this type cannot be used as a base type. Others cannot inherit from it
(
    int ProjectID,
    int LugID,
    int MaterialID
);
