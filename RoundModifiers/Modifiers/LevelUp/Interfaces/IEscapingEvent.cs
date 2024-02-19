using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces
{
    public interface IEscapingEvent
    {
        void OnEscaping(EscapingEventArgs ev);
    }
}