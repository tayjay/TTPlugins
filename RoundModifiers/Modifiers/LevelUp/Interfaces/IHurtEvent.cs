using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces
{
    public interface IHurtEvent
    {
        void OnHurt(HurtEventArgs ev);
    }
}