using ImGuiNET;

using LiftLugCalc2.Core.Models;
using LiftLugCalc2.GUI.Helpers;

namespace LiftLugCalc2.GUI.Windows.Selections;

public static class LugSelectionWindow
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;

        GuiCommon.SectionHeader("Lug Selection");

        GuiCommon.Spacer();

        if (ImGui.BeginTable(
            "Lugs",
            3,
            ImGuiTableFlags.RowBg |
            ImGuiTableFlags.Borders |
            ImGuiTableFlags.ScrollY))
        {
            ImGui.TableSetupColumn("ID");
            ImGui.TableSetupColumn("Type");
            ImGui.TableSetupColumn("WLL");

            ImGui.TableHeadersRow();

            foreach (TableLug lug in AppState.Lugs)
            {
                ImGui.TableNextRow();

                ImGui.TableSetColumnIndex(0);

                bool selected =
                    session.SelectedLug == lug;

                if (ImGui.Selectable(
                    lug.LugID.ToString(),
                    selected,
                    ImGuiSelectableFlags.SpanAllColumns))
                {
                    session.SelectedLug = lug;
                }

                ImGui.TableSetColumnIndex(1);
                ImGui.Text(
                    LugPresentation.GetLugTypeName(
                        lug.LugType));

                ImGui.TableSetColumnIndex(2);
                ImGui.Text($"{lug.LugWLL:N0}");
            }

            ImGui.EndTable();
        }

        GuiCommon.Spacer();

        if (session.SelectedLug != null)
        {
            GuiCommon.SectionHeader("Selected Lug");

            ImGui.Text(
                $"Type : {LugPresentation.GetLugTypeName(session.SelectedLug.LugType)}");

            ImGui.Text(
                $"WLL : {session.SelectedLug.LugWLL:N0} kg");

            ImGui.Text(
                $"Plate Thickness : {session.SelectedLug.ThicknessPlate:N1} mm");

            ImGui.Text(
                $"Hole Diameter : {session.SelectedLug.DiameterHole:N1} mm");

            ImGui.Text(
                $"Length : {session.SelectedLug.LengthLug:N1} mm");
        }
    }
}
