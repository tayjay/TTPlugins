using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using MapGeneration;
using UnityEngine;

namespace TTAdmin.Data;

public class RoomData
{
    public string Name { get; set; }
    //public RoomIdentifier Identifier { get; set; }
    public string Type { get; set; }
    public string Zone { get; set; }
    public string RoomShape { get; set; }
    public List<DoorData> Doors { get; set; }
    public List<PlayerData> Players { get; set; }
    //public List<LiftData> Lifts { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public Color LightColor { get; set; }
    
    
    
    
    public RoomData(Room room)
    {
        Name = room.Name;
        Type = room.Type.ToString();
        Zone = room.Zone.ToString();
        Doors = DoorData.ConvertList(room.Doors.ToList());
        Players = PlayerData.ConvertList(room.Players.ToList());
        Rotation = room.Transform.rotation;
        Position = room.Transform.position;
        RoomShape = room.RoomShape.ToString();
        LightColor = room.Color;
    }
    
    public static List<RoomData> ConvertList(List<Room> rooms)
    {
        List<RoomData> roomData = new List<RoomData>();
        foreach (var room in rooms)
        {
            roomData.Add(new RoomData(room));
        }
        return roomData;
    }
    
    public static RoomData ByName(string name)
    {
        Room room = Room.Get(k => k.Name == name).First();
        return new RoomData(room);
    }
    
    
    
}