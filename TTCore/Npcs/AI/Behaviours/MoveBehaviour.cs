using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.API.Structs;
using MapGeneration;
using MEC;
using PlayerRoles.FirstPersonControl;
using TTCore.Extensions;
using TTCore.Npcs.AI.Pathing;
using UnityEngine;
using UnityEngine.AI;

namespace TTCore.Npcs.AI.Behaviours;

public enum MoveState
{
    Idle,
    RoomTarget,
    DoorTarget,
    ThroughDoor,
    InElevator,
    Stop
}

public class MoveBehaviour : AIBehaviour
{
    
    private Player player;
    private NavMeshAgent navMeshAgent;
    private Vector3 MoveTarget, TempMoveTarget;
    private float LastUpdate;
    private List<Primitive> primitives;
    private Primitive MoveTargetPrim, TempMoveTargetPrim;
    private NavMeshPath Path;
    private MoveState state;
    
    public MoveBehaviour(Priority priority) : base(priority)
    {
        LastUpdate = Time.time;
    }

    void Start()
    {
        Log.Info("Loaded");
        player = Player.Get(GetComponent<ReferenceHub>());
        navMeshAgent = gameObject.AddComponent<NavMeshAgent>();
        var characterController = GetComponent<CharacterController>();
        navMeshAgent.radius = characterController.radius;
        navMeshAgent.acceleration = 40f;
        navMeshAgent.speed = 8.5f;
        navMeshAgent.angularSpeed = 120f;
        navMeshAgent.stoppingDistance = 0.3f;
        navMeshAgent.baseOffset = 1;
        navMeshAgent.autoRepath = true;
        navMeshAgent.autoTraverseOffMeshLink = false;
        navMeshAgent.height = characterController.height;
        navMeshAgent.obstacleAvoidanceType = ObstacleAvoidanceType.HighQualityObstacleAvoidance;
        navMeshAgent.agentTypeID = -1;
        state = MoveState.Idle;
        player.IsNoclipPermitted = true;
        //((FpcRole)player.Role).IsNoclipEnabled = true;
        
        primitives = new List<Primitive>();
        PrimitiveSettings settings = new PrimitiveSettings(PrimitiveType.Sphere, Color.blue, Vector3.zero, Vector3.zero,
            Vector3.one * -0.1f, true);
        for(int i = 0; i < 10; i++)
        {
            primitives.Add(Primitive.Create(settings));
        }
        
        MoveTargetPrim = Primitive.Create(new PrimitiveSettings(PrimitiveType.Sphere, Color.red, Vector3.zero, Vector3.zero,
            Vector3.one * -0.1f, true));
        TempMoveTargetPrim = Primitive.Create(new PrimitiveSettings(PrimitiveType.Sphere, Color.green, Vector3.zero, Vector3.zero,
            Vector3.one * -0.1f, true));
        
        Path = new NavMeshPath();
        gameObject.AddComponent<AgentLinkMover>();
    }

    public override void UpdateBehaviour()
    {
        
    }

