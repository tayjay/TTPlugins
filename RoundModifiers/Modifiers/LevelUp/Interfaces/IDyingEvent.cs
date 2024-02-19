using Exiled.Events.EventArgs.Player;

namespace TayTaySCPSL.Modifiers.LevelUp.Interfaces
{
    public interface IDyingEvent
    {
        void OnDying(DyingEventArgs ev);
    }
}