using ImGuiNET;

using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.GUI.Windows.Newones;

public static class MaterialSelectionWindow
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;

        GuiCommon.SectionHeader("Material Selection");

        GuiCommon.Spacer();

        if (ImGui.BeginTable(
            "Materials",
            4,
            ImGuiTableFlags.RowBg |
            ImGuiTableFlags.Borders |
            ImGuiTableFlags.ScrollY))
        {
            ImGui.TableSetupColumn("ID");
            ImGui.TableSetupColumn("Designation");
            ImGui.TableSetupColumn("Fy");
            ImGui.TableSetupColumn("Fu");

            ImGui.TableHeadersRow();

            foreach (Material material in AppState.Materials)
            {
                ImGui.TableNextRow();

                //------------------------------------------------
                // ID
                //------------------------------------------------

                ImGui.TableSetColumnIndex(0);

                bool selected =
                    session.SelectedMaterial == material;

                if (ImGui.Selectable(
                    material.MaterialID.ToString(),
                    selected,
                    ImGuiSelectableFlags.SpanAllColumns))
                {
                    session.SelectedMaterial = material;
                }

                //------------------------------------------------
                // Designation
                //------------------------------------------------

                ImGui.TableSetColumnIndex(1);

                ImGui.Text(material.Designation);

                //------------------------------------------------
                // Yield
                //------------------------------------------------

                ImGui.TableSetColumnIndex(2);

                ImGui.Text(
                    material.YieldStrength.ToString("N0"));

                //------------------------------------------------
                // Tensile
                //------------------------------------------------

                ImGui.TableSetColumnIndex(3);

                ImGui.Text(
                    material.TensileStrength.ToString("N0"));
            }

            ImGui.EndTable();
        }

        GuiCommon.Spacer();

        //--------------------------------------------------------
        // Current selection
        //--------------------------------------------------------

        if (session.SelectedMaterial != null)
        {
            ImGui.Text(
                $"Selected: {session.SelectedMaterial.Designation}");
        }
        else
        {
            ImGui.TextDisabled("No material selected.");
        }
    }
}
