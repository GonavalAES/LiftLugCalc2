using LiftLugCalc2.GUI.Enums;
namespace LiftLugCalc2.GUI.Helpers;

public static class SessionValidation
{
    public static bool HasProject(Session session)
    {
        if (session.CurrentProject is not null) return true;

        session.StatusType = StatusType.Error;
        session.StatusMessage = "No active project.";
        session.CurrentScreen = ScreenEnum.MainMenu;

        return false;
    }
}
