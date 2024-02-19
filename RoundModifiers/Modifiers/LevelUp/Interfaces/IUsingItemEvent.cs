using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces
{
    public interface IUsingItemEvent
    {
        void OnUsingItem(UsingItemEventArgs ev);
    }
}