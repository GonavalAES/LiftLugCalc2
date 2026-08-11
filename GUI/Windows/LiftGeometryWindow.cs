using ImGuiNET;

namespace LiftLugCalc2.GUI.Windows;

public static class LiftGeometryWindow
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;

        GuiCommon.SectionHeader("Lift Geometry");

        GuiCommon.Spacer();

        //--------------------------------------------------------
        // Number of lifting points
        //--------------------------------------------------------

        ImGui.Text("Number of lifting points");

        int numberPoints = session.NumberPoints;

        ImGui.RadioButton("1", ref numberPoints, 1);
        ImGui.SameLine();

        ImGui.RadioButton("2", ref numberPoints, 2);
        ImGui.SameLine();

        ImGui.RadioButton("3", ref numberPoints, 3);
        ImGui.SameLine();

        ImGui.RadioButton("4", ref numberPoints, 4);

        session.NumberPoints = numberPoints;

        GuiCommon.Spacer();

        //--------------------------------------------------------
        // Geometry
        //--------------------------------------------------------

        switch (numberPoints)
        {
            case 1:

                ImGui.Text("Single lifting point.");
                ImGui.Text("No geometry input required.");

                break;

            case 2:
                DrawDistance(
                    "A1 [m]",
                    session.A1,
                    value => session.A1 = value);

                DrawDistance(
                    "A2 [m]",
                    session.A2,
                    value => session.A2 = value);
                break;

            case 3:
                DrawDistance(
                    "A1 [m]",
                    session.A1,
                    value => session.A1 = value);

                DrawDistance(
                    "A2 [m]",
                    session.A2,
                    value => session.A2 = value);

                DrawDistance(
                    "B1 [m]",
                    session.B1,
                    value => session.B1 = value);
                break;

            case 4:
                DrawDistance(
                    "A1 [m]",
                    session.A1,
                    value => session.A1 = value);

                DrawDistance(
                    "A2 [m]",
                    session.A2,
                    value => session.A2 = value);

                DrawDistance(
                    "B1 [m]",
                    session.B1,
                    value => session.B1 = value);

                DrawDistance(
                    "B2 [m]",
                    session.B2,
                    value => session.B2 = value);
                break;
        }
    }

    //------------------------------------------------------------
    // Helpers
    //------------------------------------------------------------

    private static void DrawDistance(
    string label,
    double value,
    Action<double> setter)
    {
        double distance = value;

        if (GuiCommon.InputDouble(
                label,
                ref distance))
        {
            setter(distance);
        }
    }
}
