using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.WeaponStats.Interfaces;

public interface IDying
{
    void OnOwnerDying(DyingEventArgs ev);
    void OnTargetDying(DyingEventArgs ev);
    
    void OnOwnerDied(DiedEventArgs ev);
    void OnTargetDied(DiedEventArgs ev);
}