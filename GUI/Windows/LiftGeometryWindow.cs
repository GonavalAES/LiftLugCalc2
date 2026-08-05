using ImGuiNET;

using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.GUI.Windows;

public static class LiftGeometryWindow
{
    public static void Render(Session session)
    {
        Project project = session.CurrentProject!;

        ImGui.Begin("Lift Geometry");

        ImGui.SeparatorText("Lift Geometry");

        ImGui.Text("Number of lifting points");

        if (ImGui.RadioButton("1", project.NumberPoints == 1)) project.NumberPoints = 1;
        ImGui.SameLine();
        if (ImGui.RadioButton("2", project.NumberPoints == 2)) project.NumberPoints = 2;
        ImGui.SameLine();
        if (ImGui.RadioButton("3", project.NumberPoints == 3)) project.NumberPoints = 3;
        ImGui.SameLine();
        if (ImGui.RadioButton("4", project.NumberPoints == 4)) project.NumberPoints = 4;

        ImGui.Spacing();

        double a1 = project.A1;
        double a2 = project.A2;
        double b1 = project.B1;
        double b2 = project.B2;

        switch (project.NumberPoints)
        {
            case 1:

                ImGui.Text("Single point lift - no geometry adjustment needed.");

                project.A1 = 0.0;
                project.A2 = 0.0;
                project.B1 = 0.0;
                project.B2 = 0.0;

                break;

            case 2:

                if (ImGui.InputDouble("A1 [m]", ref a1)) project.A1 = a1;
                if (ImGui.InputDouble("A2 [m]", ref a2)) project.A2 = a2;

                project.B1 = 0.0;
                project.B2 = 0.0;

                break;

            case 3:

                if (ImGui.InputDouble("A1 [m]", ref a1)) project.A1 = a1;
                if (ImGui.InputDouble("A2 [m]", ref a2)) project.A2 = a2;
                if (ImGui.InputDouble("B1 [m]", ref b1)) project.B1 = b1;

                project.B2 = 0.0;

                break;

            case 4:

                if (ImGui.InputDouble("A1 [m]", ref a1)) project.A1 = a1;
                if (ImGui.InputDouble("A2 [m]", ref a2)) project.A2 = a2;
                if (ImGui.InputDouble("B1 [m]", ref b1)) project.B1 = b1;
                if (ImGui.InputDouble("B2 [m]", ref b2)) project.B2 = b2;

                break;
        }

        ImGui.Separator();

        if (ImGui.Button("Back"))
        {
            session.CurrentScreen = Screen.WeightDefinition;
        }

        ImGui.SameLine();

        if (ImGui.Button("Next"))
        {
            session.CurrentScreen = Screen.MaterialSelection;
        }

        ImGui.End();
    }
}
