using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Permissions.Extensions;
using InventorySystem.Items.Firearms.Modules;
using PlayerRoles.Ragdolls;
using UnityEngine;

namespace TTCore.Extensions
{
    public static class PlayerExtensions
    {
        public static bool TryGetNpcOnSight(this Player player, float maxDistance, out Npc npc)
        {
            npc = null;

            if (!Physics.Raycast(new Ray(player.ReferenceHub.PlayerCameraReference.position + player.GameObject.transform.forward * 0.3f, player.ReferenceHub.PlayerCameraReference.forward), out RaycastHit hit, maxDistance, LayerMask.GetMask("Player")))
                return false;

            npc = Npc.Get((hit.transform.gameObject));

            return npc != null;
        }
        
        public static bool TryGetPlayerOnSight(this Player player, float maxDistance, out Player target)
        {
            target = null;

            if (!Physics.Raycast(new Ray(player.ReferenceHub.PlayerCameraReference.position + player.GameObject.transform.forward * 0.3f, player.ReferenceHub.PlayerCameraReference.forward), out RaycastHit hit, maxDistance, LayerMask.GetMask("Player")))
                return false;

            target = Player.Get((hit.collider));

            return target != null;
        }
        
        public static bool TryGetPickupOnSight(this Player player, float maxDistance, out Pickup pickup)
        {
            //todo: Make sure the generated code works
            pickup = null;
            
            RaycastHit[] hits = new RaycastHit[30];
            Physics.RaycastNonAlloc(new Ray(player.ReferenceHub.PlayerCameraReference.position, player.ReferenceHub.PlayerCameraReference.forward), hits, maxDistance);

            
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform == null)
                    continue;
                
                pickup = Pickup.Get(hit.transform.gameObject);
                if (pickup != null)
                    return true;
            }
            return pickup != null;
        }
        
        public static bool TryGetRagdollOnSight(this Player player, float maxDistance, out Ragdoll ragdoll)
        {
            ragdoll = null;
            
            RaycastHit[] hits = new RaycastHit[30];
            Physics.RaycastNonAlloc(new Ray(player.ReferenceHub.PlayerCameraReference.position, player.ReferenceHub.PlayerCameraReference.forward), hits, maxDistance, LayerMask.GetMask("Ragdoll"));

            
            foreach (RaycastHit hit in hits)
            {
                if (hit.transform == null)
                    continue;
                Log.Info(hit.transform.name);
                foreach(Ragdoll doll in Ragdoll.List)
                {
                    if (hit.transform.parent.gameObject.TryGetComponent(out BasicRagdoll checkDoll) && Ragdoll.Get(checkDoll) == doll)
                    {
                        ragdoll = doll;
                        return true;
                    }
                    if (hit.transform.parent.gameObject == doll.GameObject)
                    {
                        ragdoll = doll;
                        return true;
                    }
                }
                
                if (ragdoll != null)
                    return true;
            }

            return ragdoll != null;
        }
        
        public static bool CanSee(this Player player, Player target, float range = 100)
        {
            RaycastHit[] hits = new RaycastHit[30];
            Physics.RaycastNonAlloc(new Ray(player.Position, target.Position - player.Position), hits,range, StandardHitregBase.HitregMask);
            float wallDistance = 1000;
            float targetDistance = 1000;
            foreach(RaycastHit hit in hits)
            {
                if (hit.transform == null)
                    continue;
                Player hitPlayer = Player.Get(hit.collider);
                if (hitPlayer == null)
                {
                    if(wallDistance > hit.distance)
                        wallDistance = hit.distance;
                }
                if (hitPlayer != null && hitPlayer == target)
                {
                    targetDistance = hit.distance;
                }
            }
            return targetDistance < wallDistance;
        }

        public static void ChangeSize(this Player player, float size)
        {
            TTCore.Instance.PlayerSizeManager.SetSize(player, size);
        }
        
        /// <summary>
        /// Checks if the player has NPC permissions.
        /// If 'RequirePermission' is enabled and the player has 'npc' permission, it returns true.
        /// If 'RequirePermission' is not enabled, it always returns true.
        /// </summary>
        /// <param name="player">The player instance.</param>
        /// <returns>Boolean indicating if player has NPC permissions.</returns>
        
        public static bool HasNpcPermissions(this Player player)
        {
            if(!player.CheckPermission("npc"))
            {
                return false;
            }
                                
            return true;
        }
    }
}