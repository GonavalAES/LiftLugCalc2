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

    public TableLug? SelectedLug { get; set; }// table lug (Forward/Reverse)
    public Material? SelectedMaterial { get; set; }
    public CustomLug? CustomLug { get; set; }         // user-defined lug 

    public int? UserLugID { get; set; }
    public int? UserMaterialID { get; set; }

    public int? ExploratoryReferenceLugID { get; set; }
    public int ExploratoryLugType { get; set; }
    public double ExploratoryLugWLL { get; set; }

    public double ExploratoryThicknessPlate { get; set; }
    public double ExploratoryDiameterHole { get; set; }
    public double ExploratoryRadiusLug { get; set; }
    public double ExploratoryHeightCenterHole { get; set; }
    public double ExploratoryLengthLug { get; set; }
    public double ExploratoryHeightToe { get; set; }

    public double ExploratoryRadiusCheekBoss { get; set; }
    public double ExploratoryThicknessCheekBoss { get; set; }

    public double ExploratoryWeldThroatCheek { get; set; }
    public double ExploratoryLugWeldThroat { get; set; }

    public int ExploratoryBracket { get; set; }

    public bool ExploratoryChangeThicknessPlate { get; set; }
    public bool ExploratoryChangeDiameterHole { get; set; }
    public bool ExploratoryChangeRadiusLug { get; set; }
    public bool ExploratoryChangeHeightCenterHole { get; set; }
    public bool ExploratoryChangeLengthLug { get; set; }
    public bool ExploratoryChangeHeightToe { get; set; }

    public bool ExploratoryChangeRadiusCheekBoss { get; set; }
    public bool ExploratoryChangeThicknessCheekBoss { get; set; }

    public bool ExploratoryChangeWeldThroatCheek { get; set; }
    public bool ExploratoryChangeLugWeldThroat { get; set; }
}
