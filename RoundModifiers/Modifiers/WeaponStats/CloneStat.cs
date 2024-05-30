using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;
using TTCore.HUDs;

namespace RoundModifiers.Modifiers.WeaponStats;

public class CloneStat : Stat, IDying
{

    public override string Name => "Clone";
    public override int Rarity => 5;
    public override string Description => "This weapon will clone the target on death";
    
    public void OnOwnerDying(DyingEventArgs ev)
    {
        
    }

    public void OnTargetDying(DyingEventArgs ev)
    {
        ev.IsAllowed = false;
        ev.Player.RoleManager.ServerSetRole(ev.Attacker.Role.Type, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.None);
        ev.Player.DisplayNickname = ev.Attacker.DisplayNickname==string.Empty?ev.Attacker.Nickname:ev.Attacker.DisplayNickname;
        ev.Attacker.ShowHUDHint("Cloned", 5f);
    }

    public void OnOwnerDied(DiedEventArgs ev)
    {
        
    }

    public void OnTargetDied(DiedEventArgs ev)
    {
        
    }
}