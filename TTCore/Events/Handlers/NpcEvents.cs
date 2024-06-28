using Exiled.API.Features;
using Exiled.Permissions.Commands.Permissions.Group;
using HarmonyLib;
using MapGeneration;
using TTCore.API;
using TTCore.Npcs.AI.Pathing;
using TTCore.Utilities;

namespace TTCore.Events.Handlers;

public class NpcEvents : IRegistered
{
    public void OnGenerated()
    {
        if(!TTCore.Instance.Config.EnableAi)
            return;
        NavMeshManager.InitializeMap();

        foreach (Room room in Room.List)
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

    static void Add(Room room)
    {
        if(NpcUtilities.ZoneElevatorRooms.ContainsKey(room.Zone))
            NpcUtilities.ZoneElevatorRooms[room.Zone].AddItem(room);
        else
            NpcUtilities.ZoneElevatorRooms.Add(room.Zone,[room]);
    }

    public void Register()
    {
        Exiled.Events.Handlers.Map.Generated += OnGenerated;
    }

    public void Unregister()
    {
        Exiled.Events.Handlers.Map.Generated -= OnGenerated;
    }
}