using System;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using TTCore.API;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;
using UnityEngine;

namespace TTCore.Components;

public class RoomTriggerHandler : MonoBehaviour
{
    private bool initialized =false;
    public Room Room { get; private set; }

    public void Init(Room room)
    {
        Room = room;
        initialized = true;
        
        BoxCollider collider = gameObject.AddComponent<BoxCollider>();
        collider.isTrigger = true;
        collider.size = new Vector3(15,100,15); // RoomIdUtils Grid Scale

        if (Room.Zone == ZoneType.Surface)
        {
            collider.size = new Vector3(100,100,100); // Surface is big
        }
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.TryGetComponent<ReferenceHub>(out ReferenceHub rh))
        {
            Log.Debug("Player "+rh.nicknameSync._firstNickname+" entered room "+Room.Name);
            RoomTrigger.OnEnterRoom(new EnterRoomEventArgs(Room, rh));
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.TryGetComponent<ReferenceHub>(out ReferenceHub rh))
        {
            Log.Debug("Player "+rh.nicknameSync._firstNickname+" exited room "+Room.Name);
            RoomTrigger.OnExitRoom(new ExitRoomEventArgs(Room, rh));
        }
    }
    
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.TryGetComponent<ReferenceHub>(out ReferenceHub rh))
        {
            RoomTrigger.OnStayRoom(new StayRoomEventArgs(Room, rh));
        }
    }

    public static void OnGenerated()
    {
        Timing.CallDelayed(3f, () =>
        {
            foreach (Room room in Room.List)
            {
                if(room== null)
                {
                    Log.Error("Room is null!");
                    continue;
                }
                Log.Debug("Adding room trigger to "+room.Name);
                try
                {
                    room.gameObject.AddComponent<RoomTriggerHandler>().Init(room);
                } catch (Exception e)
                {
                    Log.Error("Error adding room trigger to "+room.Name+": "+e);
                }
            }
        });

    }

    public static void Register()
    {
        Exiled.Events.Handlers.Map.Generated += OnGenerated;
    }

    public static void Unregister()
    {
        Exiled.Events.Handlers.Map.Generated -= OnGenerated;
    }
}