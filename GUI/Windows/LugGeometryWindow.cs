using ImGuiNET;

using LiftLugCalc2.Core.Models;
using LiftLugCalc2.GUI.Helpers;

namespace LiftLugCalc2.GUI.Windows;

public static class LugGeometryWindow
{
    public static void Render(Session session)
    {
        ImGui.Begin("Lug Selection");

        ImGui.SeparatorText("Select Lifting Lug");

        if (ImGui.BeginTable(
            "Lugs",
            3,
            ImGuiTableFlags.Borders |
            ImGuiTableFlags.RowBg |
            ImGuiTableFlags.ScrollY |
            ImGuiTableFlags.Resizable))
        {
            ImGui.TableSetupColumn("ID", ImGuiTableColumnFlags.WidthFixed, 50.0f);
            ImGui.TableSetupColumn("Lug Type", ImGuiTableColumnFlags.WidthStretch);
            ImGui.TableSetupColumn("WLL [kg]", ImGuiTableColumnFlags.WidthFixed, 90.0f);

            ImGui.TableHeadersRow();

            foreach (TableLug lug in AppState.Lugs)
            {
                ImGui.TableNextRow();

                ImGui.TableNextColumn();

                bool selected =
                    session.SelectedLug?.LugID == lug.LugID;

                if (ImGui.Selectable(
                    lug.LugID.ToString(),
                    selected,
                    ImGuiSelectableFlags.SpanAllColumns))
                {
                    session.SelectedLug = lug;
                }

                ImGui.TableNextColumn();
                ImGui.Text(LugPresentation.GetLugTypeName(lug.LugType));

                ImGui.TableNextColumn();
                ImGui.Text($"{lug.LugWLL:F1}");
            }

            ImGui.EndTable();
        }

        ImGui.Separator();

        if (session.SelectedLug != null)
        {
            ImGui.Text($"Selected Lug : {session.SelectedLug.LugType}");
            ImGui.Text($"WLL          : {session.SelectedLug.LugWLL:F1} kg");

            ImGui.Spacing();
            ImGui.SeparatorText("Lug Preview");

            // Future replacement for UICommon.DrawLugType().
            ImGui.Text($"Sketch for lug type '{session.SelectedLug.LugType}' will appear here.");
        }

        ImGui.Spacing();
        ImGui.Separator();

        if (ImGui.Button("Back"))
        {
            session.CurrentScreen = Screen.MaterialSelection;
        }

        ImGui.SameLine();

        bool canContinue = session.SelectedLug != null;

        if (!canContinue)
            ImGui.BeginDisabled();

        if (ImGui.Button("Calculate"))
        {
            session.CurrentProject!.SelectedMaterial = session.SelectedMaterial;
            session.CurrentProject.SelectedLug = session.SelectedLug;

            // Calculation stage comes next.
            session.CurrentScreen = Screen.ForwardCalculation;
        }

        if (!canContinue)
            ImGui.EndDisabled();

        ImGui.End();
    }
}
