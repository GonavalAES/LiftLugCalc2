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

        DrawExploratoryGeometryEditor(session, controller);
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

    private static void DrawExploratoryGeometryEditor(Session session, GuiController controller)
    {
        if (session.ExploratoryReferenceLug is null)
        {
            GuiCommon.SectionHeader("Custom Lug Geometry");
            ImGui.TextWrapped("Select a reference lug to begin editing the custom lug geometry.");
            return;
        }

        TableLug referenceLug = session.ExploratoryReferenceLug;

        ExploratoryLugEditor editor = session.ExploratoryLugEditor;

        GuiCommon.SectionHeader("Custom Lug Geometry");
        ImGui.TextWrapped("Select one or more properties to modify. Unticked properties remain equal to the selected reference lug.");

        GuiCommon.Spacer();
        DrawLugIdentification(referenceLug, editor);

        GuiCommon.Spacer();
        DrawMainGeometryTable(controller, referenceLug, editor);

        if (editor.LugType is 2 or 3)
        {
            GuiCommon.Spacer();
            DrawCheekBossGeometryTable(controller, referenceLug, editor);
        }

        if (editor.LugType is 1 or 2 or 3)
        {
            GuiCommon.Spacer();
            DrawWeldGeometryTable(controller, referenceLug, editor);
        }

        GuiCommon.Spacer();

        DrawValidationResult(session);
    }

    private static void DrawValidationResult(Session session)
    {
        ExploratoryValidationResult? result = session.ExploratoryValidationResult;

        if (result is null) return;

        GuiCommon.SectionHeader("Geometry Validation");

        if (result.IsValid) GuiCommon.StatusMessage(StatusType.Information, "Geometry is valid for calculation.");
        else GuiCommon.StatusMessage(StatusType.Error, "Geometry contains errors. Correct them before calculation.");

        foreach (string error in result.Errors) GuiCommon.StatusMessage(StatusType.Error, error);
        foreach (string warning in result.Warnings) GuiCommon.StatusMessage(StatusType.Warning, warning);
    }

    private static void DrawLugIdentification(TableLug referenceLug, ExploratoryLugEditor editor)
    {
        GuiCommon.SectionHeader("Reference");
        GuiCommon.LabelValue("Reference lug", $"ID {referenceLug.LugID}");
        GuiCommon.LabelValue("Lug type", LugPresentation.GetLugTypeName(editor.LugType));
        GuiCommon.LabelValue("Reference WLL", referenceLug.LugWLL, "kg");
    }

    private static void DrawMainGeometryTable(GuiController controller, TableLug referenceLug, ExploratoryLugEditor editor)
    {
        GuiCommon.SectionHeader("Main Lug Geometry");

        if (!ImGui.BeginTable("MainLugGeometryTable", 4, ImGuiTableFlags.Borders |
                              ImGuiTableFlags.RowBg | ImGuiTableFlags.SizingFixedFit))
            return;

        ImGui.TableSetupColumn("Modify");
        ImGui.TableSetupColumn("Property");
        ImGui.TableSetupColumn("Reference");
        ImGui.TableSetupColumn("Custom value");
        ImGui.TableHeadersRow();

        bool hasChanged = false;

        hasChanged |= DrawEditableDoubleRow("PlateThickness", "Plate thickness [mm]", referenceLug.ThicknessPlate, editor.ChangeThicknessPlate,
                              editor.ThicknessPlate, out bool changeThicknessPlate, out double thicknessPlate);

        editor.ChangeThicknessPlate = changeThicknessPlate;
        editor.ThicknessPlate = thicknessPlate;

        hasChanged |= DrawEditableDoubleRow("HoleDiameter", "Hole diameter [mm]", referenceLug.DiameterHole, editor.ChangeDiameterHole,
                              editor.DiameterHole, out bool changeDiameterHole, out double diameterHole);

        editor.ChangeDiameterHole = changeDiameterHole;
        editor.DiameterHole = diameterHole;

        hasChanged |= DrawEditableDoubleRow("LugRadius", "Lug radius [mm]", referenceLug.RadiusLug, editor.ChangeRadiusLug,
                              editor.RadiusLug, out bool changeRadiusLug, out double radiusLug);

        editor.ChangeRadiusLug = changeRadiusLug;
        editor.RadiusLug = radiusLug;

        hasChanged |= DrawEditableDoubleRow("HoleCentreHeight", "Hole centre height [mm]", referenceLug.HeightCenterHole,
                              editor.ChangeHeightCenterHole, editor.HeightCenterHole, out bool changeHeightCenterHole,
                              out double heightCenterHole);

        editor.ChangeHeightCenterHole = changeHeightCenterHole;
        editor.HeightCenterHole = heightCenterHole;

        hasChanged |= DrawEditableDoubleRow("LugLength", "Lug length [mm]", referenceLug.LengthLug, editor.ChangeLengthLug,
                              editor.LengthLug, out bool changeLengthLug, out double lengthLug);

        editor.ChangeLengthLug = changeLengthLug;
        editor.LengthLug = lengthLug;

        hasChanged |= DrawEditableDoubleRow("ToeHeight", "Toe height [mm]", referenceLug.HeightToe, editor.ChangeHeightToe,
                              editor.HeightToe, out bool changeHeightToe, out double heightToe);

        editor.ChangeHeightToe = changeHeightToe;
        editor.HeightToe = heightToe;

        if (hasChanged) controller.ClearExploratoryValidation();

        ImGui.EndTable();
    }

    private static void DrawCheekBossGeometryTable(GuiController controller, TableLug referenceLug, ExploratoryLugEditor editor)
    {
        GuiCommon.SectionHeader("Cheek / Boss Geometry");

        if (!ImGui.BeginTable("CheekBossGeometryTable", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg |
                              ImGuiTableFlags.SizingFixedFit))
            return;

        ImGui.TableSetupColumn("Modify");
        ImGui.TableSetupColumn("Property");
        ImGui.TableSetupColumn("Reference");
        ImGui.TableSetupColumn("Custom value");
        ImGui.TableHeadersRow();

        bool hasChanged = false;

        string radiusName = editor.LugType == 2 ? "Cheek radius [mm]" : "Boss radius [mm]";
        string thicknessName = editor.LugType == 2 ? "Cheek thickness [mm]" : "Boss thickness [mm]";

        hasChanged |= DrawEditableDoubleRow("CheekBossRadius", radiusName, referenceLug.RadiusCheek_Boss, editor.ChangeRadiusCheekBoss,
                              editor.RadiusCheekBoss, out bool changeRadiusCheekBoss, out double radiusCheekBoss);

        editor.ChangeRadiusCheekBoss = changeRadiusCheekBoss;
        editor.RadiusCheekBoss = radiusCheekBoss;

        hasChanged |= DrawEditableDoubleRow("CheekBossThickness", thicknessName, referenceLug.ThicknessCheek_Boss,
                              editor.ChangeThicknessCheekBoss, editor.ThicknessCheekBoss, out bool changeThicknessCheekBoss,
                              out double thicknessCheekBoss);

        editor.ChangeThicknessCheekBoss = changeThicknessCheekBoss;
        editor.ThicknessCheekBoss = thicknessCheekBoss;

        if (hasChanged) controller.ClearExploratoryValidation();

        ImGui.EndTable();
    }

    private static void DrawWeldGeometryTable(GuiController controller, TableLug referenceLug, ExploratoryLugEditor editor)
    {
        GuiCommon.SectionHeader("Weld Geometry");

        if (!ImGui.BeginTable("WeldGeometryTable", 4, ImGuiTableFlags.Borders | ImGuiTableFlags.RowBg |
                              ImGuiTableFlags.SizingFixedFit))
            return;

        ImGui.TableSetupColumn("Modify");
        ImGui.TableSetupColumn("Property");
        ImGui.TableSetupColumn("Reference");
        ImGui.TableSetupColumn("Custom value");
        ImGui.TableHeadersRow();

        bool hasChanged = false;

        hasChanged |= DrawEditableDoubleRow("MainWeldThroat", "Main weld throat [mm]", referenceLug.LugWeldThroat, editor.ChangeLugWeldThroat,
                              editor.LugWeldThroat, out bool changeLugWeldThroat, out double lugWeldThroat);

        editor.ChangeLugWeldThroat = changeLugWeldThroat;
        editor.LugWeldThroat = lugWeldThroat;

        if (editor.LugType is 2 or 3)
        {
            string weldName = editor.LugType == 2 ? "Cheek weld throat [mm]" : "Boss weld throat [mm]";

            hasChanged |= DrawEditableDoubleRow("CheekBossWeldThroat", weldName, referenceLug.WeldThroatCheek, editor.ChangeWeldThroatCheek,
                                  editor.WeldThroatCheek, out bool changeWeldThroatCheek, out double weldThroatCheek);

            editor.ChangeWeldThroatCheek = changeWeldThroatCheek;
            editor.WeldThroatCheek = weldThroatCheek;
        }

        if (hasChanged) controller.ClearExploratoryValidation();

        ImGui.EndTable();
    }

    private static bool DrawEditableDoubleRow(string id, string propertyName, double referenceValue, bool changeSelected,
                                              double customValue, out bool updatedChangeSelected, out double updatedCustomValue)
    {
        updatedChangeSelected = changeSelected;
        updatedCustomValue = customValue;

        ImGui.TableNextRow();
        ImGui.TableSetColumnIndex(0);

        bool checkboxChanged = ImGui.Checkbox($"##Change{id}", ref updatedChangeSelected);

        ImGui.TableSetColumnIndex(1);
        ImGui.Text(propertyName);
        ImGui.TableSetColumnIndex(2);
        ImGui.Text($"{referenceValue:F2}");
        ImGui.TableSetColumnIndex(3);

        bool valueChanged = GuiCommon.InputDouble($"##Custom{id}", ref updatedCustomValue, updatedChangeSelected);

        return checkboxChanged || valueChanged;
    }


}
