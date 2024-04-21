using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.Extensions;
using UnityEngine;

namespace RoundModifiers.Modifiers;

public class Paper : Modifier
{
    
    //On player spawn set their scale to be like paper
    public void OnSpawn(SpawnedEventArgs ev)
    {
        if(!ev.Player.Role.IsAlive) return;
        if (ev.Player.Role == RoleTypeId.Scp106 || ev.Player.Role == RoleTypeId.Scp939)//Found that 106 and 939 incorrectly rescale with this modifier. Need to not scale them til resolved
        {
            //ev.Player.ChangeSize(1f);
            //ev.Player.Scale = new Vector3(1,1,1);
            return;
        }
        //ev.Player.ChangeSize(new Vector3(1,1,0.01f));
        ev.Player.Scale = new Vector3(1,1,0.01f);
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.Spawned += OnSpawn;
    }

    protected override void UnregisterModifier()
    {
        TTCore.TTCore.Instance.PlayerSizeManager.ResetAll();
        Exiled.Events.Handlers.Player.Spawned -= OnSpawn;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "Paper",
        Aliases = new [] {"paper"},
        Description = "Everyone is paper thin!",
        FormattedName = "<color=yellow>S-C-Paper Mario</color>",
        Impact = ImpactLevel.MajorGameplay,
        MustPreload = false
    };
}