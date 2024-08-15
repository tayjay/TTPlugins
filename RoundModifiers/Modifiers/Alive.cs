using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;
using TTCore.Npcs.AI;
using TTCore.Npcs.AI.Behaviours;
using TTCore.Utilities;
using UnityEngine;
using UnityEngine.AI;
using Room = Exiled.API.Features.Room;

namespace RoundModifiers.Modifiers
{
    public class Alive : Modifier
    {
        public static readonly LayerMask HitregMask = (LayerMask) LayerMask.GetMask("Default", "Glass"/*"Hitbox", "CCTV","Door" */);
        
        public static Dictionary<RoomType,List<Vector3>> RoomMovePositions = new Dictionary<RoomType, List<Vector3>>();
        public List<Npc> Npcs = new List<Npc>();
        protected CoroutineHandle MoveNpcsHandle;
        
        public Alive()
        {
            /*RoomMovePositions[RoomType.Lcz330] = new List<Vector3>()
            {
                /*new Vector3(0.67f, 0.00f, 2.50f),
                new Vector3(-3.46f, 1.37f, 4.64f),
                new Vector3(3.11f, 1.28f, 0.79f),
                new Vector3(0.83f, 0.91f, 0.78f),
                new Vector3(-5.77f, 1.40f, -3.42f),
                new Vector3(1.62f, 1.05f, -3.15f),#1#
                new Vector3(1.03f, 0.00f, 2.27f),
                new Vector3(-1.49f, 0.00f, 2.46f),
                new Vector3(-1.26f, 0.00f, 4.83f),
            };*/
            RoomMovePositions[RoomType.LczClassDSpawn] = new List<Vector3>()
            {
                new Vector3(3.51f, 0.00f, 0.00f),
                new Vector3(-10.13f, 0.00f, 4.90f),
                new Vector3(-9.96f, 0.00f, -4.53f),
            };
            //Toilets
            RoomMovePositions[RoomType.LczToilets] = new List<Vector3>()
            {
                new Vector3(-1.67f, 0.00f, -7.03f),
                new Vector3(-4.60f, 0.00f, -7.31f),
                new Vector3(-4.69f, 0.00f, -5.82f),
                new Vector3(4.72f, 0.00f, -7.23f),
                new Vector3(2.67f, 0.00f, -5.08f),
            };
            
            //Lcz173
            RoomMovePositions[RoomType.Lcz173] = new List<Vector3>()
            {
                new Vector3(-3.42f, 11.44f, -5.01f),
                new Vector3(8.86f, 11.47f, -1.34f),
                new Vector3(10.16f, 11.47f, 7.69f),
                new Vector3(13.53f, 11.47f, 4.96f),
                new Vector3(20.2f, 11.47f, 11.06f),
                new Vector3(16.84f, 11.47f, 8.06f),
                new Vector3(9.32f, 11.47f, 11.9f),
                new Vector3(6.22f, 11.47f, 12.71f)
            };
            
            //Lcz914
            RoomMovePositions[RoomType.Lcz914] = new List<Vector3>()
            {
                new Vector3(2.35f, 0.0f, -0.31f),
                new Vector3(4.89f, 0.06f, 6.0f),
                new Vector3(-1.48f, 0.0f, 6.35f),
                new Vector3(-1.47f, 0.0f, -6.24f),
                new Vector3(2.31f, 0.0f, 0.32f),
                new Vector3(3.08f, 0.0f, -6.07f)
            };
            
            //LczVT
            RoomMovePositions[RoomType.LczPlants] = new List<Vector3>()
            {
                new Vector3(-1.02f, 0.0f, 6.82f),
                new Vector3(2.59f, 0.0f, 6.65f),
                new Vector3(2.44f, 0.0f, 2.8f),
                new Vector3(6.44f, 0.0f, 6.87f)
            };
            
            //Hcz049
            RoomMovePositions[RoomType.Hcz049] = new List<Vector3>()
            {
                new Vector3(0.02f, 199.47f, 9.00f),
                new Vector3(-0.04f, 199.47f, 12.5f),
                new Vector3(-2.6f, 199.47f, 14.87f),
                new Vector3(-2.78f, 199.47f, 9.65f)
            };
            
            //EzIntercom
            RoomMovePositions[RoomType.EzIntercom] = new List<Vector3>()
            {
                new Vector3(1.11f, -5.82f, 4.15f),
                new Vector3(-2.70f, -5.82f, 3.36f),
                new Vector3(-2.57f, -5.82f, -2.69f),
                new Vector3(-4.81f, -5.82f, -2.63f),
                new Vector3(-4.66f, -5.82f, 1.49f),
            };
            
            //EzOffice
            RoomMovePositions[RoomType.EzUpstairsPcs] = new List<Vector3>()
            {
                new Vector3(6.03f, 2.87f, 5.29f),
                new Vector3(3.12f, 2.86f, -6.28f),
                new Vector3(2.70f, 0.00f, 5.77f),
                new Vector3(-3.45f, 0.00f, -1.85f),
                new Vector3(-2.70f, 0.00f, -6.28f),
                new Vector3(-2.34f, 0.00f, 5.45f),
                new Vector3(-3.16f, 0.00f, 2.03f),
            };
        }
        

