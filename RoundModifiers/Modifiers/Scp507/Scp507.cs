using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Warhead;
using LightContainmentZoneDecontamination;
using MapGeneration;
using MEC;
using PlayerRoles;
using PlayerStatsSystem;
using RoundModifiers.API;
using TTCore.HUDs;
using UnityEngine;
using VoiceChat;
using VoiceChat.Networking;

namespace RoundModifiers.Modifiers.Scp507;

public class Scp507 : Modifier
{
    //Scp507 is a new role that is given to a class D, they don't know they are 507 until some event happens to reveal them.
    public static Scp507Role Scp507Role { get; set; }
    public bool ListenToScpChat { get; set; } = true;
    
    public uint TargetPlayerId { get; set; }
    
    private bool WasFriendlyFireEnabled = false;
    private bool WasFriendlyFireDetectionPaused = false;
    
    //Upon dying, or random timer ending they are teleported to a random location on the map.
    public void OnDying(DyingEventArgs ev)
    {
        if (ev.Player.NetId == TargetPlayerId)
        {
            ev.IsAllowed = false;
            if (!Scp507Role.Check(ev.Player))
            {
                Scp507Role.AddRole(ev.Player);
                Round.IgnoredPlayers.Add(ev.Player.ReferenceHub);
            }

            List<Item> items = new List<Item>();
            foreach (var item in ev.Player.Items)
            {
                if(item.Type == ItemType.GunCOM15 || item.Type == ItemType.Flashlight)
                    continue;
                items.Add(item);
            }
            foreach (var item in items)
            {
                ev.Player.DropItem(item);
            }

            Ragdoll.CreateAndSpawn(ev.Player.Role.Type, ev.Player.DisplayNickname, ev.DamageHandler, ev.Player.Position,
                ev.Player.Rotation, ev.Player).IsConsumed = true;
            ev.Player.RoleManager.ServerSetRole(RoleTypeId.ClassD, RoleChangeReason.Revived, RoleSpawnFlags.None); // Patching client assuming player dead and permanently blinding them
            //Teleport to random room in facility
            ev.Player.Teleport(FindSafeLocation(ev.Player));
            ev.Player.DisableAllEffects(EffectCategory.Negative);
            ev.Player.DisableAllEffects(EffectCategory.Harmful);
            ev.Player.Health = ev.Player.MaxHealth;
            ev.Player.EnableEffect(EffectType.SpawnProtected, 1, 7f); 
            ev.Player.ShowHUDHint("If your vision goes dark use the ~ command '.507' to reset your role.", 17f);
        }
    }
    
    public void OnEscaping(EscapingEventArgs ev)
    {
        if (ev.Player.NetId == TargetPlayerId)
        {
            ev.IsAllowed = false;
            if (!Scp507Role.Check(ev.Player))
            {
                Scp507Role.AddRole(ev.Player);
                Round.IgnoredPlayers.Add(ev.Player.ReferenceHub);
            }
            List<Item> items = new List<Item>();
            foreach (var item in ev.Player.Items)
            {
                if(item.Type == ItemType.GunCOM15 || item.Type == ItemType.Flashlight)
                    continue;
                items.Add(item);
            }
            foreach (var item in items)
            {
                ev.Player.DropItem(item);
            }
            //Teleport to random room in facility
            Ragdoll.CreateAndSpawn(ev.Player.Role.Type, ev.Player.DisplayNickname, "SCP-507", ev.Player.Position,
                ev.Player.Rotation, ev.Player).IsConsumed = true; // Patch infinite healing for zombies
            ev.Player.Teleport(FindSafeLocation(ev.Player));
            ev.Player.DisableAllEffects(EffectCategory.Negative);
            ev.Player.DisableAllEffects(EffectCategory.Harmful);
            ev.Player.Health = ev.Player.MaxHealth;
            ev.Player.EnableEffect(EffectType.SpawnProtected, 1, 3f);
            ev.Player.RoleManager.ServerSetRole(RoleTypeId.ClassD, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.None);
        }
    }

