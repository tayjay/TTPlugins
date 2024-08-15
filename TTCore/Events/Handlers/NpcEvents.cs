using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Permissions.Commands.Permissions.Group;
using HarmonyLib;
using MapGeneration;
using TTCore.API;
using TTCore.Npcs.AI.Core.Management;
using TTCore.Npcs.AI.Pathing;
using TTCore.Utilities;
using UnityEngine;

namespace TTCore.Events.Handlers;

public class NpcEvents : IRegistered
{
    public void OnGenerated()
    {
        if(!TTCore.Instance.Config.EnableAi)
            return;
        NavMeshManager.InitializeMap();

        foreach (Exiled.API.Features.Room room in Exiled.API.Features.Room.List)
        {
            switch (room.Identifier.Name)
            {
                case RoomName.LczCheckpointA:
                case RoomName.LczCheckpointB:
                case RoomName.HczCheckpointA:
                case RoomName.HczCheckpointB:
                case RoomName.EzGateA:
                case RoomName.EzGateB:
                    Add(room);
                    break;
            }
        }
    }

    static void Add(Exiled.API.Features.Room room)
    {
        if(NpcUtilities.ZoneElevatorRooms.ContainsKey(room.Zone))
            NpcUtilities.ZoneElevatorRooms[room.Zone].AddItem(room);
        else
            NpcUtilities.ZoneElevatorRooms.Add(room.Zone,[room]);
    }

    public void OnHurt(HurtingEventArgs ev)
    {
        //stop Falling damage
        if (ev.Player.TryGetAI(out AIPlayerProfile prof))
        {
            prof.WorldPlayer.Damage(ev.Attacker);

            if (ev.DamageHandler.Type == DamageType.Falldown)
            {
                ev.IsAllowed = false;
                ev.Amount = 0f;
            } else if (ev.DamageHandler.Type == DamageType.Tesla)
            {
                ev.IsAllowed = false;
                ev.Amount = 0;
            }
        }
    }
    
    public void OnDying(DyingEventArgs ev)
    {
        //rotate on death?
        if(ev.Player.TryGetAI(out AIPlayerProfile prof))
            prof.ReferenceHub.transform.eulerAngles = new Vector3(0f, prof.ReferenceHub.transform.eulerAngles.y, 0f);
    }

    public void OnChangeRole(ChangingRoleEventArgs ev)
    {
        if(ev.Player.TryGetAI(out AIPlayerProfile prof))
            prof.WorldPlayer.RoleChange(ev.NewRole);
    }

    public void OnRestart()
    {
        //Kick all NPCs, already done by EXILED
    }

    public void Register()
    {
        Exiled.Events.Handlers.Map.Generated += OnGenerated;
        Exiled.Events.Handlers.Player.Hurting += OnHurt;
        Exiled.Events.Handlers.Player.Dying += OnDying;
        Exiled.Events.Handlers.Player.ChangingRole += OnChangeRole;
        Exiled.Events.Handlers.Server.RestartingRound += OnRestart;
    }

    public void Unregister()
    {
        Exiled.Events.Handlers.Map.Generated -= OnGenerated;
        Exiled.Events.Handlers.Player.Hurting -= OnHurt;
        Exiled.Events.Handlers.Player.Dying -= OnDying;
        Exiled.Events.Handlers.Player.ChangingRole -= OnChangeRole;
        Exiled.Events.Handlers.Server.RestartingRound -= OnRestart;
    }
}