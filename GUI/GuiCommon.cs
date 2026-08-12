using ImGuiNET;

using LiftLugCalc2.GUI.Enums;

using System.Numerics;

namespace LiftLugCalc2.GUI;

public static class GuiCommon
{
    public static void BeginPanel(string id, Vector2 size)
        => ImGui.BeginChild(id, size, ImGuiChildFlags.Borders, ImGuiWindowFlags.None);

    public static void EndPanel() => ImGui.EndChild();

    public static void SectionHeader(string title) => ImGui.SeparatorText(title);

    public static void Spacer(float lines = 1.0f)
    {
        for (int i = 0; i < lines; i++) ImGui.Spacing();
    }

    public static void Separator() => ImGui.Separator();

    public static void LabelValue(string label, string value) => ImGui.Text($"{label} : {value}");

    public static void LabelValue(string label, double value, string unit = "")
    {
        if (string.IsNullOrWhiteSpace(unit)) ImGui.Text($"{label} : {value:F2}");
        else ImGui.Text($"{label} : {value:F2} {unit}");
    }

    public static void StatusMessage(StatusType type, string message)
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

    public static void CenteredText(string text)
    {
        float windowWidth = ImGui.GetWindowSize().X;
        float textWidth = ImGui.CalcTextSize(text).X;

        ImGui.SetCursorPosX((windowWidth - textWidth) * 0.5f);
        ImGui.Text(text);
    }

    public static bool Button(string text) => ImGui.Button(text, new Vector2(GuiConstants.ButtonWidth, GuiConstants.ButtonHeight));

    public static bool Button(string text, bool enabled)
    {
        ImGui.BeginDisabled(!enabled);
        bool clicked = ImGui.Button(text, new Vector2(GuiConstants.ButtonWidth, GuiConstants.ButtonHeight));
        ImGui.EndDisabled();
        return clicked;
    }

    public static bool InputDouble(string label, ref double value)
    {
        ImGui.PushItemWidth(GuiConstants.NumericInputWidth);
        bool changed = ImGui.InputDouble(label, ref value, 0.0, 0.0, "%.2f");
        ImGui.PopItemWidth();
        return changed;
    }

    public static void AlignRight()
    {
        float buttonWidth = GuiConstants.ButtonWidth;
        float rightEdge = ImGui.GetWindowWidth() - ImGui.GetStyle().WindowPadding.X;
        ImGui.SameLine();
        ImGui.SetCursorPosX(rightEdge - buttonWidth);
    }

    public static void AlignCenter()
    {
        float buttonWidth = GuiConstants.ButtonWidth;
        float availableWidth = ImGui.GetWindowWidth() - ImGui.GetStyle().WindowPadding.X * 2.0f;
        float centerX = (availableWidth - buttonWidth) * 0.5f;
        ImGui.SetCursorPosX(centerX);
    }

    public static void ResultMessage(bool pass)
    {
        Vector4 colour = pass ? new Vector4(0.30f, 0.85f, 0.30f, 1.0f) : new Vector4(1.00f, 0.35f, 0.35f, 1.0f);
        string message = pass ? "OVERALL RESULT: PASS" : "OVERALL RESULT: FAIL";
        ImGui.TextColored(colour, message);
    }

    public static void SafetyFactorIndicator(double safetyFactor)
    {
        Vector4 colour;
        if (safetyFactor < GuiConstants.RESULT_FS_RED_LIMIT) colour = new Vector4(1.00f, 0.35f, 0.35f, 1.0f);
        else if (safetyFactor < GuiConstants.RESULT_FS_YELLOW_LIMIT) colour = new Vector4(1.00f, 0.80f, 0.20f, 1.0f);
        else colour = new Vector4(0.30f, 0.85f, 0.30f, 1.0f);
        ImGui.TextColored(colour, "o");
    }
}
