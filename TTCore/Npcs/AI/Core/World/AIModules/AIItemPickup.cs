using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using MapGeneration;
using TTCore.Npcs.AI.Core.Management;
using UnityEngine;

namespace TTCore.Npcs.AI.Core.World.AIModules;

public class AIItemPickup : AIModuleBase
{
    
    public Pickup TargetPickup { get; private set; }
    public float PickupTimer { get; private set; }
    public bool CanPickup { get; private set; }
    
    public override bool Condition()
    {
        return Parent.Player.IsHuman && Parent.Player.Items.Count < 8 && GetNearPickups(Parent.Position).Any();
    }
    
    public IEnumerable<Pickup> GetNearPickups(Vector3 position, float toleration = 5f)
    {
        return Room.Get(position).Pickups.Where((pickup => (double) (position - pickup.Position).sqrMagnitude <= (double) toleration * (double) toleration && CanAIPickup(pickup)));
    }
    
    public bool CanAIPickup(Pickup pickup)
    {
        if(pickup.PreviousOwner == null) return false;
        if (pickup.PreviousOwner.IsAI()) return false;
        if(pickup.InUse) return false;
        if(!pickup.IsSpawned) return false;
        if (pickup.Info.ItemId.IsWeapon())
        {
            /*Item currentWeapon = Parent.Player.Items.FirstOrDefault(i => i.IsWeapon);
            if (currentWeapon != null)
            {
                Parent.Player.DropItem(currentWeapon);
                return true;
            }*/
            return true;
        }
        if (pickup.Info.ItemId.IsArmor())
        {
            /*Item currentArmor = Parent.Player.Items.FirstOrDefault(i => i.IsArmor);
            if (currentArmor != null)
            {
                Parent.Player.DropItem(currentArmor);
                return true;
            }*/
            return true;
        }
        //todo: Check if too many of certain type of item

        return true;
    }

    public override void Init()
    {
        Tags = [AIBehaviorBase.AutonomyTag];
        
    }

    public override void OnDisabled()
    {
        if(TargetPickup != null && CanPickup)
            TargetPickup.InUse = false;
        CanPickup = false;
        PickupTimer = 0f;
        TargetPickup = null;
    }

    public override void OnEnabled()
    {
        PickupTimer = 0f;
        TargetPickup = null;
    }

    public override void Tick()
    {
        if (TargetPickup == null || TargetPickup == default)
        {
            TargetPickup = GetNearPickups(Parent.Position).FirstOrDefault();
            if(TargetPickup==null) return;
            CanPickup = false;
            Log.Debug("Found pickup "+TargetPickup?.Info.ItemId+" at "+TargetPickup?.Position);
        } else if (!CanPickup && TargetPickup.InUse)
        {
            Log.Debug("Someone else is picking up item");
            //Someone else has started picking up the item
            TargetPickup = null;
            CanPickup = false;
        }
        else
        {
            if (PickupTimer <= 0f && CanPickup)
            {
                //Swap out any held items
                if (TargetPickup.Info.ItemId.IsWeapon())
                {
                    Item currentWeapon = Parent.Player.Items.FirstOrDefault(i => i.IsWeapon);
                    if (currentWeapon != null)
                    {
                        Parent.Player.DropItem(currentWeapon);
                    }
                }
                if (TargetPickup.Info.ItemId.IsArmor())
                {
                    Item currentArmor = Parent.Player.Items.FirstOrDefault(i => i.IsArmor);
                    if (currentArmor != null)
                    {
                        Parent.Player.DropItem(currentArmor);
                    }
                }
                
                Log.Debug("Picking up item");
                Parent.Player.AddItem(TargetPickup.Info.ItemId); //todo: get exact item rather than id
                TargetPickup.UnSpawn();
                TargetPickup = null;
                CanPickup = false;
                Log.Debug("Item picked up");
            }
            else
            {
                if((Parent.Position - TargetPickup.Position).sqrMagnitude <= 2f && !CanPickup)
                {
                    Log.Debug("Starting pickup");
                    this.Parent.MovementEngine.LookPos = TargetPickup.Position;
                    TargetPickup.InUse = true;
                    PickupTimer = TargetPickup.PickupTimeForPlayer(Parent.Player)+1;
                    CanPickup = true;
                }
                else
                {
                    Log.Debug("Moving to pickup");
                    Parent.GetModule<AIPathfinder>().SetDestination(TargetPickup.Position);
                }
            }
            //Log.Debug("Something else is happening");
        }
        
        PickupTimer -= Time.fixedDeltaTime;
    }
}