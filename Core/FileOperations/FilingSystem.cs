using LiftLugCalc2.ConsoleFrontEnd;
using LiftLugCalc2.Core.Models;

using System.Reflection;

namespace LiftLugCalc2.Core.FileOperations;

public static class FilingSystem
{
    public static string BaseDirectory { get; }

    // COnstructor to set up base directory
    static FilingSystem()
    {
        BaseDirectory = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), Constants.BASE_DIRECTORY);
        Directory.CreateDirectory(BaseDirectory);
    }

    // Full path to project folder (creates if missing)
    public static string GetProjectDirectory(string projectName)
    {
        string projectDir = Path.Combine(BaseDirectory, projectName);
        Directory.CreateDirectory(projectDir);

        return projectDir;
    }

    public static string EnsureProjectDirectory(string projectName) => GetProjectDirectory(projectName);

    // Gets the folder path where the currently executing assembly (the .exe) resides
    public static string GetExecutableDirectory() => Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location) ?? ".";

    // Combines executable directory with a relative filename or folder and returns full path
    public static string GetFilePath(string relativePath) => Path.Combine(GetExecutableDirectory(), relativePath);

    // List all projects in the base directory
    public static List<string?> GetAllProjectNames()
    {
        try
        {
            return Directory.GetDirectories(BaseDirectory).Select(Path.GetFileName).Where(name => !string.IsNullOrEmpty(name)).ToList();
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
