using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using InventorySystem.Items.Firearms.Attachments;
using MEC;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;
using TTCore.HUDs;
using TTCore.Utilities;
using UnityEngine;

namespace RoundModifiers.Modifiers;

public class GunGame : Modifier
{
    // Your bulk standard gun game gamemode
    // Players start with a weak weapon and have to kill other players to get stronger weapons
    // The player who gets the final kill with the final weapon wins
    
    // Want to add a catchup system so players that are in the lower levels can catch up to the higher levels
    // Give them helpful items
    
    // Guns
    public ItemType[] Weapons => new[]
    {
        ItemType.GunCOM15,
        ItemType.GunCOM18,
        ItemType.GunFSP9,
        ItemType.GunCrossvec,
        ItemType.GunE11SR,
        ItemType.GunAK,
        ItemType.GunShotgun,
        ItemType.GunRevolver,
        ItemType.GunLogicer,
        ItemType.GunFRMG0,
        ItemType.GunA7,
        ItemType.GunCom45,
        ItemType.ParticleDisruptor,
        ItemType.Jailbird,
        ItemType.MicroHID
    };

    public ItemType[] CatchupItems => new[]
    {
        ItemType.Medkit,
        ItemType.Adrenaline,
        ItemType.Painkillers,
        ItemType.GrenadeFlash,
        ItemType.GrenadeHE,
        ItemType.SCP500,
        ItemType.SCP207,
        ItemType.AntiSCP207,
        ItemType.SCP1853,
        ItemType.SCP2176,
        ItemType.SCP268
    };
    
    public List<WeightedItem<ItemType>> CatchupItemsWeighted => new()
    {
        new WeightedItem<ItemType>(ItemType.ArmorCombat, 6),
        new WeightedItem<ItemType>(ItemType.Medkit, 6),
        new WeightedItem<ItemType>(ItemType.Adrenaline, 6),
        new WeightedItem<ItemType>(ItemType.Painkillers, 7),
        new WeightedItem<ItemType>(ItemType.GrenadeFlash, 5),
        new WeightedItem<ItemType>(ItemType.GrenadeHE, 4),
        new WeightedItem<ItemType>(ItemType.SCP500, 3),
        new WeightedItem<ItemType>(ItemType.ArmorHeavy, 3),
        new WeightedItem<ItemType>(ItemType.SCP207, 2),
        new WeightedItem<ItemType>(ItemType.AntiSCP207, 2),
        new WeightedItem<ItemType>(ItemType.SCP1853, 2),
        new WeightedItem<ItemType>(ItemType.SCP2176, 1),
        new WeightedItem<ItemType>(ItemType.SCP268, 1)
    };
    
    public bool IsGameActive { get; set; }
    
    public Dictionary<Player,int> PlayerGunLevels { get; set; }
    
    private bool WasFriendlyFireEnabled = false;
    private bool WasFriendlyFireDetectionPaused = false;
    
    //public double AverageScore => PlayerGunLevels.Values.Average();
    
    public readonly int OneZoneThreshold = 7;


    public void SpawnQueue(ChoosingStartTeamQueueEventArgs ev)
    {
        int length = ev.TeamRespawnQueue.Count;
        ev.TeamRespawnQueue.Clear();
        for (int i = 0; i < length; i++)
        {
            ev.TeamRespawnQueue.Add(Team.ClassD);
        }
    }

    public void Spawning(SpawningEventArgs ev)
    {
        if(ev.Player.Role == RoleTypeId.Spectator) return;
        ev.Position = FindSafeRoom(ev.Player).Position + Vector3.up;
    }

    public void OnPlayerJoined(JoinedEventArgs ev)
    {
        if (!IsGameActive) return;
        if (ev.Player.Role != RoleTypeId.Spectator) return;
        Timing.CallDelayed(5f, () =>
        {
            if (ev.Player.Role != RoleTypeId.Spectator) return;
            ev.Player.RoleManager.ServerSetRole(RoleTypeId.ClassD, RoleChangeReason.Respawn);
        });
    }

