using ImGuiNET;

using System.Numerics;

namespace LiftLugCalc2.GUI;

public static class GuiCommon
{
    // Color Constants (Vector4 RGBA)
    public static readonly Vector4 PassColor = new(0.2f, 0.85f, 0.3f, 1.0f);
    public static readonly Vector4 FailColor = new(0.9f, 0.25f, 0.25f, 1.0f);
    public static readonly Vector4 SubtextColor = new(0.6f, 0.6f, 0.6f, 1.0f);

    public static void Header(string text)
    {
        ImGui.SeparatorText(text);
    }

    public static void StatusBanner(bool isPass, string text)
    {
        Vector4 color = isPass ? PassColor : FailColor;
        ImGui.TextColored(color, text);
    }
}
