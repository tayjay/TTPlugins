using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp914;
using Exiled.Events.EventArgs.Server;
using MapGeneration;
using MEC;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PluginAPI.Events;
using RoundModifiers.API;
using TTCore.HUDs;
using TTCore.Utilities;
using UnityEngine;

namespace RoundModifiers.Modifiers;

public class ZombieSurvival : Modifier
{

    public void ChoosingTeams(ChoosingStartTeamQueueEventArgs ev)
    {
        int playerCount = ev.TeamRespawnQueue.Count;
        ev.TeamRespawnQueue.Clear();
        for(int i=0;i<playerCount;i++)
        {
            ev.TeamRespawnQueue.Add(i % 2 == 0 ? Team.ClassD : Team.Scientists);
        }
    }

    CoroutineHandle _zombieSpawnHandle;
    CoroutineHandle _zombieMoveHandle;
    CoroutineHandle _speedBoostHandle;
    CoroutineHandle _nukeTimerHandle;
    
    private float zombieSpeed { get; set; }
    //private byte playerSpeedBoost { get; set; }
    
    
    public void OnRoundStart()
    {
        //Start with 1 zombie
        _zombieSpawnHandle = Timing.RunCoroutine(ZombieSpawnCoroutine());
        _zombieMoveHandle = Timing.RunCoroutine(ZombieMoveCoroutine());
        //_speedBoostHandle = Timing.RunCoroutine(CheckSpeedBoost());

        _nukeTimerHandle = Timing.CallDelayed(ZombieSurvivalConfig.AutoNukeTime, () =>
        {
            if (!Round.IsEnded)
            {
                Warhead.DetonationTimer = 120f;
                Warhead.IsLocked = true;
                Warhead.Start();
            }
        });
    }

    public IEnumerator<float> ZombieSpawnCoroutine()
    {
        //todo: Check not in a bad room in entrance
        yield return Timing.WaitForSeconds(1f);
        int spawnCount = 0;
        while (!Round.IsEnded)
        {
            if(!Player.Get(p=>p.Role==RoleTypeId.ClassD || p.Role==RoleTypeId.Scientist).Any())
            {
                Round.EndRound(true);
                Log.Info("Attempting to end round, no humans left");
                yield break;
            }
            try
            {
                ZoneType targetType = ZoneType.HeavyContainment;
                switch(spawnCount % 5)
                {
                    case 0:
                    case 1:
                        targetType = ZoneType.HeavyContainment;
                        break;
                    case 2:
                    case 3:
                        targetType = ZoneType.Entrance;
                        break;
                    case 4:
                        targetType = ZoneType.LightContainment;
                        break;
                }

                if (spawnCount <= ZombieSurvivalConfig.TotalZombies)
                {
                    Room room = GetValidRoom(targetType);
                    NpcUtilities.CreateZombieAI(room.Position+Vector3.up);
                }
                    
            } catch
            {
                Log.Error("Failed to spawn zombie");
            }
            spawnCount++;
            if(spawnCount%3==0)
            {
                /*//slowdownFactor = (byte)(slowdownFactor - ZombieSurvivalConfig.SlowdownDecrease);
                playerSpeedBoost = (byte)(playerSpeedBoost - ZombieSurvivalConfig.PlayerSpeedBoostDecrease);
                if(playerSpeedBoost<0) playerSpeedBoost = 0;
                foreach(Player human in Player.List.Where(p=>p.IsHuman))
                {
                    if(human.Role is FpcRole role)
                    {
                        /*role.WalkingSpeed += 1f;
                        role.SprintingSpeed += 1f;#1#
                        
                        //human.ChangeEffectIntensity<MovementBoost>((byte)(playerSpeedBoost));
                        if (!human.IsNPC)
                        {
                            //human.ShowHUDHint("You feel stronger...", 3f);
                        }
                    }
                }*/
            }
            yield return Timing.WaitForSeconds(ZombieSurvivalConfig.ZombieSpawnInterval);
        }
    }

