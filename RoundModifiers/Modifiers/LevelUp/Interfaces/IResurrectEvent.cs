using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces;

public interface IResurrectEvent
{
    void OnResurrect(ChangingRoleEventArgs ev);
}