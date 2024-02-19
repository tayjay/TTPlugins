using Exiled.Events.EventArgs.Player;

namespace TayTaySCPSL.Modifiers.LevelUp.Interfaces
{
    public interface IHurtEvent
    {
        void OnHurt(HurtEventArgs ev);
    }
}