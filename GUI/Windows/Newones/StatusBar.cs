namespace LiftLugCalc2.GUI.Windows.Newones;

public static class StatusBar
{
    /// <summary>
    /// Draws the application status bar.
    /// </summary>
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;

        GuiCommon.StatusMessage(
            session.StatusType,
            session.StatusMessage);
    }
}
