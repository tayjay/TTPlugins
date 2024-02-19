using Exiled.Events.EventArgs.Player;

namespace TayTaySCPSL.Modifiers.LevelUp.Interfaces
{
    public interface IUsedItemEvent
    {
        void OnUsedItem(UsedItemEventArgs ev);
    }
}