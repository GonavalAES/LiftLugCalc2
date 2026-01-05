using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.Core.FileOperations;

public static class MaterialLoader
{
    private const char Separator = ';';

    public static List<Material> LoadFromCsv()
    {
        var list = new List<Material>();
        string csvPath = FilingSystem.GetFilePath(Path.Combine("Resources", Constants.FILE_MATERIALS));

        if (!File.Exists(csvPath))
        {
            // UI error message here
            return list; // Return empty list
        }

        foreach (var line in File.ReadLines(csvPath).Skip(1)) // skip header
        {
            try
            {
                var fields = line.Split(Separator);
                var culture = new System.Globalization.CultureInfo("pt-PT");

                var mat = new Material
                (
                    MaterialID: int.Parse(fields[0]),
                    Designation: fields[1],
                    YieldStrength: double.Parse(fields[2], culture),
                    TensileStrength: double.Parse(fields[3], culture),
                    YoungModulus: double.Parse(fields[4], culture),
                    PoissonRatio: double.Parse(fields[5], culture)
                );

                list.Add(mat);
            }
            catch (FormatException ex)
            {
                // UI error message here
                continue;
            }
            catch (IndexOutOfRangeException)
            {
                // UI error message here
                continue;
            }
            catch (Exception ex)
            {
                // UI error message here
                continue;
            }
        }

        return list;
    }
}