    public void OnCuffed(HandcuffingEventArgs ev)
    {
        if (ev.Player.NetId == TargetPlayerId)
        {
            ev.IsAllowed = false;
            if (!Scp507Role.Check(ev.Player))
            {
                Scp507Role.AddRole(ev.Player); 
                Round.IgnoredPlayers.Add(ev.Player.ReferenceHub);
                
            }
            List<Item> items = new List<Item>();
            foreach (var item in ev.Player.Items)
            {
                if(item.Type == ItemType.GunCOM15 || item.Type == ItemType.Flashlight)
                    continue;
                items.Add(item);
            }
            foreach (var item in items)
            {
                ev.Player.DropItem(item);
            }
            //Teleport to random room in facility
            Ragdoll.CreateAndSpawn(ev.Player.Role.Type, ev.Player.DisplayNickname, "SCP-507", ev.Player.Position,
                ev.Player.Rotation, ev.Player).IsConsumed = true;
            ev.Player.Teleport(FindSafeLocation(ev.Player));
            ev.Player.DisableAllEffects(EffectCategory.Negative);
            ev.Player.DisableAllEffects(EffectCategory.Harmful);
            ev.Player.Health = ev.Player.MaxHealth;
            ev.Player.EnableEffect(EffectType.SpawnProtected, 1, 3f);
            ev.Player.RoleManager.ServerSetRole(RoleTypeId.ClassD, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.None);
        }
    }

    CoroutineHandle teleportHandle;
    public void OnRoundStart()
    {
        //Randomly select a player to be 507
        Player player = Player.List.GetRandomValue(p => p.Role == RoleTypeId.ClassD);
        TargetPlayerId = player.NetId;
        teleportHandle = Timing.RunCoroutine(TeleportTick(player));
    }

    public IEnumerator<float> TeleportTick(Player player)
    {
        yield return Timing.WaitForOneFrame;
        while (player.IsAlive)
        {
            if (player == null) break;
            yield return Timing.WaitForSeconds(Random.Range(60, 300));
            if (!Scp507Role.Check(player))
            {
                Scp507Role.AddRole(player);
                Round.IgnoredPlayers.Add(player.ReferenceHub);
                
            }
            List<Item> items = new List<Item>();
            foreach (var item in player.Items)
            {
                if(item.Type == ItemType.GunCOM15 || item.Type == ItemType.Flashlight)
                    continue;
                items.Add(item);
            }
            foreach (var item in items)
            {
                player.DropItem(item);
            }
            //Teleport to random room in facility
            Ragdoll.CreateAndSpawn(player.Role.Type, player.DisplayNickname, "SCP-507", player.Position,
                player.Rotation, player).IsConsumed = true;
            player.Teleport(FindSafeLocation(player));
            player.DisableAllEffects(EffectCategory.Negative);
            player.DisableAllEffects(EffectCategory.Harmful);
            player.Health = player.MaxHealth;
            player.EnableEffect(EffectType.SpawnProtected, 1, 3f);
            yield return Timing.WaitForSeconds(1f);
            player.RoleManager.ServerSetRole(RoleTypeId.ClassD, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.None);
        }
    }

    // todo: Fix this as VoiceChannel doesn't seem to to be a syncvar
    public void OnPressNoClip(TogglingNoClipEventArgs ev)
    {
        if (Scp507Role.Check(ev.Player))
        {
            ListenToScpChat = !ListenToScpChat;
            if (ListenToScpChat)
            {
                ev.Player.Broadcast(5, "You are now listening to SCP chat.");
            }
            else
            {
                ev.Player.Broadcast(5, "You are no longer listening to SCP chat.");
            }
        }
    }

    public void OnVoiceChatting(VoiceChattingEventArgs ev)
    {
        if (!ListenToScpChat) return;
        if (ev.VoiceMessage.Channel == VoiceChatChannel.ScpChat)
        {
            //Want to also send to 507
            Player scp507 = Player.Get(TargetPlayerId);
            if(!Scp507Role.Check(scp507)) return;
            //Create a copy of the voice message
            VoiceMessage message = ev.VoiceMessage with
            {
                Channel = VoiceChatChannel.RoundSummary
            };
            scp507.ReferenceHub.connectionToClient.Send(message);
        } else if (Scp507Role.Check(ev.Player))
        {
            //Want to send to SCP chat
            VoiceMessage message = ev.VoiceMessage with
            {
                Channel = VoiceChatChannel.RoundSummary
            };
            foreach (Player player in Player.List.Where(
                player => player.Role.Team == Team.SCPs))
            {
                player.ReferenceHub.connectionToClient.Send(message);
            }
        }
        
            
        
    }

