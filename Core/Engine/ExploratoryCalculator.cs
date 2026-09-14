using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.Core.Engine;

public static class ExploratoryCalculator
{
    public static CalculationResult Run(
        ExploratoryInput input,
        string choice)
    {
        TableLug calculationLug =
            ConvertToCalculationLug(input.Lug);

        ForwardInput forwardInput = new(
            input.Project,
            calculationLug,
            input.Material);

        return ForwardCalculator.Run(
            forwardInput,
            choice);
    }

    private static TableLug ConvertToCalculationLug(
        CustomLug lug)
    {
        return new TableLug(
            LugID: -1,
            LugType: lug.LugType,
            LugWLL: lug.LugWLL,
            ThicknessPlate: lug.ThicknessPlate,
            DiameterHole: lug.DiameterHole,
            RadiusLug: lug.RadiusLug,
            HeightCenterHole: lug.HeightCenterHole,
            LengthLug: lug.LengthLug,
            HeightToe: lug.HeightToe,
            RadiusCheek_Boss: lug.RadiusCheekBoss,
            ThicknessCheek_Boss: lug.ThicknessCheekBoss,
            WeldThroatCheek: lug.WeldThroatCheek,
            LugWeldThroat: lug.LugWeldThroat,
            Bracket: lug.Bracket);
    }
}
