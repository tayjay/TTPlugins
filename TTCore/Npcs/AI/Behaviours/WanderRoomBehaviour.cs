using System;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Roles;
using MEC;
using PlayerRoles.FirstPersonControl;
using TTCore.Extensions;
using UnityEngine;
using UnityEngine.AI;

namespace TTCore.Npcs.AI.Behaviours;

public class WanderRoomBehaviour : AIBehaviour
{
    private NavMeshAgent navMeshAgent;
    private Vector3 MoveTarget;
    
    public WanderRoomBehaviour(Priority priority) : base(priority)
    {
    }

    private void Start()
    {
        navMeshAgent = Owner.GameObject.AddComponent<NavMeshAgent>();
        CharacterController characterController = GetComponent<CharacterController>();
        navMeshAgent.radius = characterController.radius;
        navMeshAgent.acceleration = 40f;
        navMeshAgent.speed = 5.5f;//8.5f;
        navMeshAgent.angularSpeed = 120f;
        navMeshAgent.stoppingDistance = 0.3f;
        navMeshAgent.baseOffset = 1;
        navMeshAgent.autoRepath = true;
        navMeshAgent.autoTraverseOffMeshLink = false;
        navMeshAgent.height = characterController.height;
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        navMeshAgent.agentTypeID = -1;
        
        MoveTarget = Owner.Position;
        
        Log.Debug("Setup WanderRoomBehaviour");
    }
    
    public void SetMoveTarget(Vector3 target)
    {
        MoveTarget = target;
        navMeshAgent.SetDestination(target);
        //Log.Debug("Changing target to: " + target);
    }

    public override void UpdateBehaviour()
    {
        
    }
    
    float LastUpdate = 0f;

    public override void FixedUpdateBehaviour()
    {
        if (Owner.Role is FpcRole fpcRole)
        {
            Vector3 LookAt = MoveTarget;
            if (Owner.Velocity.magnitude <0.01f)
            {
                if (Owner.CurrentRoom.Players.Where(p => !p.IsNPC).Any())
                {
                    LookAt = Owner.CurrentRoom.Players.First(p => !p.IsNPC).Position;
                }
            }
            Vector3 direction = (LookAt) - Owner.Position;
            Quaternion quat = Quaternion.LookRotation(direction, Vector3.up);
            FpcMouseLook mouseLook = fpcRole.FirstPersonController.FpcModule.MouseLook;
            (ushort horizontal, ushort vertical) = quat.ToClientUShorts();
            mouseLook.ApplySyncValues(horizontal, vertical);

            fpcRole.MoveState = PlayerMovementState.Sneaking;
            navMeshAgent.speed = fpcRole.WalkingSpeed-2;
        }

        if (Time.time - LastUpdate > 1f)
        {
            LastUpdate = Time.time;
            Door nearestDoor = Door.GetClosest(Owner.Position, out float dist);
            if (dist < 1.7f)
            {
                nearestDoor.IsOpen = true;
                Timing.CallDelayed(3f, () => nearestDoor.IsOpen = false);
            }
        }
        

        
        
        navMeshAgent.SetDestination(MoveTarget);
    }
}