    public void OnPlayerSpawn(SpawnedEventArgs ev)
    {
        if(ev.Player.Role == RoleTypeId.Spectator) return;
        // Setup player for the gamemode
        ev.Player.ClearInventory(true);
        if(!PlayerGunLevels.ContainsKey(ev.Player))
            PlayerGunLevels.Add(ev.Player, 0);
        ItemType weapon = Weapons[PlayerGunLevels[ev.Player]];
        ev.Player.AddItem(weapon);
        
        ev.Player.CurrentItem = ev.Player.Items.First(item => item.Type == weapon);
        if (ev.Player.CurrentItem is Firearm firearm)
        {
            ev.Player.AddAmmo(firearm.AmmoType, (byte)(firearm.MaxAmmo*3));
            firearm.ClearAttachments();
        }
            
        ev.Player.IsBypassModeEnabled = true;
        double average = PlayerGunLevels.Values.Average();
        
        if (PlayerGunLevels[ev.Player] < average - 2)
        {
            WeightedRandomSelector<ItemType> selector = new WeightedRandomSelector<ItemType>(CatchupItemsWeighted);
            ev.Player.AddItem(selector.SelectItem());
            ev.Player.ShowHUDHint("You are falling behind, here is a helpful item", 5f);
        }
        if (PlayerGunLevels[ev.Player] < average - 3)
        {
            WeightedRandomSelector<ItemType> selector = new WeightedRandomSelector<ItemType>(CatchupItemsWeighted);
            ev.Player.AddItem(selector.SelectItem());
        }
        if (PlayerGunLevels[ev.Player] < average - 4)
        {
            WeightedRandomSelector<ItemType> selector = new WeightedRandomSelector<ItemType>(CatchupItemsWeighted);
            ev.Player.AddItem(selector.SelectItem());
        }
    }

    public void OnDying(DyingEventArgs ev)
    {
        if(ev.ItemsToDrop==null) return;
        /*foreach (var item in ev.ItemsToDrop.AsEnumerable())
        {
            if(Weapons.Contains(item.Type))
                ev.ItemsToDrop.Remove(item);
        }*/
        if(!IsGameActive) return; //All items will drop during overtime
        ev.ItemsToDrop.Clear();
        ev.Player.RemoveItem(item => item !=null,true);
    }

    public void Respawn(RespawningTeamEventArgs ev)
    {
        ev.IsAllowed = false;
    }

    public void OnDied(DiedEventArgs ev)
    {
        //if (!IsGameActive) return;
        //Handle respawning player
        Timing.CallDelayed(3f, () =>
        {
            if(IsGameActive && ev.Player.Role == RoleTypeId.Spectator)
                ev.Player.RoleManager.ServerSetRole(RoleTypeId.ClassD, RoleChangeReason.Respawn);
        });
        
        // Handled giving points
        if (ev.Attacker == null) return;
        if (ev.Attacker == ev.Player) return;
        if (!PlayerGunLevels.ContainsKey(ev.Attacker))
        {
            PlayerGunLevels.Add(ev.Attacker, 0);
        }
        PlayerGunLevels[ev.Attacker]++;

        if (PlayerGunLevels[ev.Attacker] >= Weapons.Length)
        {
            //End the game
            StartEndgame(ev.Attacker);
        }
        else
        {
            //Give attacker the next weapon
            ev.Attacker.RemoveItem(item => Weapons.Contains(item.Type));
            ev.Attacker.AddItem(Weapons[PlayerGunLevels[ev.Attacker]]);
            ev.Attacker.ClearAmmo();
            foreach(Item item in ev.Attacker.Items)
            {
                if(item is Firearm firearm)
                {
                    ev.Attacker.SetAmmo(firearm.AmmoType, (byte)(firearm.MaxAmmo*3));
                    firearm.ClearAttachments();
                }
            }
            ev.Attacker.CurrentItem = ev.Attacker.Items.First(i => i.Type == Weapons[PlayerGunLevels[ev.Attacker]]);
            
        }
    }

    private CoroutineHandle NukeTimer;

    public void StartEndgame(Player winner)
    {
        if (IsGameActive)
        {
            NukeTimer = Timing.CallDelayed(240f, () =>
            {
                if (Round.IsEnded) return;
                Warhead.DetonationTimer = 60f;
                Warhead.Start();
            });
        }
        IsGameActive = false;
        Round.IsLocked = false;
        Map.ChangeLightsColor(new Color(2,0,0));
        //Server.FriendlyFire = false;
        winner.RoleManager.ServerSetRole(RoleTypeId.Scp3114, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.AssignInventory);
        Cassie.Message("SCP 3 1 1 4 has breached containment");
        
    }
    
    public void OnInteractElevator(InteractingElevatorEventArgs ev)
    {
        if(Warhead.IsInProgress && (ev.Lift.Type == ElevatorType.GateA || ev.Lift.Type == ElevatorType.GateB)) return;
        ev.IsAllowed = false;
    }
    
    public void OnInteractDoor(InteractingDoorEventArgs ev)
    {
        if(Player.List.Count <= OneZoneThreshold && ev.Door.Type==DoorType.CheckpointGate) 
            ev.IsAllowed = false;
    }
    
