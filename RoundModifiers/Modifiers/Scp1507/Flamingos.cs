using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pools;
using Exiled.CustomRoles.API;
using Exiled.Events.EventArgs.Item;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Interactables.Interobjects;
using Interactables.Interobjects.DoorUtils;
using InventorySystem.Items.Jailbird;
using MEC;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.Events.EventArgs;
using TTCore.Extensions;
using TTCore.HUDs;
using UnityEngine;
using VoiceChat;
using BreakableDoor = Exiled.API.Features.Doors.BreakableDoor;
using CheckpointDoor = Exiled.API.Features.Doors.CheckpointDoor;
using IDamageableDoor = Exiled.API.Interfaces.IDamageableDoor;

namespace RoundModifiers.Modifiers.Scp1507;

public class Flamingos : Modifier
{
    public static Scp1507Role Scp1507Role { get; set; }
    public bool HasFlamingoed { get; set; }
    
    public CoroutineHandle FlamingoCoroutine { get; set; }
    public CoroutineHandle TapeRecorderCoroutine { get; set; }
    
    public Dictionary<Player, int> RespawnAttempts { get; set; }

    public void OnChargingJailbird(ChargingJailbirdEventArgs ev)
    {
        if (Scp1507Role.Check(ev.Player))
        {
            ev.IsAllowed = false;
            return;
        }
    }

    public void OnPickingUpItem(PickingUpItemEventArgs ev)
    {
        if(Scp1507Role.Check(ev.Player))
            if(ev.Pickup.Type != ItemType.Jailbird)
                ev.IsAllowed = false;
    }

    public void OnSwing(SwingingEventArgs ev)
    {
        if(!Scp1507Role.Check(ev.Player)) return;
        Door door = Door.GetClosest(ev.Player.Position, out float distance);
        if (door == null || distance > 3) return;
        Log.Debug("Found door!");
        
        if (door is CheckpointDoor checkpointDoor)
        {
            checkpointDoor.CurrentStage = Interactables.Interobjects.CheckpointDoor.CheckpointSequenceStage.Granted;
        }
        else if (door is BreakableDoor damagable)
        {
            if (!damagable.IsDestroyed)
            {
                Log.Debug("Damaging door!");
                damagable.Damage(50);
                ev.Player.ShowHitMarker();
            }
        } else if (door is Gate gate)
        {
            if (gate.IsFullyClosed)
            {
                Log.Debug("Opening gate!");
                gate.IsOpen = true;
                ev.Player.ShowHitMarker();
            }
        }

        foreach (Ragdoll ragdoll in Ragdoll.Get(Player.List.Where(p=>p.IsDead)))
        {
            if(ragdoll.Owner.IsAlive) continue;
            if(ragdoll.Role != RoleTypeId.Tutorial) continue;
            if (Vector3.Distance(ragdoll.Position, ev.Player.Position) < 3)
            {
                Log.Debug("Found ragdoll!");
                if(RespawnAttempts.ContainsKey(ragdoll.Owner))
                {
                    RespawnAttempts[ragdoll.Owner]++;
                }
                else
                {
                    RespawnAttempts[ragdoll.Owner] = 1;
                }

                if (RespawnAttempts[ragdoll.Owner] < 10)
                {
                    ev.Player.ShowHUDHint("Attempting to respawn player...");
                    continue;//Change to break to only do 1 flamingo at a time
                }
                ragdoll.Owner.RoleManager.ServerSetRole(RoleTypeId.Tutorial, RoleChangeReason.Revived,RoleSpawnFlags.None);
                Scp1507Role.AddRole(ragdoll.Owner);
                ragdoll.Destroy();
            }
        }
        
        if(ev.Item is Jailbird jb)
        {
            jb.TotalCharges += 2;
        }
        
    }

    public void OnDying(DyingEventArgs ev)
    {
        if (ev.Player.Role == RoleTypeId.Tutorial)
        {
            ev.Player.ClearInventory();
            ev.ItemsToDrop.Clear();
        }
    }

