﻿using System.Collections.Generic;
using MapGeneration;
using UnityEngine;
using UnityEngine.AI;

namespace TTCore.Npcs.AI.Pathing;

public class NavMeshManager
{
    public static readonly List<NavMeshSurface> Surfaces = [];

    public static void InitializeMap()
    {
        LayerMask layer = 0;
        List<int> layers = [];

        foreach (RoomIdentifier room in RoomIdentifier.AllRoomIdentifiers)
            if (room.gameObject.activeSelf)
            {
                Collider[] colliders = room.gameObject.GetComponentsInChildren<Collider>();
                foreach (Collider col in colliders)
                    if (!layers.Contains(col.gameObject.layer))
                    {
                        layers.Add(col.gameObject.layer);
                        layer |= col.gameObject.layer;
                    }
            }

        Bake(layer);
    }

    private static void Bake(LayerMask layer)
    {
        GameObject go = new("NavMeshSurface");
        var lightZoneSurface = go.AddComponent<NavMeshSurface>();

        lightZoneSurface.layerMask = layer;
        lightZoneSurface.collectObjects = CollectObjects.All;
        lightZoneSurface.useGeometry = NavMeshCollectGeometry.PhysicsColliders;
        lightZoneSurface.voxelSize = 0.05f;
        lightZoneSurface.agentTypeID = 1;

        lightZoneSurface.BuildNavMesh();
    }
}