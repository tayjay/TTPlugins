using System.Collections.Generic;
using System.ComponentModel;
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
using MapGeneration;
using MEC;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;
using TTCore.HUDs;
using TTCore.Utilities;
using UnityEngine;
using Utils.NonAllocLINQ;
using Firearm = Exiled.API.Features.Items.Firearm;

namespace RoundModifiers.Modifiers.GunGame;

public class GunGame : Modifier
{
    // Your bulk standard gun game gamemode
    // Players start with a weak weapon and have to kill other players to get stronger weapons
    // The player who gets the final kill with the final weapon wins
    
    // Want to add a catchup system so players that are in the lower levels can catch up to the higher levels
    // Give them helpful items
    
    
    public bool SequentialGuns => GunGameConfig.Sequential;
    
    // Guns
    public ItemType[] Weapons => new[]
    {
        ItemType.GunFRMG0,
        ItemType.GunLogicer,
        ItemType.GunA7,
        ItemType.GunE11SR,
        ItemType.GunRevolver,
        ItemType.GunShotgun,
        ItemType.GunAK,
        ItemType.GunCrossvec,
        ItemType.GunFSP9,
        ItemType.GunCOM18,
        ItemType.ParticleDisruptor,
        ItemType.GunCom45,
        ItemType.Jailbird,
        ItemType.MicroHID,
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
        ItemType.SCP268,
        ItemType.SCP018
    };
    
    public List<WeightedItem<ItemType>> CatchupItemsWeighted => new()
    {
        
        new WeightedItem<ItemType>(ItemType.Medkit, 6),
        new WeightedItem<ItemType>(ItemType.Adrenaline, 6),
        new WeightedItem<ItemType>(ItemType.Painkillers, 7),
        new WeightedItem<ItemType>(ItemType.GrenadeFlash, 5),
        new WeightedItem<ItemType>(ItemType.GrenadeHE, 4),
        new WeightedItem<ItemType>(ItemType.SCP500, 3),
        new WeightedItem<ItemType>(ItemType.SCP207, 2),
        new WeightedItem<ItemType>(ItemType.AntiSCP207, 2),
        new WeightedItem<ItemType>(ItemType.SCP1853, 2),
        new WeightedItem<ItemType>(ItemType.SCP244a, 1),
        new WeightedItem<ItemType>(ItemType.SCP268, 1),
        new WeightedItem<ItemType>(ItemType.SCP018, 1),
        new WeightedItem<ItemType>(ItemType.ArmorCombat, 0.5f),
    };
    
    public bool IsGameActive { get; set; }
    
    //public Dictionary<Player,int> PlayerGunLevels { get; set; }
    public Dictionary<Player,List<ItemType>> PlayerKillItems { get; set; }
    public Dictionary<Player, ItemType> PlayerCurrentWeapon { get; set; }
    
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
        if(ev.Player.Role.Side == Side.Scp) return;
        // Setup player for the gamemode
        ev.Player.ClearInventory(true);
        
        if(!PlayerKillItems.ContainsKey(ev.Player))
            PlayerKillItems.Add(ev.Player, ListPool<ItemType>.Pool.Get());
        if(!PlayerCurrentWeapon.ContainsKey(ev.Player))
            PlayerCurrentWeapon.Add(ev.Player, GetNextWeapon(ev.Player));
        
        ev.Player.AddItem(PlayerCurrentWeapon[ev.Player]);
        
        ev.Player.CurrentItem = ev.Player.Items.First(item => item.Type == PlayerCurrentWeapon[ev.Player]);
        if (ev.Player.CurrentItem is Firearm firearm)
        {
            if(firearm.AmmoType == AmmoType.None) return;
            ev.Player.SetAmmo(firearm.AmmoType, (byte)(firearm.MaxAmmo*3));
            firearm.ClearAttachments();
        }
            
