﻿using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Pickups;
using Exiled.Permissions.Extensions;
using InventorySystem.Items.Firearms.Modules;
using PlayerRoles.Ragdolls;
using TTCore.Utilities;
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
        
        public static bool TryGetDoorOnSight(this Player player, float maxDistance, out Door door)
        {
            door = null;
            RaycastHit[] hits = new RaycastHit[30];
            Physics.RaycastNonAlloc(new Ray(player.ReferenceHub.PlayerCameraReference.position, player.ReferenceHub.PlayerCameraReference.forward), hits, maxDistance, LayerMask.GetMask("Door"));

            /*foreach (RaycastHit hit in hits)
            {
                //Log.Debug(hit.collider?.name);
                if (hit.collider == null)
                    continue;
                if (hit.collider.name.StartsWith("Door") || hit.collider.name.StartsWith("PlainSide"))
                {
                    Log.Debug("Found possible door "+hit.collider.gameObject.name);
                    door = Door.GetClosest(hit.point, out float distance);
                    if (door != null && distance < 2f)
                        return true;
                }
            }*/
            float closestDistance = 1000;
            Vector3 closestDoor = Vector3.zero;
            foreach (RaycastHit hit in hits)
            {
                if(hit.distance < closestDistance)
                {
                    closestDistance = hit.distance;
                    closestDoor = hit.point;
                }
            }
            door = Door.GetClosest(closestDoor, out float distance);
            if(distance < 2f)
                return true;
            else
                door = null;
            return door != null;
        }
        
        public static bool CanSee(this Player player, Player target, float range = 100)
        {
            RaycastHit[] hits = new RaycastHit[30];
            Physics.RaycastNonAlloc(new Ray(player.Position, target.Position - player.Position), hits,range, HitscanHitregModuleBase.HitregMask);
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
        
        public static void ChangeSize(this Player player, Vector3 size)
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