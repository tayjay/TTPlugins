using HarmonyLib;
using PlayerRoles.PlayableScps.Scp079;
using TTCore.Events.Handlers;

namespace TTCore.Patches;

[HarmonyPatch(typeof(Scp079Recontainer),"BeginOvercharge")]
public class OverchargePatch
{
    [HarmonyPostfix]
    public static void Postfix(Scp079Recontainer __instance)
    {
        //Log.Debug("OverchargePatch.Postfix");
        Custom.OnScp079BeginOvercharge();
    }
}