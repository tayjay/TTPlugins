using Exiled.Events.EventArgs.Player;

namespace TayTaySCPSL.Modifiers.LevelUp.Interfaces
{
    public interface IHurtingEvent
    {
        void OnHurting(HurtingEventArgs ev);
    }
}