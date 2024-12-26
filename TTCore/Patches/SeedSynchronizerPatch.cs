using HarmonyLib;
using MapGeneration;
using TTCore.Events.Handlers;

namespace TTCore.Patches;

[HarmonyPatch(typeof(SeedSynchronizer),"GenerateLevel")]
public class SeedSynchronizerPatch
{
    [HarmonyPrefix]
    public static void Prefix(SeedSynchronizer __instance)
    {
        Custom.OnPreGenerateMap();
    }
}