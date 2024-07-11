using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features.Doors;
using Interactables.Interobjects.DoorUtils;
using UnityEngine;

namespace TTAdmin.Data;

public class DoorData
{
    public string Name { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    public bool IsOpen { get; set; }
    public bool IsLocked { get; set; }
    public bool IsDestroyed { get; set; }
    public bool AllowsScp106 { get; set; }
    public List<string> Rooms { get; set; }
    
    public int Id { get; set; }
    
    
    public DoorData(Door door)
    {
        Name = door.Name;
        Position = door.Position;
        Rotation = door.Rotation;
        IsOpen = door.IsOpen;
        IsLocked = door.IsLocked;
        IsDestroyed = (door is BreakableDoor breakableDoor) && breakableDoor.IsDestroyed;
        AllowsScp106 = door.AllowsScp106;
        Rooms = new List<string>();
        foreach (var room in door.Rooms)
        {
            Rooms.Add(room.Name);
        }

        Id = DoorsById.Where(pair => pair.Value == door).Select(pair => pair.Key).FirstOrDefault();
    }
    
    public static List<DoorData> ConvertList(List<Door> doors)
    {
        List<DoorData> doorData = new List<DoorData>();
        foreach (var door in doors)
        {
            doorData.Add(new DoorData(door));
        }
        return doorData;
    }
    
    public static DoorData ByName(string name)
    {
        Door door = Door.Get(name);
        return new DoorData(door);
    }
    
    public static Dictionary<int,Door> DoorsById { get; set; }
    
    public static DoorData DataById(int id)
    {
        if(DoorsById.TryGetValue(id, out var door))
            return new DoorData(door);
        return null;
    }
    
    public static Door DoorById(int id)
    {
        if(DoorsById.TryGetValue(id, out var byId))
            return byId;
        return null;
    }
}