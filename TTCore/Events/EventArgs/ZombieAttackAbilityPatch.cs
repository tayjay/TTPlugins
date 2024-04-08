using Exiled.API.Features;
using HarmonyLib;
using PlayerRoles.PlayableScps.Scp049.Zombies;
using TTCore.Events.Handlers;

namespace TTCore.Events.EventArgs;

[HarmonyPatch(typeof(ZombieAttackAbility), "DamagePlayers")]
public class ZombieAttackAbilityPatch
{
    public static bool Prefix(ZombieAttackAbility __instance)
    {
        Log.Debug("ZombieAttackAbility::DamagePlayers");
        ZombieAttackEventArgs ev = new ZombieAttackEventArgs(__instance.CastRole, __instance.Owner);
        Custom.OnZombieAttack(ev);
        return ev.IsAllowed;
    }
}