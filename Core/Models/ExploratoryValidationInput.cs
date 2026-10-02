namespace LiftLugCalc2.Core.Models;

public sealed record ExploratoryValidationInput
(
    CustomLug Lug,
    bool CheckLugWeldThroat,
    bool CheckCheekBossWeldThroat
);
