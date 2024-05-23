using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.WeaponStats.Interfaces;

public interface IHurting
{
    void OnOwnerHurting(HurtingEventArgs ev);
    void OnTargetHurting(HurtingEventArgs ev);
    
    void OnOwnerHurt(HurtEventArgs ev);
    void OnTargetHurt(HurtEventArgs ev);
}