using System;
using System.Collections.Generic;
using AdminToys;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using MEC;
using RoundModifiers.API;
using TTCore.HUDs;
using UnityEngine;

namespace RoundModifiers.Modifiers;

public class BehindYou : Modifier
{
    // Basics of this modifier:
    // - This is an implentation of SCP-650, the mannequin.
    // - SCP-650 will be contained in GR18 in Light containment zone.
    // - When it's room is opened, SCP-650 will be released.
    // - SCP-650 will follow the player who opened it's room.
    // - To follow a player, it will attempt to teleport behind them when not being looked at. Its new location must also not be looked at by another player.
    // - SCP-650 will not kill the player, but will attempt to scare them
    // - If someone else looks at SCP-650, it will stop following the original player and follow the new player.
    // - SCP-650 will only follow humans
    
    public Player Target { get; private set; }
    public Vector3 LastPosition { get; private set; }
    //public bool IsSeen { get; private set; }
    public GameObject Scp650 { get; private set; }

    public void OnRoundStart()
    {
        Player target = Player.List.GetRandomValue();
        Log.Debug(PrefabHelper.Spawn(PrefabType.DBoyTarget, target.Position, target.Rotation));
        target.ShowHUDHint("SCP-650 is following you!", 5f);
    }

    // Need a slow tick to update the possible next position for SCP-650
    // Need a fast tick that will check for when SCP-650 is about to be looked at, or position has already.
    
    public IEnumerator<float> SlowTick()
    {
        
        while (Round.InProgress)
        {
            yield return Timing.WaitForSeconds(1f);
            if(Target.IsDead) Target = null;
            if(Target.IsScp) Target = null;
            if (Target == null) continue;
            bool IsSeen = false;
            bool NewSeen = false;
            //Check all players in a range of the teleport position and perform a ray trace against them.
            foreach (Player player in Player.Get(p => Vector3.Distance(p.Position, LastPosition) < 7f))
            {
                // Get the direction from SCP-650 to the player
                Vector3 direction = player.Position - LastPosition;
                Vector3 currentDirection = player.Position - Scp650.transform.position;
                // Get the Vector3 rotation of the player
                Vector3 rotation = player.Rotation.eulerAngles;
                // Get the angle between the direction and the rotation
                float angle = Vector3.Angle(direction, rotation);
                float currentAngle = Vector3.Angle(currentDirection, rotation);
                // If the angle is less than 90 degrees, the player is looking at SCP-650
                if (Math.Abs(angle) < 90f)
                {
                    //Player is looking at the position
                    // To be sure run a ray trace to see if anything is in the way
                    NewSeen = true;
                }
                if (Math.Abs(currentAngle) < 90f)
                {
                    //Player is looking at the position
                    // To be sure run a ray trace to see if anything is in the way
                    IsSeen = true;
                }
                
            }

            
            if(!NewSeen)
            {
                // Teleport SCP605 to the player
                Scp650.transform.position = LastPosition;
                // Rotate SCP605 to face the player
                Scp650.transform.LookAt(Target.Position);
            } else if (!IsSeen && Vector3.Distance(Scp650.transform.position, LastPosition) > 6f)
            {
                // Teleport SCP605 away from the player
                Scp650.transform.position = Vector3.zero;
            }

            if (Vector3.Distance(LastPosition, Target.Position) > 1f)
            {
                LastPosition = Target.Position;
            }

        }
    }
    
    public IEnumerator<float> FastTick()
    {
        while (Round.InProgress)
        {
            yield return Timing.WaitForSeconds(0.1f);
            
        }
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "BehindYou",
        Description = "SCP-650 is following you!",
        Aliases = new []{"mannequin","scp650","650"},
        Impact = ImpactLevel.MajorGameplay,
        MustPreload = false,
        Balance = 3,
        Category = Category.Visual,
        FormattedName = "<color=red>Behind You</color>"
    };
}