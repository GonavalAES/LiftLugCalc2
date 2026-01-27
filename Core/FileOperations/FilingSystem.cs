using LiftLugCalc2.ConsoleFrontEnd;

using System.Reflection;

namespace LiftLugCalc2.Core.FileOperations;

public static class FilingSystem
{
    public static string BaseDirectory { get; }

    static FilingSystem()
    {
        BaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "LiftingLugCalc2");
        EnsureDirectoryExists(BaseDirectory);
    }

    public static string GetProjectDirectory(string projectName) => Path.Combine(BaseDirectory, projectName);

    public static string GetProjectSubDirectory(string projectName, string subDirName) =>
        Path.Combine(GetProjectDirectory(projectName), subDirName);

    public static void EnsureProjectDirectoriesExist(string projectName)
    {
        string[] subDirs = { "Logs", "Data", "Results" };
        foreach (var subDir in subDirs)
            EnsureDirectoryExists(GetProjectSubDirectory(projectName, subDir));
    }

    public static void EnsureDirectoryExists(string path)
    {
        if (!Directory.Exists(path)) Directory.CreateDirectory(path);
    }

    // Gets the folder path where the currently executing assembly (the .exe) resides
    public static string GetExecutableDirectory()
    {
        string exePath = Assembly.GetExecutingAssembly().Location;
        string? exeDir = Path.GetDirectoryName(exePath);
        return exeDir!;
    }

    // Combines executable directory with a relative filename or folder and returns full path
    public static string GetFilePath(string relativePath)
    {
        string baseDir = GetExecutableDirectory();
        return Path.Combine(baseDir ?? ".", relativePath);
    }

    // List all projects in the base directory
    public static List<string?> GetAllProjectNames()
    {
        try
        {
            return Directory.
                GetDirectories(BaseDirectory).
                Select(Path.GetFileName).
                Where(name => !string.IsNullOrEmpty(name)).
                ToList();
        }
        catch (Exception ex)
        {
            UICommon.MessageError($"> Project listing failed: {ex.Message}");
            return new List<string?>();
        }
    }


    public static void SaveTextFile(string filePath, string content) => File.WriteAllText(filePath, content);

    public static string LoadTextFile(string filePath) => File.Exists(filePath) ? File.ReadAllText(filePath) : string.Empty;

}
