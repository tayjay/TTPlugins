using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;
using TTCore.HUDs;

namespace RoundModifiers.Modifiers.WeaponStats;

public class DefibStat : Stat, IDying
{

    public override string Name => "Defib";
    public override int Rarity => 10;
    public override string Description => "This weapon will revive the owner on death";
    
    public void OnOwnerDying(DyingEventArgs ev)
    {
        ev.IsAllowed = false;
        ev.Player.RemoveHeldItem(true);
        ev.Player.Health = ev.Player.MaxHealth;
        ev.Player.ShowHUDHint("Your life was spared, but your gun is gone.", 5f);
    }

    public void OnTargetDying(DyingEventArgs ev)
    {
        
    }

    public void OnOwnerDied(DiedEventArgs ev)
    {
        
    }

    public void OnTargetDied(DiedEventArgs ev)
    {
        
    }

}