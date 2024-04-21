using MoonSharp.Interpreter;
using SCriPt.Handlers;

namespace SCriPt.API.Lua.Globals
{
    //[MoonSharpUserData]
    public class LuaEvents
    {
        public static PlayerEvents Player => SCriPt.Instance.PlayerEvents;
        public static ServerEvents Server => SCriPt.Instance.ServerEvents;
        public static WarheadEvents Warhead => SCriPt.Instance.WarheadEvents;
        public static ItemEvents Item => SCriPt.Instance.ItemEvents;
        public static MapEvents Map => SCriPt.Instance.MapEvents;
        public static Scp049Events Scp049 => SCriPt.Instance.Scp049Events;
        public static Scp049Events Doctor => SCriPt.Instance.Scp049Events;
        public static Scp0492Events Scp0492 => SCriPt.Instance.Scp0492Events;
        public static Scp0492Events Zombie => SCriPt.Instance.Scp0492Events;
        public static Scp079Events Scp079 => SCriPt.Instance.Scp079Events;
        public static Scp079Events Computer => SCriPt.Instance.Scp079Events;
        public static Scp079Events Camera => SCriPt.Instance.Scp079Events;
        public static Scp096Events Scp096 => SCriPt.Instance.Scp096Events;
        public static Scp096Events ShyGuy => SCriPt.Instance.Scp096Events;
        public static Scp106Events Scp106 => SCriPt.Instance.Scp106Events;
        public static Scp106Events Larry => SCriPt.Instance.Scp106Events;
        public static Scp173Events Scp173 => SCriPt.Instance.Scp173Events;
        public static Scp173Events Peanut => SCriPt.Instance.Scp173Events;
        public static Scp244Events Scp244 => SCriPt.Instance.Scp244Events;
        public static Scp244Events Vase => SCriPt.Instance.Scp244Events;
        public static Scp330Events Scp330 => SCriPt.Instance.Scp330Events;
        public static Scp330Events Candy => SCriPt.Instance.Scp330Events;
        public static Scp914Events Scp914 => SCriPt.Instance.Scp914Events;
        public static Scp939Events Scp939 => SCriPt.Instance.Scp939Events;
        public static Scp939Events Dog => SCriPt.Instance.Scp939Events;
        
        public static Scp3114Events Scp3114 => SCriPt.Instance.Scp3114Events;
        public static Scp3114Events Skeleton => SCriPt.Instance.Scp3114Events;
    }
}