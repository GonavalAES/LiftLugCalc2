using ImGuiNET;

namespace LiftLugCalc2.GUI.Windows.Newones;

public static class CommandBar
{
    /// <summary>
    /// Draws the application's command bar.
    /// </summary>
    public static void Render(GuiController controller)
    {

        switch (controller.CurrentScreen)
        {
            case Screen.NewProject:

                {
                    if (GuiCommon.Button("Back"))
                    {
                        controller.CurrentScreen = Screen.MainMenu;
                    }

                    ImGui.SameLine();

                    if (GuiCommon.Button("Next"))
                    {
                        controller.CreateProject();
                    }

                    break;
                }
            case Screen.WeightDefinition:

                {
                    if (GuiCommon.Button("Back"))
                    {
                        controller.CurrentScreen = Screen.NewProject;
                    }

                    ImGui.SameLine();

                    if (GuiCommon.Button("Next"))
                    {
                        controller.AcceptWeightDefinition();
                    }
                    break;
                }

            case Screen.LiftGeometry:

                {
                    if (GuiCommon.Button("Back"))
                    {
                        controller.CurrentScreen = Screen.WeightDefinition;
                    }

                    ImGui.SameLine();

                    if (GuiCommon.Button("Next"))
                    {
                        controller.AcceptLiftGeometry();
                    }

                    break;
                }

            case Screen.CalculationMode:

                {
                    if (GuiCommon.Button("Back"))
                    {
                        controller.CurrentScreen = Screen.LiftGeometry;
                    }

                    ImGui.SameLine();

                    if (GuiCommon.Button("Next"))
                    {
                        controller.AcceptCalculationMode();
                    }

                    break;
                }

            case Screen.MaterialSelection:

                {
                    if (GuiCommon.Button("Back"))
                    {
                        controller.CurrentScreen = Screen.CalculationMode;
                    }

                    ImGui.SameLine();

                    bool enabled =
                        controller.CurrentSession.SelectedMaterial != null;

                    ImGui.BeginDisabled(!enabled);

                    if (GuiCommon.Button("Next"))
                    {
                        controller.AcceptMaterialSelection();
                    }

                    ImGui.EndDisabled();

                    break;
                }

            case Screen.LugGeometry:

                {
                    if (GuiCommon.Button("Back"))
                    {
                        controller.CurrentScreen =
                            Screen.MaterialSelection;
                    }

                    ImGui.SameLine();

                    bool enabled =
                        controller.CurrentSession.SelectedLug != null;

                    ImGui.BeginDisabled(!enabled);

                    if (GuiCommon.Button("Next"))
                    {
                        controller.AcceptLugSelection();
                    }

                    ImGui.EndDisabled();

                    break;
                }


            default:

                ImGui.Text("Commands");
                break;
        }
    }
}
