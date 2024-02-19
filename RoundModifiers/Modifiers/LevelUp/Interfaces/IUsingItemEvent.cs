using Exiled.Events.EventArgs.Player;

namespace TayTaySCPSL.Modifiers.LevelUp.Interfaces
{
    public interface IUsingItemEvent
    {
        void OnUsingItem(UsingItemEventArgs ev);
    }
}