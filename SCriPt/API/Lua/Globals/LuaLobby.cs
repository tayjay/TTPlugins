using GameCore;
using MoonSharp.Interpreter;

namespace SCriPt.API.Lua.Globals
{
    [MoonSharpUserData]
    public class LuaLobby
    {
        public static void Lock()
        {
            RoundStart.LobbyLock = true;
        }
        
        public static void Unlock()
        {
            RoundStart.LobbyLock = false;
        }
    }
}