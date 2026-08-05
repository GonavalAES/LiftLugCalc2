using ImGuiNET;

using LiftLugCalc2.Core.Engine;
using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.GUI.Windows;

public static class ForwardCalculationWindow
{
    private static bool _calculated;

    public static void Render(Session session)
    {
        ImGui.Begin("Forward Calculation");

        if (!_calculated)
        {
            ImGui.Text("Performing engineering calculations...");

            ForwardInput input = new(
                session.CurrentProject!,
                session.SelectedLug!,
                session.SelectedMaterial!);

            session.Result =
                ForwardCalculator.Run(
                    input,
                    "1");

            _calculated = true;

            session.StatusMessage =
                "Forward calculation completed.";

            session.StatusType =
                StatusType.Information;
        }
        else
        {
            ImGui.Text("Calculation completed.");

            if (ImGui.Button("Continue"))
            {
                _calculated = false;

                session.CurrentScreen = Screen.Results;
            }
        }

        ImGui.End();
    }
}