    public IEnumerator<float> ZombieMoveCoroutine()
    {
        Dictionary<ZoneType, int> playerCounts = DictionaryPool<ZoneType, int>.Pool.Get();
        while (!Round.IsEnded)
        {
            yield return Timing.WaitForSeconds(ZombieSurvivalConfig.ZombieMoveTime);
            playerCounts[ZoneType.LightContainment] = Player.Get(p => p.Zone == ZoneType.LightContainment).Count();
            playerCounts[ZoneType.HeavyContainment] = Player.Get(p => p.Zone == ZoneType.HeavyContainment).Count();
            playerCounts[ZoneType.Entrance] = Player.Get(p => p.Zone == ZoneType.Entrance).Count();
            playerCounts[ZoneType.Surface] = Player.Get(p => p.Zone == ZoneType.Surface).Count();
            
            foreach (Player zombie in Player.Get(RoleTypeId.Scp0492))
            {
                bool shouldMove = true;
                if (playerCounts[zombie.Zone] > 0)
                {
                    //There is a player in their zone, so should try being close to them
                    foreach(Door door in zombie.CurrentRoom.Doors.Where(d=>d.Rooms.Count>1))
                    {
                        Room otherRoom = door.Rooms.FirstOrDefault(r=>r!=zombie.CurrentRoom);
                        if(otherRoom==null) continue;
                        if(otherRoom.Players.Any(p=>p.IsHuman))
                        {
                            //If there is a room with human nearby don't try teleporting
                            shouldMove = false;
                            break;
                        }
                    }
                }
                else
                {
                    //There is no players in this zone, so keep where they are.
                    shouldMove = false;
                }
                if (shouldMove)
                {
                    Room targetRoom = GetValidRoom(zombie.Zone);
                    if (targetRoom != null)
                    {
                        zombie.Position = targetRoom.Position + Vector3.up;
                        targetRoom.TurnOffLights(3f);
                        if(!zombie.IsNPC)
                            zombie.ShowHUDHint("You hear a noise nearby...", 3f);
                    }
                }

                yield return Timing.WaitForOneFrame;
            }
            playerCounts.Clear();
        }
        DictionaryPool<ZoneType,int>.Pool.Return(playerCounts);
    }

    public Room GetValidRoom(ZoneType zoneType)
    {
        int attempts = 0;
        while (attempts++ < 15)
        {
            Room room = Room.Random(zoneType);
            if(room.Type == RoomType.Pocket || room.Type == RoomType.HczTesla || room.Type == RoomType.HczArmory) continue;
            if(room.Zone==ZoneType.Entrance && (room.RoomShape==RoomShape.Endroom && room.Type!=RoomType.EzGateA && room.Type!=RoomType.EzGateB)) continue;
            if(room.Players.Any(p=>p.IsHuman)) continue;
            bool nearbyPlayer = false;
            foreach(Door door in room.Doors.Where(d=>d.Rooms.Count>1))
            {
                Room otherRoom = door.Rooms.FirstOrDefault(r=>r!=room);
                if(otherRoom==null) continue;
                if(otherRoom.Players.Any(p=>p.IsHuman))
                {
                    //If there is a room with human nearby don't try teleporting
                    nearbyPlayer = true;
                }
            }
            if(nearbyPlayer) continue;
            return room;
        }

        return Room.Get(RoomType.Surface);
    }
    
    
    
    public void OnSpawned(SpawnedEventArgs ev)
    {
        if (ev.Player.Role == RoleTypeId.Scp0492)
        {
            //Do zombie logic
            if(ev.Player.Role is FpcRole role)
            {
                /*role.WalkingSpeed = 3;
                role.SprintingSpeed = 3;*/
                //ev.Player.EnableEffect<MovementBoost>(playerSpeedBoost);
            }
        }
    }

