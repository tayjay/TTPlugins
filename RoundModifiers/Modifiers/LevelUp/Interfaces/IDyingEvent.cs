using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces
{
    public interface IDyingEvent
    {
        void OnDying(DyingEventArgs ev);
    }
}