using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using TTCore.HUDs;
using UnityEngine;

namespace RoundModifiers.Modifiers.Scp1507;

public class Scp1507Role : CustomRole
{
    public override uint Id { get; set; } = 1507;
    public override int MaxHealth { get; set; } = 200;
    public override string Name { get; set; } = "SCP-1507";
    public override string Description { get; set; }  = "Flamingos!";
    public override string CustomInfo { get; set; } = "SCP-1507";
    public override Vector3 Scale { get; set; } = new Vector3(0.35f, 0.35f, 0.35f);
    public override RoleTypeId Role { get; set; } = RoleTypeId.Tutorial;
    public override bool KeepRoleOnDeath { get; set; } = false;
    public override bool KeepRoleOnChangingRole { get; set; } = false;
    public override bool KeepInventoryOnSpawn { get; set; } = false;
    public override bool KeepPositionOnSpawn { get; set; } = true;

    protected override void RoleAdded(Player player)
    {
        base.RoleAdded(player);
        player.ShowHUDHint("You are SCP-1507, stay with the flock and kill everyone else! Swing your jailbird to attack, break down doors, and revive your team", 17f);
    }
}