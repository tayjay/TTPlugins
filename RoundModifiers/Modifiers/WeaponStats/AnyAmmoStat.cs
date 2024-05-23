using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;

namespace RoundModifiers.Modifiers.WeaponStats;

public class AnyAmmoStat : Stat, IReloading
{

    public void OnReloading(ReloadingWeaponEventArgs ev)
    {
        // When reloading the weapon do a check to see how much ammo the player has, converting the ammo type to the weapon's ammo type
        
        //First check if they have enough of the required ammo, if so we can end
        
        //If they don't have enough of the required ammo, we need to check if what other kinds of ammo they have, starting with the highest amount
        //Remove that up to the required amount to reload and turn it into the required type
        
        
    }

    public override string Name => "AnyAmmo";
    public override int Rarity => 1;
    public override string Description => "This weapon can use any ammo type";
}