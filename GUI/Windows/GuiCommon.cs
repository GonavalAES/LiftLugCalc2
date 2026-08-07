using ImGuiNET;

using System.Numerics;

namespace LiftLugCalc2.GUI.Windows;

/// <summary>
/// Common ImGui drawing helpers.
/// Contains only reusable GUI drawing code.
/// No engineering logic.
/// </summary>
public static class GuiCommon
{
    //------------------------------------------------------------
    // Panels
    //------------------------------------------------------------

    public static void BeginPanel(
        string id,
        Vector2 size)
    {
        ImGui.BeginChild(
            id,
            size,
            ImGuiChildFlags.Borders,
            ImGuiWindowFlags.None);
    }

    public static void EndPanel()
    {
        ImGui.EndChild();
    }

    //------------------------------------------------------------
    // Section headers
    //------------------------------------------------------------

    public static void SectionHeader(string title)
    {
        ImGui.SeparatorText(title);
    }

    //------------------------------------------------------------
    // Vertical spacing
    //------------------------------------------------------------

    public static void Spacer(float lines = 1.0f)
    {
        for (int i = 0; i < lines; i++)
            ImGui.Spacing();
    }

    //------------------------------------------------------------
    // Horizontal separator
    //------------------------------------------------------------

    public static void Separator()
    {
        ImGui.Separator();
    }

    //------------------------------------------------------------
    // Label + value
    //------------------------------------------------------------

    public static void LabelValue(
        string label,
        string value)
    {
        ImGui.Text($"{label} : {value}");
    }

    public static void LabelValue(
        string label,
        double value,
        string unit = "")
    {
        if (string.IsNullOrWhiteSpace(unit))
            ImGui.Text($"{label} : {value:F2}");
        else
            ImGui.Text($"{label} : {value:F2} {unit}");
    }

    //------------------------------------------------------------
    // Status message
    //------------------------------------------------------------

    public static void StatusMessage(
        StatusType type,
        string message)
    {
        Vector4 colour = type switch
        {
            StatusType.Information => new Vector4(0.85f, 0.85f, 0.85f, 1.0f),
            StatusType.Warning => new Vector4(1.00f, 0.80f, 0.20f, 1.0f),
            StatusType.Error => new Vector4(1.00f, 0.35f, 0.35f, 1.0f),
            _ => Vector4.One
        };

        ImGui.TextColored(colour, message);
    }

    //------------------------------------------------------------
    // Centered text
    //------------------------------------------------------------

    public static void CenteredText(string text)
    {
        float windowWidth = ImGui.GetWindowSize().X;
        float textWidth = ImGui.CalcTextSize(text).X;

        ImGui.SetCursorPosX((windowWidth - textWidth) * 0.5f);

        ImGui.Text(text);
    }

    //------------------------------------------------------------
    // Fixed-width button
    //------------------------------------------------------------

    public static bool Button(string text)
    {
        return ImGui.Button(
            text,
            new Vector2(
                GuiLayout.ButtonWidth,
                GuiLayout.ButtonHeight));
    }
}
