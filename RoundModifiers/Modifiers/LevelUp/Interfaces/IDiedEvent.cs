using Exiled.Events.EventArgs.Player;

namespace TayTaySCPSL.Modifiers.LevelUp.Interfaces
{
    public interface IDiedEvent
    {
        void OnDied(DiedEventArgs ev);
    }
}