    public override void FixedUpdateBehaviour()
    {
        /*if (player.CurrentRoom.Type != Exiled.API.Enums.RoomType.HczServers)
            navMeshAgent.baseOffset = 1.0f;
        else
            navMeshAgent.baseOffset = 1.4f;*/

        

        /*switch (state)
        {
            case MoveState.Idle:
                Log.Info("Idle");
                state = MoveState.RoomTarget;
                MoveTarget = Player.Get(p => !p.IsNPC).First().Position;
                MoveTargetPrim.Position = MoveTarget;
                if (MoveTarget != null)
                {
                    //Log.Info("Moving to " + MoveTarget);
                    SetMoveTarget(MoveTarget);
                }
                navMeshAgent.isStopped = false;
                break;
            case MoveState.RoomTarget:
                Log.Info("RoomTarget");
                navMeshAgent.isStopped = false;
                //Normal pathing through room.
                MoveTarget = Player.Get(p => !p.IsNPC).First().Position;
                MoveTargetPrim.Position = MoveTarget;
                if (MoveTarget != null)
                {
                    //Log.Info("Moving to " + MoveTarget);
                    SetMoveTarget(MoveTarget);
                }
                if (Room.Get(MoveTarget) != player.CurrentRoom)
                {
                    //Path through door
                    foreach (Door door in player.CurrentRoom.Doors)
                    {
                        if (door.Rooms.Contains(Room.Get(MoveTarget)))
                        {
                            Vector3 doorSide1 = door.Position + (door.Rotation * (Vector3.forward) + Vector3.up);
                            Vector3 doorSide2 = door.Position + (door.Rotation * (Vector3.back) + Vector3.up);
                            float dist1 = Vector3.Distance(doorSide1, player.Position);
                            float dist2 = Vector3.Distance(doorSide2, player.Position);
                            Vector3 targetSide = dist1<dist2 ? doorSide1 : doorSide2;
                            TempMoveTarget = targetSide;
                            TempMoveTargetPrim.Position = TempMoveTarget;
                            SetMoveTarget(TempMoveTarget);
                            state = MoveState.DoorTarget;
                                
                            break;
                        }
                    }
                }
                break;
            case MoveState.DoorTarget:
                Log.Info("DoorTarget");
                MoveTarget = Player.Get(p => !p.IsNPC).First().Position;
                MoveTargetPrim.Position = MoveTarget;
                navMeshAgent.isStopped = false;
                
                if (Vector3.Distance(TempMoveTarget, player.Position) < 0.2f)
                {
                    foreach (Door door in player.CurrentRoom.Doors)
                    {
                        if (door.Rooms.Contains(Room.Get(MoveTarget)))
                        {
                            Vector3 doorSide1 = door.Position + (door.Rotation * (Vector3.forward) + Vector3.up);
                            Vector3 doorSide2 = door.Position + (door.Rotation * (Vector3.back) + Vector3.up);
                            float dist1 = Vector3.Distance(doorSide1, player.Position);
                            float dist2 = Vector3.Distance(doorSide2, player.Position);
                            Vector3 targetSide = dist1<dist2 ? doorSide2 : doorSide1;
                            //Start forcing through doorway
                            TempMoveTarget = targetSide;
                            TempMoveTargetPrim.Position = TempMoveTarget;
                            state = MoveState.ThroughDoor;
                            break;
                        }
                    }
                }
                
                break;
            case MoveState.ThroughDoor:
                Log.Info("ThroughDoor");
                MoveTarget = Player.Get(p => !p.IsNPC).First().Position;
                MoveTargetPrim.Position = MoveTarget;
                navMeshAgent.isStopped = true;
                if (player.Role is FpcRole fpcRole2)
                {
                    //fpcRole2.IsNoclipEnabled = true;
                }

                player.Transform.position = TempMoveTarget;//Vector3.MoveTowards(player.Position, TempMoveTarget, navMeshAgent.speed* Time.deltaTime);
                if (Vector3.Distance(TempMoveTarget, player.Position) < 0.2f)
                {
                    state = MoveState.Stop;/////
                    MoveTarget = Player.Get(p => !p.IsNPC).First().Position;
                    MoveTargetPrim.Position = MoveTarget;
                    if (MoveTarget != null)
                    {
                        //Log.Info("Moving to " + MoveTarget);
                        SetMoveTarget(MoveTarget);
                    }
                    
                    navMeshAgent.isStopped = true;////
                    if (player.Role is FpcRole fpcRole1)
                    {
                        fpcRole1.IsNoclipEnabled = false;
                    }
                    TempMoveTargetPrim.Position = Vector3.zero;
                    Log.Info("ThroughDoor Complete");
                }
                break;
        }*/

        
        MoveTarget = Player.Get(p => !p.IsNPC).First().Position;
        MoveTargetPrim.Position = MoveTarget;
        if (MoveTarget != null)
        {
            //Log.Info("Moving to " + MoveTarget);
            SetMoveTarget(MoveTarget);
        }
        
        if (player.Lift != null)
        {
            if (player.Lift.IsMoving)
            {
                state = MoveState.InElevator;
                navMeshAgent.isStopped = true;
                navMeshAgent.enabled = false;
            } else if(state == MoveState.InElevator)
            {
                state = MoveState.RoomTarget;
                navMeshAgent.isStopped = false;
                navMeshAgent.enabled = true;
            }
        }
        

        

        if (player.Role is FpcRole fpcRole)
        {
            Vector3 direction = (MoveTarget+(Vector3.up*0.1f)) - player.Position;
            Quaternion quat = Quaternion.LookRotation(direction, Vector3.up);
            FpcMouseLook mouseLook = fpcRole.FirstPersonController.FpcModule.MouseLook;
            (ushort horizontal, ushort vertical) = quat.ToClientUShorts();
            mouseLook.ApplySyncValues(horizontal, vertical);
            /*if (!fpcRole.FirstPersonController.FpcModule.CharController.SimpleMove(Vector3.Normalize(direction) *
                    (navMeshAgent.speed * Time.deltaTime)))
            {
                fpcRole.FirstPersonController.FpcModule.CharController.Move(Vector3.Normalize(direction) *
                                                                            (navMeshAgent.speed * Time.deltaTime));
            }*/
        }

        if (Time.time - LastUpdate > 1f)
        {
            
            Door nearestDoor = Door.GetClosest(player.Position, out float dist);
            if (dist < 2f)
            {
                nearestDoor.IsOpen = true;
            }
            
            if (!navMeshAgent.isOnNavMesh)
            {
                Log.Error("Not on NavMesh");
            }

            LastUpdate = Time.time;
            if (navMeshAgent.pathStatus != NavMeshPathStatus.PathComplete)
            {
                int i = 0;
                foreach (Vector3 pos in navMeshAgent.path.corners)
                {
                    if (i < 10)
                        primitives.ElementAt(i++).Position = pos;
                }
                string output = "Corner Count: " + navMeshAgent.path.corners.Length + " ";
                foreach (Vector3 pos in navMeshAgent.path.corners)
                {
                    output += pos + " ";
                }
                Log.Info(output);
            }
        }
    }
    
    
    public void SetMoveTarget(Vector3 target)
    {
        MoveTarget = target;
        navMeshAgent.SetDestination(target);
        //navMeshAgent.CalculatePath(target, Path);
        //navMeshAgent.SetPath(Path);
        
        //navMeshAgent.isStopped = true;
    }
    
    
}