        public void OnRoundStart()
        {
            //Spawn NPCs
            Vector3 pos = RoomMovePositions[RoomType.LczClassDSpawn][0];
            Vector3 LczClassDSpawnGlobalPos = TransformUtils.CalculateGlobalPosition(pos, Room.Get(RoomType.LczClassDSpawn).Position, Room.Get(RoomType.LczClassDSpawn).Rotation);
            TTCore.TTCore.Instance.NpcManager.SpawnNpc("D1234", RoleTypeId.ClassD,
                LczClassDSpawnGlobalPos+Vector3.one,out Npc npc, isGodMode:false);
            Npcs.Add(npc);
            
            TTCore.TTCore.Instance.NpcManager.SpawnNpc("D2213", RoleTypeId.ClassD,
                LczClassDSpawnGlobalPos+Vector3.one,out Npc npc1, isGodMode:false);
            Npcs.Add(npc1);
            
            pos = RoomMovePositions[RoomType.LczToilets][0];
            Vector3 LczToiletsGlobalPos = TransformUtils.CalculateGlobalPosition(pos, Room.Get(RoomType.LczToilets).Position, Room.Get(RoomType.LczToilets).Rotation);
            TTCore.TTCore.Instance.NpcManager.SpawnNpc("Dr. P", RoleTypeId.Scientist,
                LczToiletsGlobalPos+Vector3.one,out Npc npc2, isGodMode:false);
            Npcs.Add(npc2);
            
            pos = RoomMovePositions[RoomType.Lcz173][0];
            Vector3 Lcz173GlobalPos = TransformUtils.CalculateGlobalPosition(pos, Room.Get(RoomType.Lcz173).Position, Room.Get(RoomType.Lcz173).Rotation);
            TTCore.TTCore.Instance.NpcManager.SpawnNpc("D234", RoleTypeId.ClassD,
                Lcz173GlobalPos+Vector3.one,out Npc npc3, isGodMode:false);
            Npcs.Add(npc3);
            
            pos = RoomMovePositions[RoomType.Lcz914][0];
            Vector3 Lcz914GlobalPos = TransformUtils.CalculateGlobalPosition(pos, Room.Get(RoomType.Lcz914).Position, Room.Get(RoomType.Lcz914).Rotation);
            TTCore.TTCore.Instance.NpcManager.SpawnNpc("Dr. Orgona", RoleTypeId.Scientist,
                Lcz914GlobalPos+Vector3.one,out Npc npc4, isGodMode:false);
            Npcs.Add(npc4);
            
            /*pos = RoomMovePositions[RoomType.LczPlants][0];
            Vector3 LczPlantsGlobalPos = TransformUtils.CalculateGlobalPosition(pos, Room.Get(RoomType.LczPlants).Position, Room.Get(RoomType.LczPlants).Rotation);
            TTCore.TTCore.Instance.NpcManager.SpawnNpc("Dr. Plantera", RoleTypeId.Scientist,
                LczPlantsGlobalPos+Vector3.one,out Npc npc5, isGodMode:false);
            Npcs.Add(npc5);*/
            
            pos = RoomMovePositions[RoomType.Hcz049][0];
            Vector3 Hcz049GlobalPos = TransformUtils.CalculateGlobalPosition(pos, Room.Get(RoomType.Hcz049).Position, Room.Get(RoomType.Hcz049).Rotation);
            TTCore.TTCore.Instance.NpcManager.SpawnNpc("Dr. Zed", RoleTypeId.Scientist,
                Hcz049GlobalPos+Vector3.one,out Npc npc6, isGodMode:false);
            Npcs.Add(npc6);
            
            pos = RoomMovePositions[RoomType.EzIntercom][0];
            Vector3 EzIntercomGlobalPos = TransformUtils.CalculateGlobalPosition(pos, Room.Get(RoomType.EzIntercom).Position, Room.Get(RoomType.EzIntercom).Rotation);
            TTCore.TTCore.Instance.NpcManager.SpawnNpc("Sgt. Intercom", RoleTypeId.FacilityGuard,
                EzIntercomGlobalPos+Vector3.one,out Npc npc7, isGodMode:false);
            Npcs.Add(npc7);
            
            pos = RoomMovePositions[RoomType.EzUpstairsPcs][0];
            Vector3 EzUpstairsPcsGlobalPos = TransformUtils.CalculateGlobalPosition(pos, Room.Get(RoomType.EzUpstairsPcs).Position, Room.Get(RoomType.EzUpstairsPcs).Rotation);
            TTCore.TTCore.Instance.NpcManager.SpawnNpc("Dr. PC", RoleTypeId.Scientist,
                EzUpstairsPcsGlobalPos+Vector3.one,out Npc npc8, isGodMode:false);
            Npcs.Add(npc8);


            MoveNpcsHandle = Timing.RunCoroutine(MoveNpcs());
        }
        
