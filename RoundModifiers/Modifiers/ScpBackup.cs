using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
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
        if(!ScpBackupConfig.CombineWithScpRoleModifiers) return RoleTypeId.Scp0492;
        if (RoundModifiers.Instance.IsModifierActive("Puppies"))
            return RoleTypeId.Scp939;
        if (RoundModifiers.Instance.IsModifierActive("Peanuts"))
            return RoleTypeId.Scp173;
        if (RoundModifiers.Instance.IsModifierActive("BoneZone"))
            return RoleTypeId.Scp3114;
        return RoleTypeId.Scp0492;
    }
    
    public int RespawnWave { get; set; } = 1;
    
    
    public void OnRoundStart()
    {
        Timing.CallDelayed(10f, () =>
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
        
        Timing.CallDelayed(2f, () =>
        {
            for(int i=0;i<RespawnWave;i++)
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
            }
            if(ExponentialRespawns)
                RespawnWave++;
        });
    }

    public void OnSpawned(SpawnedEventArgs ev)
    {
        if (ev.Player.IsNPC && ev.Player.Role == GetBackupRole())
        {
            Timing.CallDelayed(0.2f, () =>
            {
                ev.Player.MaxHealth = ScpBackupConfig.Health;
                ev.Player.Health = ScpBackupConfig.Health;
                ev.Player.HumeShield = ScpBackupConfig.Hume;
                //ev.Player.Scale *= 0.9f;
            });
        }
    }

    public void OnDied(DiedEventArgs ev)
    {
        if(!LeaveOnDeath) return;
        if (ev.Player.IsNPC && ev.TargetOldRole == GetBackupRole())
        {
            ev.Player.ReferenceHub.OnDestroy();
            LeftEventArgs newLeft = new(ev.Player);
            Exiled.Events.Handlers.Player.OnLeft(newLeft);
        }
    }

    public void OnWarheadDetonation()
    {
        foreach (Player backup in Player.Get(p => p.IsNPC && (p.Zone!=ZoneType.Surface || p.Lift!=null)))
        {
            backup.Kill("Nuked");
        }
    }

    public void OnDecon(DecontaminatingEventArgs ev)
    {
        foreach (Player backup in Player.Get(p => p.IsNPC && p.Zone==ZoneType.LightContainment))
        {
            backup.Kill("Decontaminated");
        }
    }

    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Server.RespawningTeam += OnRespawnWave;
        Exiled.Events.Handlers.Player.Died += OnDied;
        Exiled.Events.Handlers.Player.Spawned += OnSpawned;
        Exiled.Events.Handlers.Warhead.Detonated += OnWarheadDetonation;
        Exiled.Events.Handlers.Map.Decontaminating += OnDecon;
        
        RespawnWave = 1;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawnWave;
        Exiled.Events.Handlers.Player.Died -= OnDied;
        Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
        
        RespawnWave = 1;
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
    
    public static Config ScpBackupConfig => RoundModifiers.Instance.Config.ScpBackup;
    public bool LeaveOnDeath => ScpBackupConfig.LeaveOnDeath;
    public bool ExponentialRespawns => ScpBackupConfig.ExponentialRespawns;
    
    public class Config
    {
        [Description("Whether the backup NPCs should leave the game on death. Default is true. If false, they will can be resurrected or respawn with humans.")]
        public bool LeaveOnDeath { get; set; } = true;
        
        [Description("Respawn wave behaviour. If false, 1 SCP will be spawned per wave. If true, it will first spawn 1 SCP, then 2 new wave, then 3, and so on. Default is true.")]
        public bool ExponentialRespawns { get; set; } = true;
        [Description("Whether the backup SCP should be the same as the SCP role modifiers (ex. Puppies). Default is false.")]
        public bool CombineWithScpRoleModifiers { get; set; } = false; 
        [Description("The starting health the backup SCP should have. Default is 600.")]
        public float Health { get; set; } = 600f;
        [Description("The starting hume the backup SCP should have. Default is 100.")]
        public float Hume { get; set; } = 100f;
    }
}