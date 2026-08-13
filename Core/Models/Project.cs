namespace LiftLugCalc2.Core.Models;

public record Project
{
    public int ProjectID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string CreatedBy { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;
    public string Revision { get; set; } = string.Empty;

    public int WeightBasis { get; set; }
    public double NominalWeightKg { get; set; }
    public int WcfSelection { get; set; }
    public double WCF { get; set; }

    public double WLL { get; set; }           // Total Working Load Limit
    public int NumberPoints { get; set; }     // Number of lifting points


    // Geometry of COG relative to lifting points
    public double A1 { get; set; }            // Distance from COG to 1st point (longitudinal)
    public double A2 { get; set; }            // Distance from COG to 2nd point (longitudinal)
    public double B1 { get; set; }            // Distance transverse 1
    public double B2 { get; set; }            // Distance transverse 2

    public TableLug? SelectedLug { get; set; }
    public Material? SelectedMaterial { get; set; }

    public int? UserLugID { get; set; }
    public int? UserMaterialID { get; set; }
}