    public void OnUsingScp1576(UsingItemEventArgs ev)
    {
        if(ev.Item.Type != ItemType.SCP1576) return;
        if(HasFlamingoed) return;
        float spectatorCount = Player.List.Count(player => player.Role == RoleTypeId.Spectator);
        float playerCount = Player.List.Count;
        if (spectatorCount/playerCount >= 0.3f)
        {
            //Warn player they're about to use the tape recorder
            ev.Player.ShowHUDHint("You hear the sound of flamingos in the distance.");
        }
        else
        {
            //Tell player they cannot use this item yet
            Log.Info("Player tried to use SCP-1576 but there are not enough spectators. "+spectatorCount+"/"+playerCount+" players are spectators.");
            ev.IsAllowed = false;
            ev.Player.ShowHUDHint("The handle does not turn, it seems to be waiting for something. Try again later.");
        }
    }
    
    public void FinishUsingScp1576(UsingItemCompletedEventArgs ev)
    {
        if(ev.Item.Type != ItemType.SCP1576) return;
        if(HasFlamingoed) return;
        ev.IsAllowed = false;
        
        Scp1507Role.AddRole(ev.Player);
        foreach (Player p in Player.Get(p => p.Role == RoleTypeId.Spectator))
        {
            Scp1507Role.AddRole(p);
            p.Position = ev.Player.Position;
        }
        Cassie.Message(".g7", false, false, false);
        HasFlamingoed = true;
        FlamingoCoroutine = Timing.RunCoroutine(OnFlamingoTick());
    }
    
    public void OnInspectWeapon(InspectFirearmEventArgs ev)
    {
        if (Scp1507Role.Check(ev.Player)) return;
        //Scp1507Role.AddRole(ev.Player);
    }

    public IEnumerator<float> OnFlamingoTick()
    {
        while(Scp1507Role.TrackedPlayers.Count > 0)
        {
            foreach (var player in Scp1507Role.TrackedPlayers)
            {
                int nearbyPlayers = Scp1507Role.TrackedPlayers
                    .Count(p => p!=player && Vector3.Distance(p.Position,player.Position) < 5f);
                if(nearbyPlayers > 1)
                    player.Heal(nearbyPlayers);
                yield return Timing.WaitForOneFrame;
            }

            yield return Timing.WaitForSeconds((float)(1f-(Scp1507Role.TrackedPlayers.Count/Server.Tps)));
        }
    }
    
    public void OnChangeRole(ChangingRoleEventArgs ev)
    {
        if (Scp1507Role.Check(ev.Player) || ev.NewRole == RoleTypeId.Tutorial)
        {
            Timing.CallDelayed(0.5f, () =>
            {
                ev.Player.AddItem(ItemType.Jailbird);
                ev.Player.CurrentItem = ev.Player.Items.First(i => i.Type == ItemType.Jailbird);
                ev.Player.VoiceChannel = VoiceChatChannel.RoundSummary;
            });
        }

        // Patch to remove role when player dies
        if (Scp1507Role.Check(ev.Player) && ev.NewRole == RoleTypeId.Spectator)
        {
            Scp1507Role.RemoveRole(ev.Player);
            ev.Player.Scale = Vector3.one;
        }
    }
    
    public void OnRoundStart()
    {
        TapeRecorderCoroutine = Timing.CallDelayed(360f, () =>
        {
            if(HasFlamingoed) return;
            Room room = Room.Random(ZoneType.Entrance);
            Pickup.CreateAndSpawn(ItemType.SCP1576, room.Position + Vector3.up, room.Rotation);
        });
    }
    
    public void OnChangingItem(ChangingItemEventArgs ev)
    {
        if (Scp1507Role.Check(ev.Player) && ev.Item.Type != ItemType.Jailbird)
        {
            ev.IsAllowed = false;
        }
    }

    public void OnInteractDoor(InteractingDoorEventArgs ev)
    {
        if (Scp1507Role.Check(ev.Player))
        {
            ev.IsAllowed = false;
            return;
        }

        
        //Log.Debug("Interacting with door! "+ev.Door.Name);
    }

