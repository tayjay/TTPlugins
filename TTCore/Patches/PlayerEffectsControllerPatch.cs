using Exiled.API.Features;
using HarmonyLib;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;

namespace TTCore.Patches;

[HarmonyPatch(typeof(PlayerEffectsController), "Awake")]
public class PlayerEffectsControllerPatch
{
    [HarmonyPrefix]
    public static void Prefix(PlayerEffectsController __instance)
    {
        Log.Debug("PlayerEffectsControllerPatch.Prefix");
        PlayerEffectsAwakeArgs args = new PlayerEffectsAwakeArgs(__instance);
        Custom.OnPlayerEffectsAwake(args);
    }
    
}