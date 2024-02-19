using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces
{
    public interface IHurtingEvent
    {
        void OnHurting(HurtingEventArgs ev);
    }
}