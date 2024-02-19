using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces
{
    public interface IDiedEvent
    {
        void OnDied(DiedEventArgs ev);
    }
}