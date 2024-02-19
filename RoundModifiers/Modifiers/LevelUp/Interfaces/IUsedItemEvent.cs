using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces
{
    public interface IUsedItemEvent
    {
        void OnUsedItem(UsedItemEventArgs ev);
    }
}