    public void OnInspecting(InspectFirearmEventArgs ev)
    {
        int place = PlayerGunLevels.Count(pair => pair.Value > PlayerGunLevels[ev.Player])+1;
        string placeSuffix = place switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            _ => "th"
        };
        string displayText = $"You are in {place}{placeSuffix} place with {PlayerGunLevels[ev.Player]}/{Weapons.Length} kills";
        ev.Player.ShowHUDHint(displayText, 5f);
    }
    
    public void OnSpawnItem(SpawningItemEventArgs ev)
    {
        if (IsGameActive) return;
        if (Weapons.Contains(ev.Pickup.Type))
        {
            ev.IsAllowed = false;
        }
    }

    public Room FindSafeRoom(Player player)
    {
        Dictionary<Room,int> roomThreats = DictionaryPool<Room, int>.Pool.Get();
        foreach (Room room in Room.List)
        {
            if(room.Zone == ZoneType.LightContainment || room.Zone == ZoneType.Surface || room.Zone == ZoneType.Unspecified || room.Zone == ZoneType.Other) continue;
            if(room.Type == RoomType.Pocket) continue;
            if(room.Type == RoomType.HczTesla) continue;
            if(Player.List.Count <= OneZoneThreshold && room.Zone == ZoneType.HeavyContainment) continue;
            int threat = GetRoomThreat(room);
            if(threat > 6) continue;
            roomThreats.Add(room, threat);
        }
        
        //Room safeRoom = roomThreats.OrderBy(pair => pair.Value).First().Key;
        Room safeRoom = roomThreats.Keys.GetRandomValue();
        
        DictionaryPool<Room, int>.Pool.Return(roomThreats);
        
        return safeRoom;
    }

    private int GetRoomThreat(Room room)
    {
        int threat = 0;
        threat += room.Players.Count()*6;
        threat += room.Doors.Count(d => d.Rooms.Count > 1);
        foreach(Door door in room.Doors)
        {
            foreach(Room r in door.Rooms)
            {
                if(r == room) continue;
                threat += r.Players.Count()*3;
            }
        }

        return threat;
    }

    public void OnRoundStart()
    {
        Round.IsLocked = true;
        IsGameActive = true;
    }
    
    protected override void RegisterModifier()
    {
        //Register Events
        Exiled.Events.Handlers.Server.ChoosingStartTeamQueue += SpawnQueue;
        Exiled.Events.Handlers.Player.Spawning += Spawning;
        Exiled.Events.Handlers.Player.Spawned += OnPlayerSpawn;
        Exiled.Events.Handlers.Player.Dying += OnDying;
        Exiled.Events.Handlers.Server.RespawningTeam += Respawn;
        Exiled.Events.Handlers.Player.Died += OnDied;
        Exiled.Events.Handlers.Player.InteractingElevator += OnInteractElevator;
        Exiled.Events.Handlers.Player.InteractingDoor += OnInteractDoor;
        Custom.InspectFirearm += OnInspecting;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Player.Joined += OnPlayerJoined;
        Exiled.Events.Handlers.Map.SpawningItem += OnSpawnItem;
        
        
        
        
        PlayerGunLevels = DictionaryPool<Player, int>.Pool.Get();
        
        WasFriendlyFireEnabled = Server.FriendlyFire;
        WasFriendlyFireDetectionPaused = FriendlyFireConfig.PauseDetector;
        Server.FriendlyFire = true;
        FriendlyFireConfig.PauseDetector = true;
        ServerConfigSynchronizer.RefreshAllConfigs();
        
        IsGameActive = false;

    }

    protected override void UnregisterModifier()
    {
        //Unregister Events
        Exiled.Events.Handlers.Server.ChoosingStartTeamQueue -= SpawnQueue;
        Exiled.Events.Handlers.Player.Spawning -= Spawning;
        Exiled.Events.Handlers.Player.Spawned -= OnPlayerSpawn;
        Exiled.Events.Handlers.Player.Dying -= OnDying;
        Exiled.Events.Handlers.Server.RespawningTeam -= Respawn;
        Exiled.Events.Handlers.Player.Died -= OnDied;
        Exiled.Events.Handlers.Player.InteractingElevator -= OnInteractElevator;
        Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractDoor;
        Custom.InspectFirearm -= OnInspecting;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Player.Joined -= OnPlayerJoined;
        Exiled.Events.Handlers.Map.SpawningItem -= OnSpawnItem;
        
        
        
        
        
        DictionaryPool<Player, int>.Pool.Return(PlayerGunLevels);
        
        Server.FriendlyFire = WasFriendlyFireEnabled;
        FriendlyFireConfig.PauseDetector = WasFriendlyFireDetectionPaused;
        ServerConfigSynchronizer.RefreshAllConfigs();
        
        IsGameActive = false;
        
        Timing.KillCoroutines(NukeTimer);
    }

    public override ModInfo ModInfo { get; } = new ModInfo
    {
        Name = "GunGame",
        Aliases = new []{"gg"},
        FormattedName = "<color=green>Gun Game</color>",
        Description = "Players start with a weak weapon and have to kill other players to get stronger weapons.",
        MustPreload = true,
        Balance = 0,
        Impact = ImpactLevel.Gamemode,
        Category = Category.Combat | Category.Gamemode | Category.HumanRole | Category.ScpRole
    };
}