using LiftLugCalc2.ConsoleFrontEnd;
using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.Core.FileOperations;

public static class TableLugLoader
{
    private const char Separator = ';';

    public static List<TableLug> LoadFromCsv()
    {
        var list = new List<TableLug>();
        string relativePath = Path.Combine("Core/Resources", Constants.FILE_LUGS);
        string csvPath = FilingSystem.GetFilePath(relativePath);

        if (!File.Exists(csvPath))
        {
            UICommon.MessageError($"> Lug CSV not found at: {csvPath}");
            return list;
        }

        foreach (var line in File.ReadLines(csvPath).Skip(1)) // skip header
        {
            try
            {
                var fields = line.Split(Separator);
                var culture = new System.Globalization.CultureInfo("pt-PT");

                var tableLug = new TableLug
                (
                    LugID: int.Parse(fields[0]),
                    LugType: int.Parse(fields[1]),
                    LugWLL: double.Parse(fields[2], culture),
                    ThicknessPlate: double.Parse(fields[3], culture),
                    DiameterHole: double.Parse(fields[4], culture),
                    RadiusLug: double.Parse(fields[5], culture),
                    HeightCenterHole: double.Parse(fields[6], culture),
                    LengthLug: double.Parse(fields[7], culture),
                    HeightToe: double.Parse(fields[8], culture),
                    RadiusCheek_Boss: double.Parse(fields[9], culture),
                    ThicknessCheek_Boss: double.Parse(fields[10], culture),
                    WeldThroatCheek: double.Parse(fields[11], culture),
                    LugWeldThroat: double.Parse(fields[12], culture),
                    Bracket: int.Parse(fields[13])
                );

                list.Add(tableLug);
            }
            catch (FormatException ex)
            {
                UICommon.MessageError($"> Lug CSV parse error (line '{line}'): {ex.Message}");
                continue;
            }
            catch (IndexOutOfRangeException)
            {
                UICommon.MessageError($"> Lug CSV malformed (line '{line}' - missing columns)");
                continue;
            }
            catch (Exception ex)
            {
                UICommon.MessageError($"> Lug CSV unexpected error (line '{line}'): {ex.Message}");
                continue;
            }
        }

        return list;
    }
}
