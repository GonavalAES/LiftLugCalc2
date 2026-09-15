using ImGuiNET;

using LiftLugCalc2.Core.Models;
using LiftLugCalc2.GUI.Enums;
using LiftLugCalc2.GUI.Helpers;

namespace LiftLugCalc2.GUI.Windows.Calculations;

public static class ExploratoryCalculationWindow
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;
        Project project = session.CurrentProject!;

        GuiCommon.SectionHeader("Exploratory Calculation");

        GuiCommon.Spacer();

        DrawProjectSummary(session);

        GuiCommon.Spacer();
        ImGui.Separator();
        GuiCommon.Spacer();

        DrawReferenceLugSelection(controller, session);

        GuiCommon.Spacer();
        ImGui.Separator();
        GuiCommon.Spacer();

        DrawReferenceLugGeometry(session);
    }

    private static void DrawProjectSummary(Session session)
    {
        Project project = session.CurrentProject!;

        GuiCommon.SectionHeader("Project");
        GuiCommon.LabelValue("Project", project.Name);
        GuiCommon.LabelValue("Material", project.SelectedMaterial!.Designation);
        GuiCommon.LabelValue("Working load", project.WLL, "kg");
    }

    private static void DrawReferenceLugSelection(GuiController controller, Session session)
    {
        GuiCommon.SectionHeader("Reference Lug");

        ImGui.TextWrapped("Select a catalogue lug as the starting geometry. The reference lug will not be changed. Its values " +
                          "will be copied into the exploratory editor.");

        GuiCommon.Spacer();

        if (AppState.Lugs.Count == 0)
        {
            GuiCommon.StatusMessage(StatusType.Error, "No lugs are available in the reference table.");
            return;
        }

        int selectedIndex = GetReferenceLugIndex(session);
        string[] lugNames = AppState.Lugs.Select(CreateReferenceLugName).ToArray();
        ImGui.SetNextItemWidth(400.0f);

        if (ImGui.Combo("Catalogue lug", ref selectedIndex, lugNames, lugNames.Length))
        {
            TableLug selectedLug = AppState.Lugs[selectedIndex];
            controller.SelectExploratoryReferenceLug(selectedLug);
        }
    }

    private static int GetReferenceLugIndex(Session session)
    {
        if (session.ExploratoryReferenceLug is null) return 0;

        for (int index = 0; index < AppState.Lugs.Count; index++)
            if (AppState.Lugs[index].LugID == session.ExploratoryReferenceLug.LugID) return index;

        return 0;
    }

    private static string CreateReferenceLugName(TableLug lug)
    {
        string lugType = LugPresentation.GetLugTypeName(lug.LugType);

        return $"ID {lug.LugID} - {lugType} - {lug.LugWLL:N0} kg";
    }

    private static void DrawReferenceLugGeometry(Session session)
    {
        if (session.ExploratoryReferenceLug is null)
        {
            GuiCommon.SectionHeader("Reference Geometry");
            ImGui.TextWrapped("Select a reference lug to display its geometry.");
            return;
        }

        ExploratoryLugEditor editor = session.ExploratoryLugEditor;

        GuiCommon.SectionHeader("Reference Geometry");

        GuiCommon.LabelValue("Lug type", LugPresentation.GetLugTypeName(editor.LugType));
        GuiCommon.LabelValue("Catalogue WLL", editor.LugWLL, "kg");

        GuiCommon.Spacer();

        GuiCommon.LabelValue("Plate thickness", editor.ThicknessPlate, "mm");
        GuiCommon.LabelValue("Hole diameter", editor.DiameterHole, "mm");
        GuiCommon.LabelValue("Lug radius", editor.RadiusLug, "mm");
        GuiCommon.LabelValue("Hole centre height", editor.HeightCenterHole, "mm");
        GuiCommon.LabelValue("Lug length", editor.LengthLug, "mm");
        GuiCommon.LabelValue("Toe height", editor.HeightToe, "mm");

        if (editor.LugType is 2 or 3)
        {
            GuiCommon.Spacer();

            GuiCommon.LabelValue("Cheek/boss radius", editor.RadiusCheekBoss, "mm");
            GuiCommon.LabelValue("Cheek/boss thickness", editor.ThicknessCheekBoss, "mm");
            GuiCommon.LabelValue("Cheek/boss weld throat", editor.WeldThroatCheek, "mm");
        }

        if (editor.LugType is 1 or 2 or 3)
        {
            GuiCommon.Spacer();

            GuiCommon.LabelValue("Main weld throat", editor.LugWeldThroat, "mm");
        }
    }
}
