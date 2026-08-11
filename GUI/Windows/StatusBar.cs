namespace LiftLugCalc2.GUI.Windows;

public static class StatusBar
{
    public static void Render(GuiController controller)
    {
        Session session = controller.CurrentSession;
        GuiCommon.StatusMessage(session.StatusType, session.StatusMessage);
    }
}
