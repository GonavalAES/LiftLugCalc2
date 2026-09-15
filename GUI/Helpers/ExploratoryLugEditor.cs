using LiftLugCalc2.Core.Models;

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

    public void Reset()
    {
        LugType = 0;
        LugWLL = 0.0;

        ThicknessPlate = 0.0;
        DiameterHole = 0.0;
        RadiusLug = 0.0;
        HeightCenterHole = 0.0;
        LengthLug = 0.0;
        HeightToe = 0.0;

        RadiusCheekBoss = 0.0;
        ThicknessCheekBoss = 0.0;

        WeldThroatCheek = 0.0;
        LugWeldThroat = 0.0;

        Bracket = 0;

        ChangeLugWLL = false;

        ChangeThicknessPlate = false;
        ChangeDiameterHole = false;
        ChangeRadiusLug = false;
        ChangeHeightCenterHole = false;
        ChangeLengthLug = false;
        ChangeHeightToe = false;

        ChangeRadiusCheekBoss = false;
        ChangeThicknessCheekBoss = false;

        ChangeWeldThroatCheek = false;
        ChangeLugWeldThroat = false;
    }

    public void LoadFromReferenceLug(TableLug lug)
    {
        LugType = lug.LugType;
        LugWLL = lug.LugWLL;

        ThicknessPlate = lug.ThicknessPlate;
        DiameterHole = lug.DiameterHole;
        RadiusLug = lug.RadiusLug;
        HeightCenterHole = lug.HeightCenterHole;
        LengthLug = lug.LengthLug;
        HeightToe = lug.HeightToe;

        RadiusCheekBoss = lug.RadiusCheek_Boss;
        ThicknessCheekBoss = lug.ThicknessCheek_Boss;

        WeldThroatCheek = lug.WeldThroatCheek;
        LugWeldThroat = lug.LugWeldThroat;

        Bracket = lug.Bracket;

        ChangeLugWLL = false;

        ChangeThicknessPlate = false;
        ChangeDiameterHole = false;
        ChangeRadiusLug = false;
        ChangeHeightCenterHole = false;
        ChangeLengthLug = false;
        ChangeHeightToe = false;

        ChangeRadiusCheekBoss = false;
        ChangeThicknessCheekBoss = false;

        ChangeWeldThroatCheek = false;
        ChangeLugWeldThroat = false;
    }
}
