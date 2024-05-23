using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.WeaponStats.Interfaces;

public interface IShooting
{
    void OnShooting(ShootingEventArgs ev);
    void OnShot(ShotEventArgs ev);
}