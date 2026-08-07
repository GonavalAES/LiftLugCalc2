using ImGuiNET;

namespace LiftLugCalc2.GUI.Windows.Newones;

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

                DrawDistance(ref session.A1, "A1 [m]");
                DrawDistance(ref session.A2, "A2 [m]");

                break;

            case 3:

                DrawDistance(ref session.A1, "A1 [m]");
                DrawDistance(ref session.A2, "A2 [m]");
                DrawDistance(ref session.B1, "B1 [m]");

                break;

            case 4:

                DrawDistance(ref session.A1, "A1 [m]");
                DrawDistance(ref session.A2, "A2 [m]");
                DrawDistance(ref session.B1, "B1 [m]");
                DrawDistance(ref session.B2, "B2 [m]");

                break;
        }
    }

    //------------------------------------------------------------
    // Helpers
    //------------------------------------------------------------

    private static void DrawDistance(
        ref double value,
        string label)
    {
        double temp = value;

        if (ImGui.InputDouble(label, ref temp))
            value = temp;
    }
}
