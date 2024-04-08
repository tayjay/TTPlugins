using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using MoonSharp.Interpreter;

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
    }
}