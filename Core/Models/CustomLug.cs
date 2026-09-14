namespace LiftLugCalc2.Core.Models;

public sealed record CustomLug
(
    int LugType,
    double LugWLL,
    double ThicknessPlate,
    double DiameterHole,
    double RadiusLug,
    double HeightCenterHole,
    double LengthLug,
    double HeightToe,
    double RadiusCheekBoss,
    double ThicknessCheekBoss,
    double WeldThroatCheek,
    double LugWeldThroat,
    int Bracket
);
