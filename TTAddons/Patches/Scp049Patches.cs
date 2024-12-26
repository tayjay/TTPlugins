using Exiled.API.Features;
using HarmonyLib;
using Mirror;
using PlayerRoles;
using PlayerRoles.PlayableScps.HumeShield;
using PlayerRoles.PlayableScps.Scp049;
using PlayerRoles.Ragdolls;
using PlayerStatsSystem;
using PluginAPI.Events;
using UnityEngine;

namespace TTAddons.Patches
{
    public class Scp049Patches
    {
        public static bool ShouldReplaceScp049ResurrectAbility => TTAddons.Instance.Config.ReplaceScp049ResurrectLogic;
        public static int MaxResurrections => TTAddons.Instance.Config.Scp049MaxResurrections;
        public static float TargetCorpseDuration => TTAddons.Instance.Config.Scp049TargetCorpseDuration;
        public static float HumanCorpseDuration => TTAddons.Instance.Config.Scp049HumanCorpseDuration;
        public static float ResurrectTargetReward => TTAddons.Instance.Config.Scp049ResurrectTargetReward;
        
        //[HarmonyPatch(typeof(Scp049ResurrectAbility), "CheckMaxResurrections", MethodType.Normal)]
        public class Scp049ResurrectAbility_CheckMaxResurrections
        {
            //[HarmonyPrefix]
            public static bool Prefix(Scp049ResurrectAbility __instance, ref ReferenceHub owner, ref Scp049ResurrectAbility.ResurrectError __result)
            {
                if(!ShouldReplaceScp049ResurrectAbility) return true;
                //Log.Info("Scp049ResurrectAbility_CheckMaxResurrections");
                int resurrectionsNumber = Scp049ResurrectAbility.GetResurrectionsNumber(owner);
                if (resurrectionsNumber < MaxResurrections)
                {
                    __result = Scp049ResurrectAbility.ResurrectError.None;
                    return false;
                }
                __result = resurrectionsNumber <= MaxResurrections ? Scp049ResurrectAbility.ResurrectError.MaxReached : Scp049ResurrectAbility.ResurrectError.Refused;
                return false;
            }
        }
        
        //[HarmonyPatch(typeof(Scp049ResurrectAbility), "CheckBeginConditions", MethodType.Normal)]
        public class Scp049ResurrectAbility_CheckBeginConditions
        {
            //[HarmonyPrefix]
            public static bool Prefix(Scp049ResurrectAbility __instance, ref BasicRagdoll ragdoll, ref Scp049ResurrectAbility.ResurrectError __result)
            {
                if(!ShouldReplaceScp049ResurrectAbility) return true;
                //Log.Info("Scp049ResurrectAbility_CheckBeginConditions");
                ReferenceHub ownerHub = ragdoll.Info.OwnerHub;
                bool flag = ownerHub == (ReferenceHub) null;
                if (ragdoll.Info.RoleType == RoleTypeId.Scp0492)
                {
                    if (flag || !Scp049ResurrectAbility.DeadZombies.Contains(ownerHub.netId))
                    {
                        __result = Scp049ResurrectAbility.ResurrectError.TargetNull;
                        return false;
                    }
                    if (!(ragdoll.Info.Handler is AttackerDamageHandler))
                    {
                        __result = Scp049ResurrectAbility.ResurrectError.TargetInvalid;
                        return false;
                    }
                }
                else
                {
                    float num = (flag ? 0 : (__instance._senseAbility.DeadTargets.Contains(ownerHub) ? 1 : 0)) != 0 ? TargetCorpseDuration : HumanCorpseDuration;
                    if ((double)ragdoll.Info.ExistenceTime > (double)num)
                    {
                        __result = Scp049ResurrectAbility.ResurrectError.Expired;
                        return false;
                    }

                    if (!ragdoll.Info.RoleType.IsHuman())
                    {
                        __result = Scp049ResurrectAbility.ResurrectError.TargetInvalid;
                        return false;
                    }
                        
                    if (flag || __instance.AnyConflicts(ragdoll))
                    {
                        __result = Scp049ResurrectAbility.ResurrectError.TargetNull;
                        return false;
                    }
                    if (!Scp049ResurrectAbility.IsSpawnableSpectator(ownerHub))
                    {
                        __result = Scp049ResurrectAbility.ResurrectError.TargetInvalid;
                        return false;
                    }
                }
                __result = __instance.CheckMaxResurrections(ownerHub);
                return false;
            }
        }
        
        //[HarmonyPatch(typeof(Scp049ResurrectAbility), "ServerComplete", MethodType.Normal)]
        public class Scp049ResurrectAbility_ServerComplete
        {
            //[HarmonyPrefix]
            public static bool Prefix(Scp049ResurrectAbility __instance)
            {
                if(!ShouldReplaceScp049ResurrectAbility) return true;
                //Log.Info("Scp049ResurrectAbility_ServerComplete");
                ReferenceHub ownerHub = __instance.CurRagdoll.Info.OwnerHub;
                if (!EventManager.ExecuteEvent((IEventArguments) new Scp049ResurrectBodyEvent(__instance.Owner, ownerHub, __instance.CurRagdoll)))
                    return false;
                ownerHub.transform.position = __instance.CastRole.FpcModule.Position;
                if (__instance._senseAbility.DeadTargets.Contains(ownerHub))
                {
                    HumeShieldModuleBase humeShieldModule = __instance.CastRole.HumeShieldModule;
                    humeShieldModule.HsCurrent = Mathf.Min(humeShieldModule.HsCurrent + ResurrectTargetReward, humeShieldModule.HsMax);
                }
                ownerHub.roleManager.ServerSetRole(RoleTypeId.Scp0492, RoleChangeReason.Revived);
                NetworkServer.Destroy(__instance.CurRagdoll.gameObject);
                return false;
            }
        }
    }
}