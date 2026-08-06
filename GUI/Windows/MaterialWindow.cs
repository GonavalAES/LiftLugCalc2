using ImGuiNET;

using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.GUI.Windows;

public static class MaterialWindow
{
    public static void Render(Session session)
    {
        ImGui.Begin("Material Selection");

        ImGui.SeparatorText("Select Material");

        if (ImGui.BeginTable(
            "Materials",
            4,
            ImGuiTableFlags.Borders |
            ImGuiTableFlags.RowBg |
            ImGuiTableFlags.ScrollY |
            ImGuiTableFlags.Resizable))
        {
            ImGui.TableSetupColumn("ID", ImGuiTableColumnFlags.WidthFixed, 50.0f);
            ImGui.TableSetupColumn("Material", ImGuiTableColumnFlags.WidthStretch);
            ImGui.TableSetupColumn("Fy [MPa]", ImGuiTableColumnFlags.WidthFixed, 50.0f);
            ImGui.TableSetupColumn("Fu [MPa]", ImGuiTableColumnFlags.WidthFixed, 50.0f);

            ImGui.TableHeadersRow();

            for (int i = 0; i < AppState.Materials.Count; i++)
            {
                Material material = AppState.Materials[i];

                ImGui.TableNextRow();

                ImGui.TableNextColumn();

                bool selected =
                    session.SelectedMaterial?.MaterialID == material.MaterialID;

                if (ImGui.Selectable(
                    material.MaterialID.ToString(),
                    selected,
                    ImGuiSelectableFlags.SpanAllColumns))
                {
                    session.SelectedMaterial = material;
                }

                ImGui.TableNextColumn();
                ImGui.Text(material.Designation);

                ImGui.TableNextColumn();
                ImGui.Text(material.YieldStrength.ToString("F0"));

                ImGui.TableNextColumn();
                ImGui.Text(material.TensileStrength.ToString("F0"));
            }

            ImGui.EndTable();
        }

        ImGui.Separator();

        if (session.SelectedMaterial != null)
        {
            ImGui.Text($"Selected Material: {session.SelectedMaterial.Designation}");
        }

        ImGui.Spacing();

        if (ImGui.Button("Back"))
        {
            session.CurrentScreen = Screen.CalculationMode;
        }

        ImGui.SameLine();

        bool canContinue = session.SelectedMaterial != null;

        if (!canContinue)
            ImGui.BeginDisabled();

        if (ImGui.Button("Next"))
        {
            session.CurrentProject!.SelectedMaterial = session.SelectedMaterial;

            if (session.CalculationMode == CalculationMode.Forward)
            {
                session.CurrentScreen = Screen.LugGeometry;
            }
            else
            {
                session.CurrentScreen = Screen.ReverseCalculation;
            }
        }

        if (!canContinue)
            ImGui.EndDisabled();

        ImGui.End();
    }
}