        ev.Player.IsBypassModeEnabled = true;
        //double average = PlayerGunLevels.Values.Average();
        int leaderPoints = GetLeaderPoints();
        int playerPoints = PlayerKillItems[ev.Player].Count;
        
        
        if (playerPoints < leaderPoints - 2)
        {
            WeightedRandomSelector<ItemType> selector = new WeightedRandomSelector<ItemType>(CatchupItemsWeighted.Where(item => ev.Player.Items.All(i => i.Type != item.Item)));
            ev.Player.AddItem(selector.SelectItem());
            ev.Player.ShowHUDHint("You are falling behind, here is a helpful item", 5f);
        }
        if (playerPoints < leaderPoints - 3)
        {
            WeightedRandomSelector<ItemType> selector = new WeightedRandomSelector<ItemType>(CatchupItemsWeighted.Where(item => ev.Player.Items.All(i => i.Type != item.Item)));
            ev.Player.AddItem(selector.SelectItem());
        }
        if (playerPoints < leaderPoints - 4)
        {
            WeightedRandomSelector<ItemType> selector = new WeightedRandomSelector<ItemType>(CatchupItemsWeighted.Where(item => ev.Player.Items.All(i => i.Type != item.Item)));
            ev.Player.AddItem(selector.SelectItem());
        }
    }
    
    public int GetLeaderPoints()
    {
        return PlayerKillItems.Values.Max(list => list.Count);
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
        //ev.Player.RemoveItem(item => item !=null,true);
        ev.Player.ClearInventory();
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
        /*if (ev.Attacker == null) return;
        if (ev.Attacker == ev.Player) return;
        if (ev.Attacker.Role.Side == Side.Scp) return;
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
            
        }*/
        ProcessKill(ev.Attacker, ev.Player);
    }

    public void ProcessKill(Player killer, Player victim)
    {
        if(killer == null) return;
        if(killer == victim) return;
        if(killer.Role.Side == Side.Scp) return;
        if (!PlayerKillItems.ContainsKey(killer))
        {
            PlayerKillItems.Add(killer, ListPool<ItemType>.Pool.Get());
        }
        
        if (!PlayerKillItems[killer].Contains(PlayerCurrentWeapon[killer]))
        {
            //This is a new weapon kill, give'em a point
            PlayerKillItems[killer].Add(killer.CurrentItem.Type);
        }

        if (PlayerKillItems[killer].Count == Weapons.Length)
        {
            // Convert to SCP
            StartEndgame(killer);
            return;
        }
        else
        {
            killer.RemoveItem(item => Weapons.Contains(item.Type));
            PlayerCurrentWeapon[killer] = GetNextWeapon(killer);
            killer.ClearAmmo();
            if(PlayerCurrentWeapon[killer] == ItemType.None) return;
            killer.AddItem(PlayerCurrentWeapon[killer]);
            foreach(Item item in killer.Items)
            {
                if(item is Firearm firearm)
                {
                    if(firearm.AmmoType == AmmoType.None) continue;
                    killer.SetAmmo(firearm.AmmoType, (byte)(firearm.MaxAmmo*3));
                    firearm.ClearAttachments();
                }
            }
            killer.CurrentItem = killer.Items.First(i => i.Type == PlayerCurrentWeapon[killer]);
        }
        

    }

    public ItemType GetNextWeapon(Player player)
    {
        //Get already completed guns
        List<ItemType> completedGuns = PlayerKillItems[player];
        //Get the next gun
        ItemType nextGun = ItemType.None;
        if (SequentialGuns)
        {
            if(PlayerKillItems[player].Count >= Weapons.Length) return ItemType.None;
            nextGun = Weapons[PlayerKillItems[player].Count];
        }
        else
        {
            if(PlayerKillItems[player].Count >= Weapons.Length) return ItemType.None;
            nextGun = Weapons.GetRandomValue(gun => !completedGuns.Contains(gun));
        }
        
        return nextGun;
    }

    private CoroutineHandle NukeTimer;
    private CoroutineHandle HudUpdate;

    public void StartEndgame(Player winner)
    {
        if (IsGameActive)
        {
            NukeTimer = Timing.CallDelayed(180f, () =>
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
        winner.ClearInventory();
        winner.RoleManager.ServerSetRole(RoleTypeId.Scp3114, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.AssignInventory);
        Cassie.Message("SCP 3 1 1 4 has breached containment");
        Timing.CallDelayed(0.3f, () =>
        {
            winner.MaxHealth = 400;
            winner.Health = winner.MaxHealth;
        });
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
        /*int currentPoints = PlayerKillItems[ev.Player].Count;
        int playersWithMorePoints = PlayerKillItems.Count(value=>value.Value.Count > currentPoints);
        playersWithMorePoints += 1;
        string placeSuffix = playersWithMorePoints switch
        {
            1 => "st",
            2 => "nd",
            3 => "rd",
            _ => "th"
        };
        string displayText = $"You are in {playersWithMorePoints}{placeSuffix} place with {PlayerKillItems[ev.Player]}/{Weapons.Length} kills";
        ev.Player.ShowHUDHint(displayText, 5f);*/
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
            if(room.Type == RoomType.HczEzCheckpointA || room.Type == RoomType.HczEzCheckpointB) continue;
            if(room.Type == RoomType.Pocket) continue;
            if(room.Type == RoomType.HczTesla) continue;
            if(Player.List.Count <= OneZoneThreshold && room.Zone == ZoneType.HeavyContainment) continue;
            // Patch: People getting stuck out of bounds
            if(room.Zone == ZoneType.Entrance && room.RoomShape == RoomShape.Endroom && room.Type != RoomType.EzGateA && room.Type != RoomType.EzGateB) continue;
            
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
        Server.FriendlyFire = true;
        FriendlyFireConfig.PauseDetector = true;
        HudUpdate = Timing.RunCoroutine(HudUpdateTick());
    }

    public IEnumerator<float> HudUpdateTick()
    {
        while (!Round.IsEnded)
        {
            foreach (Player player in Player.List.ToList())
            {
                try
                {
                    if(player==null) continue;
                    if(player.IsNPC) continue;
                    if(player.IsDead) continue;
                
                    
                    if (!player.TryGetHUD(out HUD hud))
                    {
                        hud = HUD.SetupHUD(player, new GunGameHUDLayout(hud));
                    }
                    if (hud == null) continue;
                    GunGameHUDLayout layout = hud.GetLayout<GunGameHUDLayout>();
                    if (layout == null)
                    {
                        hud.SetLayout(new GunGameHUDLayout(hud));
                        layout = hud.GetLayout<GunGameHUDLayout>();
                    }
                
                    layout.Name = player.Nickname;
                    layout.Kills = PlayerKillItems[player].Count;
                    layout.CurrentWeapon = PlayerCurrentWeapon[player].ToString();
                    if (SequentialGuns)
                    {
                        if(PlayerKillItems[player].Count+1 >= Weapons.Length) 
                            layout.NextWeapon = "SCP-3114";
                        else
                            layout.NextWeapon = Weapons[PlayerKillItems[player].Count+1].ToString();
                    }
                    
                    int currentPoints = PlayerKillItems[player].Count;
                    int playersWithMorePoints = PlayerKillItems.Count(value=>value.Value.Count > currentPoints);
                    playersWithMorePoints += 1;
                    
                    layout.Place = playersWithMorePoints;
                    if (layout.Place == 1)
                    {
                        //Find person in second place
                        var second = PlayerKillItems.OrderByDescending(pair => pair.Value.Count).Skip(1).First();
                        layout.OtherName = second.Key.Nickname;
                        layout.OtherKills = second.Value.Count;
                        layout.OtherPlace = 2;
                    }
                    else
                    {
                        //Find person in first place
                        var first = PlayerKillItems.OrderByDescending(pair => pair.Value.Count).First();
                        layout.OtherName = first.Key.Nickname;
                        layout.OtherKills = first.Value.Count;
                        layout.OtherPlace = 1;
                    }
                    
                    //Log.Debug("HUD Updated");
                } catch (System.Exception e)
                {
                    Log.Error("Error in LevelUpHandler HUD Update: "+e);
                }
                yield return Timing.WaitForOneFrame;
            }
            yield return Timing.WaitForSeconds((float)(1f-(Server.PlayerCount/Server.Tps)));
        }
    }
    
    public void OnBanningPlayer(BanningEventArgs ev)
    {
        ev.IsAllowed = false;
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
        Exiled.Events.Handlers.Player.Banning += OnBanningPlayer;
        
        
        
        
        PlayerKillItems = DictionaryPool<Player, List<ItemType>>.Pool.Get();
        PlayerCurrentWeapon = DictionaryPool<Player, ItemType>.Pool.Get();
        
        WasFriendlyFireEnabled = Server.FriendlyFire;
        WasFriendlyFireDetectionPaused = FriendlyFireConfig.PauseDetector; 
        Server.FriendlyFire = true;
        FriendlyFireConfig.PauseDetector = true;
        //ServerConfigSynchronizer.RefreshAllConfigs();
        
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
        Exiled.Events.Handlers.Player.Banning -= OnBanningPlayer;
        
        
        
        
        
        
        DictionaryPool<Player, List<ItemType>>.Pool.Return(PlayerKillItems);
        DictionaryPool<Player, ItemType>.Pool.Return(PlayerCurrentWeapon);
        
        Server.FriendlyFire = WasFriendlyFireEnabled;
        FriendlyFireConfig.PauseDetector = WasFriendlyFireDetectionPaused;
        ServerConfigSynchronizer.RefreshAllConfigs();
        
        IsGameActive = false;
        
        Timing.KillCoroutines(NukeTimer);
        Timing.KillCoroutines(HudUpdate);
    }

    public override ModInfo ModInfo { get; } = new ModInfo
    {
        Name = "GunGame",
        Aliases = new []{"gg"},
        FormattedName = "<color=green>Gun Game</color>",
        Description = "Players start with a weak weapon and have to kill other players to get stronger weapons.",
        MustPreload = true,
        Balance = 0,
        Hidden = true,
        Impact = ImpactLevel.Gamemode,
        Category = Category.Combat | Category.Gamemode | Category.HumanRole | Category.ScpRole | Category.HUD
    };

    public static Config GunGameConfig => RoundModifiers.Instance.Config.GunGame;

    public class Config : ModConfig
    {
        [Description("Should guns be given in a sequential order (true) or randomly (false)? Default is true.")]
        public bool Sequential { get; set; } = true;
    }
}