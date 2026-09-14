namespace LiftLugCalc2.GUI.Helpers;

public sealed class ExploratoryLugEditor
{
    public int LugType { get; set; }

    public double LugWLL { get; set; }

    public double ThicknessPlate { get; set; }
    public double DiameterHole { get; set; }
    public double RadiusLug { get; set; }
    public double HeightCenterHole { get; set; }
    public double LengthLug { get; set; }
    public double HeightToe { get; set; }

    public double RadiusCheekBoss { get; set; }
    public double ThicknessCheekBoss { get; set; }

    public double WeldThroatCheek { get; set; }
    public double LugWeldThroat { get; set; }

    public int Bracket { get; set; }

    public bool ChangeLugWLL { get; set; }

    public bool ChangeThicknessPlate { get; set; }
    public bool ChangeDiameterHole { get; set; }
    public bool ChangeRadiusLug { get; set; }
    public bool ChangeHeightCenterHole { get; set; }
    public bool ChangeLengthLug { get; set; }
    public bool ChangeHeightToe { get; set; }

    public bool ChangeRadiusCheekBoss { get; set; }
    public bool ChangeThicknessCheekBoss { get; set; }

    public bool ChangeWeldThroatCheek { get; set; }
    public bool ChangeLugWeldThroat { get; set; }
}
