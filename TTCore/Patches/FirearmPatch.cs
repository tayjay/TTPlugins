using Exiled.API.Features;
using HarmonyLib;
using InventorySystem.Items.Firearms;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;

namespace TTCore.Patches;

/*public class FirearmPatch
{
    [HarmonyPatch(typeof(AutomaticFirearm), "BaseStats", MethodType.Getter)]
    public class AutomaticFirearmPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(AutomaticFirearm __instance, ref FirearmBaseStats __result)
        {
            //Log.Debug("AutomaticFirearmPatch.Prefix");
            AccessFirearmBaseStatsEventArgs args = new AccessFirearmBaseStatsEventArgs(__instance.ItemSerial, __instance._stats);
            Custom.OnAccessFirearmBaseStats(args);
            
            __result = args.BaseStats;
            
            return false;
        }
    }
    
    [HarmonyPatch(typeof(Revolver), "BaseStats", MethodType.Getter)]
    public class RevolverPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Revolver __instance, ref FirearmBaseStats __result)
        {
            //Log.Debug("RevolverPatch.Prefix");
            AccessFirearmBaseStatsEventArgs args = new AccessFirearmBaseStatsEventArgs(__instance.ItemSerial, __instance._stats);
            Custom.OnAccessFirearmBaseStats(args);
            
            __result = args.BaseStats;
            
            return false;
        }
    }
    
    [HarmonyPatch(typeof(Shotgun), "BaseStats", MethodType.Getter)]
    public class ShotgunPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(Shotgun __instance, ref FirearmBaseStats __result)
        {
            //Log.Debug("ShotgunPatch.Prefix");
            AccessFirearmBaseStatsEventArgs args = new AccessFirearmBaseStatsEventArgs(__instance.ItemSerial, __instance._stats);
            Custom.OnAccessFirearmBaseStats(args);
            
            __result = args.BaseStats;
            
            return false;
        }
    }
    
    [HarmonyPatch(typeof(ParticleDisruptor), "BaseStats", MethodType.Getter)]
    public class ParticleDisruptorPatch
    {
        [HarmonyPrefix]
        public static bool Prefix(ParticleDisruptor __instance, ref FirearmBaseStats __result)
        {
            //Log.Debug("ParticleDisruptorPatch.Prefix");
            AccessFirearmBaseStatsEventArgs args = new AccessFirearmBaseStatsEventArgs(__instance.ItemSerial, __instance._stats);
            Custom.OnAccessFirearmBaseStats(args);
            
            __result = args.BaseStats;
            
            return false;
        }
    }
}*/