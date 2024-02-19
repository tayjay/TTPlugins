using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces
{
    public interface IHandcuffingEvent
    {
        void OnHandcuffing(HandcuffingEventArgs ev);
    }
}