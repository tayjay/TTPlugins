using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.CustomRoles.API;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.EventArgs.Player;
using LightContainmentZoneDecontamination;
using MEC;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.HUDs;
using UnityEngine;
using VoiceChat;

namespace RoundModifiers.Modifiers.Scp507;

public class Scp507 : Modifier
{
    //Scp507 is a new role that is given to a class D, they don't know they are 507 until some event happens to reveal them.
    public static Scp507Role Scp507Role { get; set; }
    
    public uint TargetPlayerId { get; set; }
    
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
                ev.Player.Rotation, ev.Player);
            //Teleport to random room in facility
            ev.Player.Teleport(FindSafeLocation(ev.Player));
            ev.Player.DisableAllEffects(EffectCategory.Negative);
            ev.Player.DisableAllEffects(EffectCategory.Harmful);
            ev.Player.Health = ev.Player.MaxHealth;
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
            ev.Player.Teleport(FindSafeLocation(ev.Player));
            ev.Player.DisableAllEffects(EffectCategory.Negative);
            ev.Player.DisableAllEffects(EffectCategory.Harmful);
            ev.Player.Health = ev.Player.MaxHealth;
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
            ev.Player.Teleport(FindSafeLocation(ev.Player));
            ev.Player.DisableAllEffects(EffectCategory.Negative);
            ev.Player.DisableAllEffects(EffectCategory.Harmful);
            ev.Player.Health = ev.Player.MaxHealth;
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
            player.Teleport(FindSafeLocation(player));
            player.DisableAllEffects(EffectCategory.Negative);
            player.DisableAllEffects(EffectCategory.Harmful);
            player.Health = player.MaxHealth;
        }
    }

    public void OnPressNoClip(TogglingNoClipEventArgs ev)
    {
        if (ev.Player.GetCustomRoles().Contains(Scp507Role))
        {
            if (ev.Player.VoiceChannel == VoiceChatChannel.Proximity)
            {
                ev.Player.VoiceChannel = VoiceChatChannel.ScpChat;
                ev.Player.ShowHUDHint("You are now in SCP Chat", 5f);
            } else if (ev.Player.VoiceChannel == VoiceChatChannel.ScpChat)
            {
                ev.Player.VoiceChannel = VoiceChatChannel.Proximity;
                ev.Player.ShowHUDHint("You are now in Human Chat", 5f);
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
            if (room.Type == RoomType.Pocket)
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
    
    //They cannot die, and are neutral to all teams, but are allowed to kill anyone.
    protected override void RegisterModifier()
    {
        Scp507Role = new Scp507Role();
        Exiled.Events.Handlers.Player.Dying += OnDying;
        Exiled.Events.Handlers.Player.Escaping += OnEscaping;
        Exiled.Events.Handlers.Player.Handcuffing += OnCuffed;
        Exiled.Events.Handlers.Player.TogglingNoClip += OnPressNoClip;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.Dying -= OnDying;
        Exiled.Events.Handlers.Player.Escaping -= OnEscaping;
        Exiled.Events.Handlers.Player.Handcuffing -= OnCuffed;
        Exiled.Events.Handlers.Player.TogglingNoClip -= OnPressNoClip;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        
        Scp507Role = null;
        Timing.KillCoroutines(teleportHandle);
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "SCP507",
        Aliases = new [] {"507", "guy"},
        Description = "SCP507 has been moved to Site-02.",
        FormattedName = "<color=red>SCP-507</color>",
        Impact = ImpactLevel.MajorGameplay,
        MustPreload = false,
        Balance = 0
    };
}