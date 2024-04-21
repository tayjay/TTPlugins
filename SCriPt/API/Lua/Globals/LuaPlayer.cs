using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Pickups;
using MoonSharp.Interpreter;
using UnityEngine;

namespace SCriPt.API.Lua.Globals
{
    [MoonSharpUserData]
    public class LuaPlayer
    {
        public static Player Get(int id)
        {
            return Player.Get(id);
        }
        
        public static Player Get(string name)
        {
            return Player.Get(name);
        }
        
        public static List<Player> List => Player.List.ToList();
        
        public static int Count => Player.List.Count;
        
        public static Player GetClosestTo(Door door)
        {
            return GetClosestTo(door.Position);
        }
        
        public static Player GetRandom()
        {
            return Player.List.GetRandomValue();
        }
        
        public static Player GetClosestTo(Pickup pickup)
        {
            return GetClosestTo(pickup.Position);
        }
        
        public static Player GetClosestTo(Player player)
        {
            //Use a predicate on the Player.List to get the closest player to the given player.Position
            return GetClosestTo(player.Position);
        }
        
        public static Player GetClosestTo(Vector3 position)
        {
            //Use a predicate on the Player.List to get the closest player to the given position
            return Player.List.OrderBy(p => Vector3.Distance(p.Position,position)).FirstOrDefault();
        }
        
        
        
    }
}