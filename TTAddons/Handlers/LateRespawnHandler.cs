using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MEC;
using PlayerRoles;
using Respawning;
using TTCore.API;
using TTCore.HUDs;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace TTAddons.Handlers
{
    public class LateRespawnHandler : IRegistered
    {
        
        public static float RespawnTime => TTAddons.Instance.Config.LateRespawnTime;

        public float LastRespawnTime { get; set; }
        public SpawnableTeamType TeamType { get; set; }
        public List<uint> WasSuicide { get; set; }
        
        
        public void OnRespawnWave(RespawningTeamEventArgs ev)
        {
            LastRespawnTime = Time.time;
            TeamType = ev.NextKnownTeam;
        }
        
        public void OnChangeRole(ChangingRoleEventArgs ev)
        {
            if(!Round.IsStarted) return;
            if(ev.NewRole == RoleTypeId.Spectator)
            {
                if(Time.time-LastRespawnTime < RespawnTime)
                {
                    if (WasSuicide.Contains(ev.Player.NetId))
                    {
                        ev.Player.ShowHUDHint("Failed to respawn due to suicide.");
                        return;
                    }
                    Timing.CallDelayed(2f, () =>
                    {
                        switch (TeamType)
                        {
                            case SpawnableTeamType.ChaosInsurgency:
                                ev.Player.RoleManager.ServerSetRole(RoleTypeId.ChaosConscript,RoleChangeReason.Respawn);
                                break;
                            case SpawnableTeamType.NineTailedFox:
                                ev.Player.RoleManager.ServerSetRole(RoleTypeId.NtfPrivate,RoleChangeReason.Respawn);
                                break;
                            default:
                                break;
                        }
                        ev.Player.ShowHUDHint("That was close!");
                    });
                    
                }
            }
        }

        public void OnDied(DiedEventArgs ev)
        {
            if (ev.DamageHandler.IsSuicide || ev.DamageHandler.Type==DamageType.Falldown)
            {
                WasSuicide.AddIfNotContains(ev.Player.NetId);
                Timing.CallDelayed(5f, () =>
                {
                    if(WasSuicide.Contains(ev.Player.NetId))
                        WasSuicide.Remove(ev.Player.NetId);
                });
            }
            
        }
        
        public void Register()
        {
            if(RespawnTime <= 0) return;
            Exiled.Events.Handlers.Server.RespawningTeam += OnRespawnWave;
            Exiled.Events.Handlers.Player.ChangingRole += OnChangeRole;
            Exiled.Events.Handlers.Player.Died += OnDied;
            
            WasSuicide = ListPool<uint>.Pool.Get();
        }

        public void Unregister()
        {
            if(RespawnTime <= 0) return;
            Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawnWave;
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangeRole;
            Exiled.Events.Handlers.Player.Died -= OnDied;
            
            ListPool<uint>.Pool.Return(WasSuicide);
        }
    }
}