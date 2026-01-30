namespace LiftLugCalc2.Core.Models;

public record CalculationResult
{
    public string CalculationType { get; set; } = string.Empty; // Forward calculation or Reverse calculation
    public double AppliedLoad { get; set; }
    public double NetSectionArea { get; set; }
    public double TensionCapacity { get; set; }
    public double FSTension { get; set; }
    public double ShearCapacity { get; set; }
    public double FSShear { get; set; }
    public double BearingCapacity { get; set; }
    public double FSBearing { get; set; }
    public double TearOutCapacity { get; set; }
    public double FSTearOut { get; set; }
    public double WeldCapacity { get; set; }
    public double FSWeld { get; set; }
    public double MinimumFS { get; set; }
    public bool WeldGeometryOK { get; set; }
    public bool Pass { get; set; }
}
