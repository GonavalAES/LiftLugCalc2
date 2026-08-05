using ImGuiNET;

using LiftLugCalc2.Core.Engine;
using LiftLugCalc2.Core.Models;

namespace LiftLugCalc2.GUI.Windows;

public static class ReverseCalculationWindow
{
    private static bool _calculated;

    public static void Render(Session session)
    {
        ImGui.Begin("Reverse Calculation");

        if (!_calculated)
        {
            ImGui.Text("Searching suitable lifting lug...");

            ReverseInput input = new(
                session.CurrentProject!,
                session.SelectedMaterial!,
                AppState.Lugs);

            ReverseSelection selection =
                ReverseCalculator.Run(
                    input,
                    "2");

            if (selection.Best != null)
            {
                session.SelectedLug =
                    selection.Best.Lug;

                session.CurrentProject!.SelectedLug =
                    selection.Best.Lug;

                session.Result =
                    selection.Best.Result;
            }

            _calculated = true;

            session.StatusMessage =
                "Reverse calculation completed.";

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
