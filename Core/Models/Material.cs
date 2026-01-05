namespace LiftLugCalc2.Core.Models
{
    public record Material
    (
        int MaterialID,
        string Designation,
        double YieldStrength,
        double TensileStrength,
        double YoungModulus,
        double PoissonRatio
    );
}