        public void OnWaitingForPlayers()
        {
            //Build per-room NavMeshes
            Timing.RunCoroutine(BuildNavMesh());
        }

        
        

        public void OnShot(ShotEventArgs ev)
        {
            Vector3 globalPos = ev.Position;
            Room room = Room.Get(globalPos);
            Vector3 relativePos = TransformUtils.CalculateRelativePosition(globalPos, room.Position, room.Rotation);
            Log.Info($"Room: {room.Type}, Relative: {relativePos}");
        }
        
        public IEnumerator<float> BuildNavMesh()
        {
            foreach (Room room in Room.List)
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
                    Log.Info("Built NavMesh for room: " + room.Name);
                }
                catch (System.Exception e)
                {
                    Log.Error("Error building NavMesh for room: " + room.Name);
                    Log.Error(e);
                }

                yield return Timing.WaitForOneFrame;
            }
            
        }
        
        private IEnumerator<float> MoveNpcs()
        {
            yield return Timing.WaitForSeconds(7f);
            while (true)
            {
                foreach (var npc in Npcs)
                {
                    try
                    {
                        if (npc.GameObject.GetComponent<Brain>()?.GetBehaviourOfType<WanderRoomBehaviour>() == null)
                        {
                            //Log.Debug("Adding WanderRoomBehaviour to NPC.");
                            npc.GameObject.GetComponent<Brain>().AddBehaviourOfType<WanderRoomBehaviour>();
                            continue;
                        }
                        Vector3 relativePos = RoomMovePositions[npc.CurrentRoom.Type].RandomItem();
                        Vector3 globalPos = TransformUtils.CalculateGlobalPosition(relativePos, npc.CurrentRoom.Position, npc.CurrentRoom.Rotation);
                        npc.GameObject.GetComponent<WanderRoomBehaviour>().SetMoveTarget(globalPos);
                        //Log.Debug("Moving NPC to: " + globalPos);
                    } catch (System.Exception e)
                    {
                        Log.Error("Error moving NPC.");
                        Log.Error(e);
                    }
                    
                }
                yield return Timing.WaitForSeconds(10f);
            }
        }

        public void OnDied(DiedEventArgs ev)
        {
            if (ev.Player is Npc npc && Npcs.Contains(npc))
            {
                Npcs.Remove(npc);
                npc.ReferenceHub.OnDestroy();

                LeftEventArgs newLeft = new(npc);
                Exiled.Events.Handlers.Player.OnLeft(newLeft);
            }
                
        }
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Player.Shot += OnShot;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Died += OnDied;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Player.Shot -= OnShot;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Timing.KillCoroutines(MoveNpcsHandle);
            Npcs.Clear();
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "alive",
            FormattedName = "Alive",
            Description = "Others roam the facility.",
            Aliases = new []{"dead"},
            MustPreload = true,
            Impact = ImpactLevel.MajorGameplay
        };
    }
}