using Exiled.API.Enums;
using Exiled.API.Features;
using TTCore.Utilities;
using UnityEngine;

namespace TTCore.API;

public class RoomPosition
{
    private Room internalRoom;
    private RoomType internalRoomType;

    
    public Vector3 position { get; set; }
    public Quaternion rotation { get; set; }
    
    public RoomPosition(Room room, Vector3 position, Quaternion rotation)
    {
        this.internalRoom = room;
        this.position = position;
        this.rotation = rotation;
    }
        
    public RoomPosition(Room room, Vector3 position) : this(room, position, Quaternion.Euler(0,0,0))
    {
    }
    
    public RoomPosition(RoomType roomType, Vector3 position, Quaternion rotation)
    {
        this.internalRoomType = roomType;
        this.position = position;
        this.rotation = rotation;
    }
        
    public RoomPosition(RoomType roomType, Vector3 position) : this(roomType, position, Quaternion.Euler(0,0,0))
    {
    }
        
    public RoomPosition(Room room) : this(room, room.Position)
    {
    }
        
    public Room Room
    {
        get
        {
            if (internalRoom == null)
            {
                internalRoom = Room.Get(internalRoomType);
            }
            return internalRoom;
        }
    }
    
    public Vector3 GlobalPosition => TransformUtils.CalculateGlobalPosition(position, Room.Position, Room.Rotation);

    public Quaternion GlobalRotation => rotation * Room.Rotation;

}