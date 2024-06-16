using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.CustomRoles.API.Features;
using PlayerRoles;
using TTCore.HUDs;
using UnityEngine.Pool;

namespace RoundModifiers.Modifiers.Scp507;

public sealed class Scp507Role : CustomRole
{
    public override uint Id { get; set; } = 507;
    public override int MaxHealth { get; set; } = 200;
    public override string Name { get; set; } = "SCP-507";
    public override string Description { get; set; } = "You are SCP-507\nYou cannot die, and win alongside any team.\nHave fun!";
    public override string CustomInfo { get; set; }

    public override RoleTypeId Role { get; set; } = RoleTypeId.ClassD;

    public override List<string> Inventory { get; set; }

    public override Dictionary<RoleTypeId, float> CustomRoleFFMultiplier { get; set; }
    
    public Scp507Role()
    {
        Inventory = Exiled.API.Features.Pools.ListPool<string>.Pool.Get();
        Inventory.Add("Flashlight");
        Inventory.Add("GunCOM15");
        
        CustomRoleFFMultiplier = Exiled.API.Features.Pools.DictionaryPool<RoleTypeId, float>.Pool.Get();
        foreach (RoleTypeId id in Enum.GetValues(typeof(RoleTypeId)))
        {
            CustomRoleFFMultiplier.Add(id, 1f);
        }
    }

    public override bool KeepPositionOnSpawn { get; set; } = true;
    public override bool KeepInventoryOnSpawn { get; set; } = false;


    protected override void RoleAdded(Player player)
    {
        base.RoleAdded(player);
        Round.IgnoredPlayers.Add(player.ReferenceHub);
        player.IsBypassModeEnabled = true;
        player.ShowHUDHint("You are SCP-507\nYou cannot die, and win alongside any team.\nIf your vision goes dark use the ~ command '.507' to reset your role.", 17f);
        player.Broadcast(8,"Use the ~ command '.507' if your vision goes dark.");
    }
}