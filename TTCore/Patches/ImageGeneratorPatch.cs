using HarmonyLib;
using MapGeneration;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;

/*namespace TTCore.Patches;
[HarmonyPatch(typeof(ImageGenerator),"GenerateMap")]
public class ImageGeneratorPatch
{
    [HarmonyPrefix]
    public static void Prefix(ImageGenerator __instance,int seed, string newAlias)
    {
        Custom.OnPreGenerateZone(new PreGenerateZoneEventArgs(__instance,seed,newAlias));
    }
}*/