using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;
using TTCore.HUDs;

namespace RoundModifiers.Modifiers.WeaponStats;

public class InstantReloadStat : Stat, IReloading
{
    

    public void OnReloading(ReloadingWeaponEventArgs ev)
    {
        ev.IsAllowed = false;
        AmmoType ammoType = ev.Firearm.AmmoType;
        ushort playerAmmo = ev.Player.GetAmmo(ammoType);
        byte weaponAmmo = ev.Firearm.Ammo;
        byte maxAmmo = ev.Firearm.MaxAmmo;
        
        if(playerAmmo > 0 && weaponAmmo < maxAmmo)
        {
            if (playerAmmo >= (maxAmmo - weaponAmmo))
            {
                ev.Player.AddAmmo(ammoType, (ushort)(maxAmmo - weaponAmmo));
                ev.Firearm.Ammo = maxAmmo;
            }
            else
            {
                ev.Firearm.Ammo += (byte)playerAmmo;
                ev.Player.AddAmmo(ammoType, playerAmmo);
            }
        }
        ev.Player.ShowHUDHint("Reloaded", 5f);
    }

    public override string Name => "Instant Reload";
    public override int Rarity => 4;
    public override string Description => "This weapon reloads instantly";
}