using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using MEC;
using PluginAPI.Core.Zones;
using TTCore.API;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Experimental.AI;

namespace TTCore.Npcs.AI.Pathing;

public class NavMeshBuilder : IRegistered
{
    
    public static readonly LayerMask HitregMask = (LayerMask) LayerMask.GetMask("Default", "Glass");

    
    private static int _layerMask;
    public static LayerMask CalculatedMask
    {
        get
        {
            int layer = LayerMask.NameToLayer("Player");
            for (int layer2 = 0; layer2 < 32; ++layer2)
            {
                if (!Physics.GetIgnoreLayerCollision(layer, layer2))
                    _layerMask |= 1 << layer2;
            }
            return (LayerMask) _layerMask;
        }
    }
    
    public CoroutineHandle BuildHandle;
    
    public IEnumerator<float> Build()
    {
        Log.Info("Building NavMesh");

        /*for (int i = 0; i < 100; i++)
        {
            if (LayerMask.LayerToName(i) != null)
            {
                Log.Info("Layer: " + LayerMask.LayerToName(i));
            }
        }*/


        NavMeshBuildSettings settings = NavMesh.GetSettingsByID(-1);
        
        settings.agentRadius = 0.5f;
        settings.agentHeight = 2f;
        
        GameObject facilityParentZone = new GameObject("facilityParentZone");
        try
        {
            foreach(Room room in Room.List)
            {
                room.GameObject.transform.SetParent(facilityParentZone.transform);
            }
            var navSurface = facilityParentZone.AddComponent<NavMeshSurface>();
            navSurface.collectObjects = CollectObjects.Children;
            navSurface.agentTypeID = -1;
            navSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            navSurface.layerMask = HitregMask;
            navSurface.BuildNavMesh();
        } catch (System.Exception e)
        {
            Log.Error("Error building NavMesh for room: facilityParentZone");
            Log.Error(e);
        }
        
        foreach (Door door in Door.List)
        {
            ///door.IsOpen = true;
            //NavMeshObstacle obstacle = door.GameObject.AddComponent<NavMeshObstacle>();
            //obstacle.carving = true;
            if(door.Rooms.Count==1) continue;

            /*NavMeshLink link = door.Room.GameObject.AddComponent<NavMeshLink>();
            link.startPoint = door.Position+(door.Rotation*(Vector3.forward*0.5f));
            link.endPoint = door.Position+(door.Rotation*(Vector3.back*0.5f));
            link.width = 1f;
            link.bidirectional = true;
            link.agentTypeID = -1;
            
            NavMeshModifier mod = door.GameObject.AddComponent<NavMeshModifier>();
            mod.ignoreFromBuild = true;*/
            
            door.GameObject.AddComponent<NavMeshSurface>();
            var navSurface = door.GameObject.GetComponent<NavMeshSurface>();
            navSurface.collectObjects = CollectObjects.Children;
            navSurface.agentTypeID = -1;
            navSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
            navSurface.layerMask = HitregMask;
            navSurface.BuildNavMesh();
            
             
            yield return Timing.WaitForOneFrame;

        }
        
        
        /*foreach (Room room in Room.List)
        {
            try
            {
                room.GameObject.AddComponent<NavMeshSurface>();
                var navSurface = room.gameObject.GetComponent<NavMeshSurface>();
                navSurface.collectObjects = CollectObjects.Children;
                navSurface.agentTypeID = -1;
                navSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
                navSurface.layerMask = HitregMask;
                navSurface.BuildNavMesh();
                Log.Info("Built NavMesh for room: "+room.Name+" Mesh Details: "+ navSurface.GetBuildSettings().agentRadius +" "+navSurface.GetBuildSettings().agentHeight);
            } catch (System.Exception e)
            {
                Log.Error("Error building NavMesh for room: "+room.Name);
                Log.Error(e);
            }
            
            yield return Timing.WaitForOneFrame;
        }*/
        
        
        
        foreach (Lift lift in Lift.List)
        {
            try
            {
                lift.GameObject.AddComponent<NavMeshLink>();
                lift.GameObject.AddComponent<NavMeshSurface>();

                lift.CurrentDestination.GameObject.AddComponent<NavMeshLink>();
                lift.CurrentDestination.GameObject.AddComponent<NavMeshSurface>();
                //Connect Elevators between them

                var liftSur = lift.GameObject.GetComponent<NavMeshSurface>();
                liftSur.collectObjects = CollectObjects.Children;
                liftSur.agentTypeID = -1;
                liftSur.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
                liftSur.layerMask = HitregMask;
                liftSur.BuildNavMesh();
                Log.Info("Built NavMesh for lift: "+lift.Name);
            } catch (System.Exception e)
            {
                Log.Error("Error building NavMesh for lift: "+lift.Name);
                Log.Error(e);
            }
            yield return Timing.WaitForOneFrame;
        }
        
        
        
        Log.Info("NavMesh built");
        
        yield return Timing.WaitForSeconds(1f);
    }

    public IEnumerator<float> Build2()
    {
        Log.Info("Building NavMesh");
        
        
        
        
        yield return Timing.WaitForOneFrame;
        Log.Info("NavMesh built");
    }
    
    public void OnWaitingForPlayers()
    {
        if (TTCore.Instance.Config.EnablePathFinding)
        {
            /*foreach (Door door in Door.List)
            {
                door.IsOpen = true;
            }*/
            
            /*Timing.CallDelayed(Timing.WaitUntilDone(BuildHandle), () =>
            {
                Log.Info("NavMesh built");
                Map.Broadcast(3, "NavMesh built", shouldClearPrevious:true);
            });*/
        }
        /*Timing.CallDelayed(1f, () =>
        {
            BuildHandle = Timing.RunCoroutine(Build()); //Timing.RunCoroutine(Build());
        });*/
            
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