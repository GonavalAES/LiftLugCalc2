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

    public CustomLug ToCustomLug()
    {
        return new CustomLug
        (
            LugType: LugType,
            LugWLL: LugWLL,
            ThicknessPlate: ThicknessPlate,
            DiameterHole: DiameterHole,
            RadiusLug: RadiusLug,
            HeightCenterHole: HeightCenterHole,
            LengthLug: LengthLug,
            HeightToe: HeightToe,
            RadiusCheekBoss: RadiusCheekBoss,
            ThicknessCheekBoss: ThicknessCheekBoss,
            WeldThroatCheek: WeldThroatCheek,
            LugWeldThroat: LugWeldThroat,
            Bracket: Bracket
        );
    }

    public void CopyToProject(Project project, TableLug referenceLug)
    {
        project.ExploratoryReferenceLugID = referenceLug.LugID;
        project.ExploratoryLugType = LugType;
        project.ExploratoryLugWLL = LugWLL;
        project.ExploratoryThicknessPlate = ThicknessPlate;
        project.ExploratoryDiameterHole = DiameterHole;
        project.ExploratoryRadiusLug = RadiusLug;
        project.ExploratoryHeightCenterHole = HeightCenterHole;
        project.ExploratoryLengthLug = LengthLug;
        project.ExploratoryHeightToe = HeightToe;
        project.ExploratoryRadiusCheekBoss = RadiusCheekBoss;
        project.ExploratoryThicknessCheekBoss = ThicknessCheekBoss;
        project.ExploratoryWeldThroatCheek = WeldThroatCheek;
        project.ExploratoryLugWeldThroat = LugWeldThroat;
        project.ExploratoryBracket = Bracket;
        project.ExploratoryChangeThicknessPlate = ChangeThicknessPlate;
        project.ExploratoryChangeDiameterHole = ChangeDiameterHole;
        project.ExploratoryChangeRadiusLug = ChangeRadiusLug;
        project.ExploratoryChangeHeightCenterHole = ChangeHeightCenterHole;
        project.ExploratoryChangeLengthLug = ChangeLengthLug;
        project.ExploratoryChangeHeightToe = ChangeHeightToe;
        project.ExploratoryChangeRadiusCheekBoss = ChangeRadiusCheekBoss;
        project.ExploratoryChangeThicknessCheekBoss = ChangeThicknessCheekBoss;
        project.ExploratoryChangeWeldThroatCheek = ChangeWeldThroatCheek;
        project.ExploratoryChangeLugWeldThroat = ChangeLugWeldThroat;
    }

    public void LoadFromProject(Project project)
    {
        LugType = project.ExploratoryLugType;
        LugWLL = project.ExploratoryLugWLL;
        ThicknessPlate = project.ExploratoryThicknessPlate;
        DiameterHole = project.ExploratoryDiameterHole;
        RadiusLug = project.ExploratoryRadiusLug;
        HeightCenterHole = project.ExploratoryHeightCenterHole;
        LengthLug = project.ExploratoryLengthLug;
        HeightToe = project.ExploratoryHeightToe;
        RadiusCheekBoss = project.ExploratoryRadiusCheekBoss;
        ThicknessCheekBoss = project.ExploratoryThicknessCheekBoss;
        WeldThroatCheek = project.ExploratoryWeldThroatCheek;
        LugWeldThroat = project.ExploratoryLugWeldThroat;
        Bracket = project.ExploratoryBracket;
        ChangeThicknessPlate = project.ExploratoryChangeThicknessPlate;
        ChangeDiameterHole = project.ExploratoryChangeDiameterHole;
        ChangeRadiusLug = project.ExploratoryChangeRadiusLug;
        ChangeHeightCenterHole = project.ExploratoryChangeHeightCenterHole;
        ChangeLengthLug = project.ExploratoryChangeLengthLug;
        ChangeHeightToe = project.ExploratoryChangeHeightToe;
        ChangeRadiusCheekBoss = project.ExploratoryChangeRadiusCheekBoss;
        ChangeThicknessCheekBoss = project.ExploratoryChangeThicknessCheekBoss;
        ChangeWeldThroatCheek = project.ExploratoryChangeWeldThroatCheek;
        ChangeLugWeldThroat = project.ExploratoryChangeLugWeldThroat;
    }
}
