using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.WeaponStats.Interfaces;

public interface IReloading
{
    void OnReloading(ReloadingWeaponEventArgs ev);
}