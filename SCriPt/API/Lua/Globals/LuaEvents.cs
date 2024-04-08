using MoonSharp.Interpreter;
using SCriPt.Handlers;

namespace SCriPt.API.Lua.Globals
{
    [MoonSharpUserData]
    public class LuaEvents
    {
        public static PlayerEvents Player => SCriPt.Instance.PlayerEvents;
        public static ServerEvents Server => SCriPt.Instance.ServerEvents;
        public static WarheadEvents Warhead => SCriPt.Instance.WarheadEvents;
        public static ItemEvents Item => SCriPt.Instance.ItemEvents;
        public static MapEvents Map => SCriPt.Instance.MapEvents;
        public static Scp049Events Scp049 => SCriPt.Instance.Scp049Events;
        public static Scp3114Events Scp3114 => SCriPt.Instance.Scp3114Events;
    }
}