    public void OnShot(ShotEventArgs ev)
    {
        
    }

    public void OnWaitingForPlayers()
    {
        
    }

    public void OnEndingRound(EndingRoundEventArgs ev)
    {
        if(Player.List.Count(p => p.IsAlive && p.Role!=RoleTypeId.Tutorial) == 0)
        {
            ev.IsAllowed = true;
            ev.LeadingTeam = LeadingTeam.Draw;
        }
        if(Player.List.Count(p => p.Role == RoleTypeId.Tutorial) > 0) //Stop round from ending if there are still flamingos
        {
            ev.IsAllowed = false;
        }
    }

    
    // Attempting to allow flamingos to voice chat with each other
    public void OnVoiceChatting(VoiceChattingEventArgs ev)
    {
        if(ev.Player.Role != RoleTypeId.Tutorial) return;
        if(ev.VoiceMessage.Channel == VoiceChatChannel.RoundSummary) return;
        ev.IsAllowed = false;
        ev.VoiceMessage = ev.VoiceMessage with { Channel = VoiceChatChannel.RoundSummary };
        foreach(Player p in Player.List.Where(p=>p.Role == RoleTypeId.Tutorial))
            p.ReferenceHub.connectionToClient.Send(ev.VoiceMessage);
        
    }
    
    


    protected override void RegisterModifier()
    {
        Scp1507Role = new Scp1507Role();
        Scp1507Role.Register();
        
        HasFlamingoed = false;
        RespawnAttempts = DictionaryPool<Player, int>.Pool.Get();
        
        
        
        Exiled.Events.Handlers.Item.ChargingJailbird += OnChargingJailbird;
        Exiled.Events.Handlers.Item.Swinging += OnSwing;
        Exiled.Events.Handlers.Player.ChangingItem += OnChangingItem;
        TTCore.Events.Handlers.Custom.InspectFirearm += OnInspectWeapon;
        Exiled.Events.Handlers.Player.InteractingDoor += OnInteractDoor;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Player.Shot += OnShot;
        Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        Exiled.Events.Handlers.Player.ChangingRole += OnChangeRole;
        Exiled.Events.Handlers.Player.Dying += OnDying;
        Exiled.Events.Handlers.Player.UsingItem += OnUsingScp1576;
        Exiled.Events.Handlers.Player.UsingItemCompleted += FinishUsingScp1576;
        Exiled.Events.Handlers.Player.PickingUpItem += OnPickingUpItem;
        Exiled.Events.Handlers.Server.EndingRound += OnEndingRound;
        Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
        
    }

    protected override void UnregisterModifier()
    {
        Scp1507Role.Unregister();
        
        HasFlamingoed = false;
        
        Exiled.Events.Handlers.Item.ChargingJailbird -= OnChargingJailbird;
        Exiled.Events.Handlers.Item.Swinging -= OnSwing;
        Exiled.Events.Handlers.Player.ChangingItem -= OnChangingItem;
        TTCore.Events.Handlers.Custom.InspectFirearm -= OnInspectWeapon;
        Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractDoor;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Player.Shot -= OnShot;
        Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        Exiled.Events.Handlers.Player.ChangingRole -= OnChangeRole;
        Exiled.Events.Handlers.Player.Dying -= OnDying;
        Exiled.Events.Handlers.Player.UsingItem -= OnUsingScp1576;
        Exiled.Events.Handlers.Player.UsingItemCompleted -= FinishUsingScp1576;
        Exiled.Events.Handlers.Player.PickingUpItem -= OnPickingUpItem;
        Exiled.Events.Handlers.Server.EndingRound -= OnEndingRound;
        Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;

        Timing.KillCoroutines(FlamingoCoroutine);
        Timing.KillCoroutines(TapeRecorderCoroutine);
        
        DictionaryPool<Player, int>.Pool.Return(RespawnAttempts);
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "Flamingos",
        Aliases = new [] {"scp1507","1507"},
        Description = "Flamingos!",
        FormattedName = "<color=pink>Flamingos</color>",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false,
        Hidden = true,
        Category = Category.CustomRole
    };
}