    public void OnHurting(HurtingEventArgs ev)
    {
        /*if(ev.Player.Role== RoleTypeId.Scp0492)
        {
            ev.Amount = 0;
            ev.IsAllowed = false;
        }*/
        if(ev.Attacker == null) return;
        if(ev.Attacker.IsScp) return;
        if (ev.Player.IsScp) return;
        ev.Player.EnableEffect<Slowness>((byte)35,duration:1f,addDurationIfActive:true);
        //ev.Player.DisableEffect<MovementBoost>();
        ev.Amount = 0;
        ev.IsAllowed = false;
        
    }

    /*public IEnumerator<float> CheckSpeedBoost()
    {
        while (!Round.IsEnded)
        {
            foreach (Player player in Player.List)
            {
                yield return Timing.WaitForOneFrame;
                if(player.IsScp) continue;
                if(player.IsEffectActive<MovementBoost>()) continue;
                player.EnableEffect<MovementBoost>(playerSpeedBoost);
            }

            yield return Timing.WaitForSeconds(1f);
        }
    }*/

    public void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        ev.IsAllowed = false;
    }
    
    public void OnSpawnTeamVehicle(SpawningTeamVehicleEventArgs ev)
    {
        ev.IsAllowed = false;
    }

    public void PickingUpItem(PickingUpItemEventArgs ev)
    {
        if(!ev.Pickup.Type.IsKeycard()) return;
        foreach(Item item in ev.Player.Items)
        {
            if(item.Type.IsKeycard())
            {
                ev.IsAllowed = false;
                ev.Player.ShowHUDHint("You can only have one keycard at a time", 3f);
                return;
            }
        }
    }
    
    public void OnDied(DiedEventArgs ev)
    {
        if(ev.TargetOldRole == RoleTypeId.Scp0492)
        {
            Timing.CallDelayed(ZombieSurvivalConfig.ZombieRespawnTime, () =>
            {
                ev.Player.RoleManager.ServerSetRole(RoleTypeId.Scp0492, RoleChangeReason.Respawn);
                ev.Player.Position = GetValidRoom(ZoneType.HeavyContainment).Position + Vector3.up;
            });
        }
        else if(ZombieSurvivalConfig.PlayersRespawnAsZombies)
        {
            Timing.CallDelayed(1f, () =>
            {
                ev.Player.RoleManager.ServerSetRole(RoleTypeId.Scp0492, RoleChangeReason.Respawn);
                ev.Player.Position = GetValidRoom(ZoneType.HeavyContainment).Position + Vector3.up;
            });
        }
    }

    public void OnRoundEnding(EndingRoundEventArgs ev)
    {
        Log.Debug("Trying to end round...");
        if(!ev.IsForceEnded) return;

        try
        {
            if (Player.List.Any(p => p.Role == RoleTypeId.ClassD || p.Role == RoleTypeId.Scientist))
            {
                ev.IsAllowed = false;
            }

            if (RoundSummary.EscapedScientists > RoundSummary.EscapedClassD)
            {
                Log.Info("More scientists escaped");
                ev.LeadingTeam = LeadingTeam.FacilityForces;
            } else if(RoundSummary.EscapedScientists < RoundSummary.EscapedClassD)
            {
                Log.Info("More class-d escaped");
                ev.LeadingTeam = LeadingTeam.ChaosInsurgency;
            } else if(RoundSummary.EscapedScientists == 0 && RoundSummary.EscapedClassD == 0)
            {
                Log.Info("No one escaped");
                ev.LeadingTeam = LeadingTeam.Anomalies;
            } else
            {
                Log.Info("Draw");
                ev.LeadingTeam = LeadingTeam.Draw;
            }
            ev.IsAllowed = true;
            ev.IsRoundEnded = true;
        } catch (System.Exception e)
        {
            Log.Error(e);
        }
        Log.Debug("Round ended...");
    }

    public void OnActivateScp914(ActivatingEventArgs ev)
    {
        ev.IsAllowed = false;
    }
    
    public void OnWarheadDetonation()
    {
        foreach (Player zombie in Player.Get(p => p.IsNPC && (p.Zone!=ZoneType.Surface || p.Lift!=null)))
        {
            zombie.Kill("Nuked");
        }
    }

    public void OnDecon(DecontaminatingEventArgs ev)
    {
        foreach (Player zombie in Player.Get(p => p.IsNPC && p.Zone==ZoneType.LightContainment))
        {
            zombie.Kill("Decontaminated");
        }
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Server.ChoosingStartTeamQueue += ChoosingTeams;
        Exiled.Events.Handlers.Player.Spawned += OnSpawned;
        Exiled.Events.Handlers.Player.Hurting += OnHurting;
        Exiled.Events.Handlers.Player.Died += OnDied;
        Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
        Exiled.Events.Handlers.Map.SpawningTeamVehicle += OnSpawnTeamVehicle;
        Exiled.Events.Handlers.Server.EndingRound += OnRoundEnding;
        Exiled.Events.Handlers.Scp914.Activating += OnActivateScp914;
        Exiled.Events.Handlers.Player.PickingUpItem += PickingUpItem;
        Exiled.Events.Handlers.Warhead.Detonated += OnWarheadDetonation;
        Exiled.Events.Handlers.Map.Decontaminating += OnDecon;
        
        //slowdownFactor = ZombieSurvivalConfig.StartingSlowdown;
        zombieSpeed = ZombieSurvivalConfig.ZombieStartingSpeed;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Server.ChoosingStartTeamQueue -= ChoosingTeams;
        Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
        Exiled.Events.Handlers.Player.Hurting -= OnHurting;
        Exiled.Events.Handlers.Player.Died -= OnDied;
        Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
        Exiled.Events.Handlers.Map.SpawningTeamVehicle -= OnSpawnTeamVehicle;
        Exiled.Events.Handlers.Server.EndingRound -= OnRoundEnding;
        Exiled.Events.Handlers.Scp914.Activating -= OnActivateScp914;
        Exiled.Events.Handlers.Player.PickingUpItem -= PickingUpItem;
        Exiled.Events.Handlers.Warhead.Detonated -= OnWarheadDetonation;
        Exiled.Events.Handlers.Map.Decontaminating -= OnDecon;
        
        
        Timing.KillCoroutines(_zombieSpawnHandle);
        Timing.KillCoroutines(_zombieMoveHandle);
        Timing.KillCoroutines(_speedBoostHandle);
        Timing.KillCoroutines(_nukeTimerHandle);
    }

    public override ModInfo ModInfo { get; } = new()
    {
        Name = "ZombieSurvival",
        FormattedName = "Zombie Survival",
        Aliases = new[] {"ZS"},
        Description = "Watch out for the zombies!",
        Impact = ImpactLevel.Gamemode,
        MustPreload = false,
        Balance = 0,
        Category = Category.Gamemode|Category.ScpRole|Category.HumanRole|Category.Npc,
        Hidden = true
    };

    public static Config ZombieSurvivalConfig => RoundModifiers.Instance.Config.ZombieSurvival;
    public class Config : ModConfig
    {
        /*[Description("The starting slowdown factor for zombies")]
        public byte StartingSlowdown { get; set; } = 80;
        [Description("The amount to decrease the slowdown factor by every 3 zombies spawned")]
        public byte SlowdownDecrease { get; set; } = 5;*/
        [Description("The starting speed for zombie")]
        public float ZombieStartingSpeed { get; set; } = 3f;
        [Description("The amount to decrease the speed boost by every few zombies spawned")]
        public float ZombieSpeedIncrease { get; set; } = 2f;
        [Description("The interval in seconds between zombie spawns")]
        public float ZombieSpawnInterval { get; set; } = 10f;
        [Description("The maximum amount of zombies that can be spawned")]
        public int TotalZombies { get; set; } = 5;
        [Description("How long after a zombie dies should it spawn back in the facility?")]
        public float ZombieRespawnTime { get; set; } = 5f;
        [Description("The interval in seconds between zombie moves")]
        public float ZombieMoveTime { get; set; } = 3f;
        [Description("Should players respawn as zombies when they die?")]
        public bool PlayersRespawnAsZombies { get; set; } = true;
        [Description("How long until the nuke starts")]
        public float AutoNukeTime { get; set; } = 360f;
    }
}