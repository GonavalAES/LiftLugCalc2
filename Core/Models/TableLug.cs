namespace LiftLugCalc2.Core.Models
{
    public record TableLug
    (
        int LugID,
        int LugType,
        double LugWLL,
        double ThicknessPlate,
        double DiameterHole,
        double RadiusLug,
        double HeightCenterHole,
        double LengthLug,
        double HeightToe,
        double RadiusCheek_Boss,
        double ThicknessCheek_Boss,
        double WeldThroatCheek,
        double LugWeldThroat,
        int Bracket
    );
}
