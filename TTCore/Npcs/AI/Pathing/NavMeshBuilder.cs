using Exiled.API.Features;
using Exiled.API.Features.Doors;
using TTCore.API;
using UnityEngine;
using UnityEngine.AI;

namespace TTCore.Npcs.AI.Pathing;

public class NavMeshBuilder : IRegistered
{
    
    public static readonly LayerMask HitregMask = (LayerMask) LayerMask.GetMask("Default", "Hitbox", "Glass", "CCTV"/*, "Door"*/);
    
    public static bool Build()
    {
        foreach (Room room in Room.List)
        {
            room.GameObject.AddComponent<NavMeshSurface>();
            var navSurface = room.gameObject.GetComponent<NavMeshSurface>();
            navSurface.collectObjects = CollectObjects.Children;
            navSurface.agentTypeID = 0;
            navSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            navSurface.layerMask = HitregMask;
            navSurface.BuildNavMesh();
        }
        foreach (Door door in Door.List)
        {
            //if (AtlasAI.HandlerPath.doorState.ContainsKey(door)) continue;
            //AtlasAI.HandlerPath.doorState.Add(door, Interactables.Interobjects.DoorUtils.DoorAction.Closed);
            //door.GameObject.AddComponent<NavMeshObstacle>(); //NavMeshObstacle
        }
        foreach (Lift lift in Lift.List)
        {
            lift.GameObject.AddComponent<NavMeshLink>();
            lift.GameObject.AddComponent<NavMeshSurface>();

            lift.CurrentDestination.GameObject.AddComponent<NavMeshLink>();
            lift.CurrentDestination.GameObject.AddComponent<NavMeshSurface>();
            //Connect Elevators between them

            var liftSur = lift.GameObject.GetComponent<NavMeshSurface>();
            liftSur.collectObjects = CollectObjects.Children;
            liftSur.agentTypeID = 0;
            liftSur.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            liftSur.layerMask = HitregMask;
            liftSur.BuildNavMesh();
        }
        return true;
    }
    
    public void OnWaitingForPlayers()
    {
        if(TTCore.Instance.Config.EnablePathFinding)
            Build();
    }


    public void Register()
    {
        Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
    }

    public void Unregister()
    {
        Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
    }
}