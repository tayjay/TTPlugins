using Exiled.API.Features.Roles;
using Exiled.Events.Handlers;
using HarmonyLib;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.PlayableScps.Scp049.Zombies;
using PlayerRoles.PlayableScps.Subroutines;

namespace TTCore.Patches;

//[HarmonyPatch(typeof(ScpAttackAbilityBase<>), "ServerPerformAttack")]
public class ScpAttackAbilityBasePatch
{
    //[HarmonyPrefix]
    public static void ZombiePrefix(ScpAttackAbilityBase<ZombieRole> __instance)
    {
        
    }
    
}