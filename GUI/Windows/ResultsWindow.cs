using ImGuiNET;

using LiftLugCalc2.Core.Models;
using LiftLugCalc2.GUI.Helpers;

namespace LiftLugCalc2.GUI.Windows;

public static class ResultsWindow
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;
        Project project = session.CurrentProject!;

        GuiCommon.SectionHeader("Calculation Results");

        GuiCommon.Spacer();

        //--------------------------------------------------------
        // No result
        //--------------------------------------------------------

        if (session.CurrentResult == null)
        {
            ImGui.Text("No suitable lug was found.");

            GuiCommon.Spacer();

            ImGui.TextWrapped(
                "The calculation did not produce a valid lug selection.");

            return;
        }

        CalculationResult result = session.CurrentResult;

        //--------------------------------------------------------
        // Calculation summary
        //--------------------------------------------------------

        ImGui.Text(
            $"Calculation : {result.CalculationType}");

        ImGui.Text(
            $"Project : {project.Name}");

        GuiCommon.Spacer();

        //--------------------------------------------------------
        // Material
        //--------------------------------------------------------

        if (project.SelectedMaterial != null)
        {
            ImGui.Text(
                $"Material : {project.SelectedMaterial.Designation}");
        }

        //--------------------------------------------------------
        // Lug
        //--------------------------------------------------------

        if (project.SelectedLug != null)
        {
            ImGui.Text(
                $"Lug : {LugPresentation.GetLugTypeName(project.SelectedLug.LugType)}");

            ImGui.Text(
                $"Lug ID : {project.SelectedLug.LugID}");

            ImGui.Text(
                $"Lug WLL : {project.SelectedLug.LugWLL:N0} kg");
        }

        //--------------------------------------------------------
        // Applied load
        //--------------------------------------------------------

        GuiCommon.Spacer();

        ImGui.Text(
            $"Applied Load : {result.AppliedLoad:N1} kg");

        GuiCommon.Spacer();

        ImGui.Separator();

        GuiCommon.Spacer();

        //--------------------------------------------------------
        // Engineering checks
        //--------------------------------------------------------

        ImGui.Text("Engineering Checks");

        GuiCommon.Spacer();

        if (ImGui.BeginTable(
            "ResultsTable",
            3,
            ImGuiTableFlags.Borders |
            ImGuiTableFlags.RowBg))
        {
            ImGui.TableSetupColumn("Check");
            ImGui.TableSetupColumn("Capacity");
            ImGui.TableSetupColumn("Safety Factor");

            ImGui.TableHeadersRow();

            //----------------------------------------------------
            // Tension
            //----------------------------------------------------

            ImGui.TableNextRow();

            ImGui.TableSetColumnIndex(0);
            ImGui.Text("Tension");

            ImGui.TableSetColumnIndex(1);
            ImGui.Text(
                $"{result.TensionCapacity:N1} kN");

            ImGui.TableSetColumnIndex(2);
            ImGui.Text(
                $"{result.FSTension:N1}");

            //----------------------------------------------------
            // Shear
            //----------------------------------------------------

            ImGui.TableNextRow();

            ImGui.TableSetColumnIndex(0);
            ImGui.Text("Shear");

            ImGui.TableSetColumnIndex(1);
            ImGui.Text(
                $"{result.ShearCapacity:N1} kN");

            ImGui.TableSetColumnIndex(2);
            ImGui.Text(
                $"{result.FSShear:N1}");

            //----------------------------------------------------
            // Bearing
            //----------------------------------------------------

            ImGui.TableNextRow();

            ImGui.TableSetColumnIndex(0);
            ImGui.Text("Bearing");

            ImGui.TableSetColumnIndex(1);
            ImGui.Text(
                $"{result.BearingCapacity:N1} kN");

            ImGui.TableSetColumnIndex(2);
            ImGui.Text(
                $"{result.FSBearing:N1}");

            //----------------------------------------------------
            // Tear-out
            //----------------------------------------------------

            ImGui.TableNextRow();

            ImGui.TableSetColumnIndex(0);
            ImGui.Text("Tear-out");

            ImGui.TableSetColumnIndex(1);
            ImGui.Text(
                $"{result.TearOutCapacity:N1} kN");

            ImGui.TableSetColumnIndex(2);
            ImGui.Text(
                $"{result.FSTearOut:N1}");

            //----------------------------------------------------
            // Weld
            //----------------------------------------------------

            ImGui.TableNextRow();

            ImGui.TableSetColumnIndex(0);
            ImGui.Text("Weld");

            ImGui.TableSetColumnIndex(1);
            ImGui.Text(
                $"{result.WeldCapacity:N1} kN");

            ImGui.TableSetColumnIndex(2);
            ImGui.Text(
                $"{result.FSWeld:N1}");

            ImGui.EndTable();
        }

        GuiCommon.Spacer();

        //--------------------------------------------------------
        // Overall checks
        //--------------------------------------------------------

        ImGui.Text(
            $"Minimum Safety Factor : {result.MinimumFS:N1}");

        ImGui.Text(
            $"Weld Geometry : {(result.WeldGeometryOK ? "OK" : "NOT OK")}");

        GuiCommon.Spacer();

        ImGui.Separator();

        GuiCommon.Spacer();

        //--------------------------------------------------------
        // Overall result
        //--------------------------------------------------------

        if (result.Pass)
        {
            ImGui.Text("OVERALL RESULT: PASS");
        }
        else
        {
            ImGui.Text("OVERALL RESULT: FAIL");
        }
    }
}
