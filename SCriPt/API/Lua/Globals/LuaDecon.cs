using LightContainmentZoneDecontamination;
using MoonSharp.Interpreter;

namespace SCriPt.API.Lua.Globals
{
    [MoonSharpUserData]
    public class LuaDecon
    {
        public static void Disable()
        {
            DecontaminationController.Singleton.NetworkDecontaminationOverride =
                DecontaminationController.DecontaminationStatus.Disabled;
        }
        
        public static void Force()
        {
            DecontaminationController.Singleton.ForceDecontamination();
        }
    }
}