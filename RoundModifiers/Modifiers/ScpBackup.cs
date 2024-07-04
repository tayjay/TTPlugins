using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.Utilities;
using UnityEngine;

namespace RoundModifiers.Modifiers;

public class ScpBackup : Modifier
{

    //Need a switch statement to check what modifier is active
    public RoleTypeId GetBackupRole()
    {
        if (RoundModifiers.Instance.IsModifierActive("Puppies"))
            return RoleTypeId.Scp939;
        if (RoundModifiers.Instance.IsModifierActive("Peanuts"))
            return RoleTypeId.Scp173;
        if (RoundModifiers.Instance.IsModifierActive("BoneZone"))
            return RoleTypeId.Scp3114;
        return RoleTypeId.Scp0492;
    }
    
    
    public void OnRoundStart()
    {
        Timing.CallDelayed(3f, () =>
        {
            Room randomRoom = Room.Random(ZoneType.HeavyContainment);
            if(randomRoom.Type == RoomType.Pocket || randomRoom.Type == RoomType.HczTesla)
            {
                randomRoom = Room.Get(RoomType.Hcz049);
            }
            Vector3 spawnPosition = randomRoom.Position + Vector3.up;
            
            List<Player> scpPlayers = Player.Get(p=>p.Role.Side==Side.Scp && !p.IsNPC && p.Role!=RoleTypeId.Scp079).ToList();
            if(scpPlayers.Any())
            {
                spawnPosition = scpPlayers.GetRandomValue().Position+ Vector3.up;
            }

            NpcUtilities.CreateBasicAI(GetBackupRole(), spawnPosition);
        });
    }
    
    public void OnRespawnWave(RespawningTeamEventArgs ev)
    {
        
        Timing.CallDelayed(3f, () =>
        {
            Room randomRoom = Room.Random(ZoneType.HeavyContainment);
            if(randomRoom.Type == RoomType.Pocket || randomRoom.Type == RoomType.HczTesla)
            {
                randomRoom = Room.Get(RoomType.Hcz049);
            }
            Vector3 spawnPosition = randomRoom.Position + Vector3.up;
            
            List<Player> scpPlayers = Player.Get(p=>p.Role.Side==Side.Scp && !p.IsNPC && p.Role!=RoleTypeId.Scp079).ToList();
            if(scpPlayers.Any())
            {
                spawnPosition = scpPlayers.GetRandomValue().Position+ Vector3.up;
            }

            NpcUtilities.CreateBasicAI(GetBackupRole(), spawnPosition);
        });
    }

    public void OnSpawned(SpawnedEventArgs ev)
    {
        if (ev.Player.IsNPC && ev.Player.Role == GetBackupRole())
        {
            Timing.CallDelayed(0.2f, () =>
            {
                ev.Player.MaxHealth = 600;
                ev.Player.Health = 600;
                ev.Player.HumeShield = 100f;
            });
        }
    }

    public void OnDied(DiedEventArgs ev)
    {
        if(!RoundModifiers.Instance.Config.ScpBackup_LeaveOnDeath) return;
        if (ev.Player.IsNPC && ev.TargetOldRole == GetBackupRole())
        {
            ev.Player.ReferenceHub.OnDestroy();
            LeftEventArgs newLeft = new(ev.Player);
            Exiled.Events.Handlers.Player.OnLeft(newLeft);
        }
    }

    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Server.RespawningTeam += OnRespawnWave;
        Exiled.Events.Handlers.Player.Died += OnDied;
        Exiled.Events.Handlers.Player.Spawned += OnSpawned;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawnWave;
        Exiled.Events.Handlers.Player.Died -= OnDied;
        Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "SCPBackup",
        Aliases = new[] {"backup"},
        FormattedName = "SCP Backup",
        Balance = -2,
        Category = Category.Npc,
        Description = "Scp team gets some backup!",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false,
    };
}