using Exiled.API.Features;
using MoonSharp.Interpreter;

namespace SCriPt.API.Lua.Globals
{
    [MoonSharpUserData]
    public class LuaRound
    {
        public static void Lock()
        {
            RoundSummary.RoundLock = true;
        }
        
        public static void Unlock()
        {
            RoundSummary.RoundLock = false;
        }
        
        public static void Start()
        {
            RoundSummary.singleton.Start();
        }
        
        public static void Restart()
        {
            LuaServer.Restart();
        }

        public static bool InProgress => RoundSummary.RoundInProgress();
    }
}