    public Vector3 FindSafeLocation(Player player)
    {
        ZoneType zone = player.CurrentRoom.Zone;

        if (zone == ZoneType.Surface)
        {
            zone = ZoneType.HeavyContainment;
        }
        if(zone == ZoneType.Other || zone == ZoneType.Unspecified)
            zone = ZoneType.HeavyContainment;
        if(AlphaWarheadController.Detonated)
            return Room.Get(RoomType.Surface).Position + Vector3.up;
        if (zone == ZoneType.LightContainment && (byte)Map.DecontaminationState >= 4)
        {
            zone = ZoneType.HeavyContainment;
        }
        
        Room room = Room.Random(zone);
        int attempts = 0;
        while (attempts < 10)
        {
            if (room.Type == RoomType.Pocket || room.Type == RoomType.HczTesla)
            {
                room = Room.Random(zone);
                attempts++;
                continue;
            }
            // Patching: Getting stuck out of bounds in Entrance
            if (room.Zone == ZoneType.Entrance && room.RoomShape == RoomShape.Endroom &&
                room.Type != RoomType.EzGateA && room.Type != RoomType.EzGateB)
            {
                room = Room.Random(zone);
                attempts++;
                continue;
            }

            Log.Debug("Attempting to teleport 507 to "+room.Name+" in "+zone+" zone.");
            if(!room.Players.Any())
                return room.Position + Vector3.up;
            Log.Debug("Failed to teleport due to room being occupied. Retrying.");
            room = Room.Random(zone);
            attempts++;
        }
        Log.Error("Could not find a safe location for SCP-507, returning current room.");
        return player.CurrentRoom.Position + Vector3.up;

    }
    
    public void OnInteractingDoor(InteractingDoorEventArgs ev)
    {
        //Prevent people from entering 079 prematurely
        if (ev.Door is Gate gate)
        {
            if (gate.Room.Type == RoomType.Hcz079)
            {
                if (Generator.Get(GeneratorState.Engaged).Count() != 3)
                {
                    ev.IsAllowed = false;
                }
            }
                
        }
    }
    
    //They cannot die, and are neutral to all teams, but are allowed to kill anyone.
    protected override void RegisterModifier()
    {
        Scp507Role = new Scp507Role();
        Exiled.Events.Handlers.Player.Dying += OnDying;
        Exiled.Events.Handlers.Player.Escaping += OnEscaping;
        Exiled.Events.Handlers.Player.Handcuffing += OnCuffed;
        Exiled.Events.Handlers.Player.TogglingNoClip += OnPressNoClip;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
        Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
        ListenToScpChat = true;
        
        WasFriendlyFireEnabled = Server.FriendlyFire;
        WasFriendlyFireDetectionPaused = FriendlyFireConfig.PauseDetector;
        Server.FriendlyFire = true;
        FriendlyFireConfig.PauseDetector = true;
        ServerConfigSynchronizer.RefreshAllConfigs();
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.Dying -= OnDying;
        Exiled.Events.Handlers.Player.Escaping -= OnEscaping;
        Exiled.Events.Handlers.Player.Handcuffing -= OnCuffed;
        Exiled.Events.Handlers.Player.TogglingNoClip -= OnPressNoClip;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
        Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;
        
        Scp507Role = null;
        Timing.KillCoroutines(teleportHandle);
        
        Server.FriendlyFire = WasFriendlyFireEnabled;
        FriendlyFireConfig.PauseDetector = WasFriendlyFireDetectionPaused;
        ServerConfigSynchronizer.RefreshAllConfigs();
        
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "SCP507",
        Aliases = new [] {"507", "guy"},
        Description = "SCP507 has been moved to Site-02.",
        FormattedName = "<color=red>SCP-507</color>",
        Impact = ImpactLevel.MajorGameplay,
        MustPreload = false,
        Balance = 0,
        Category = Category.CustomRole
    };
}