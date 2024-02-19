﻿using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Toys;
using Exiled.API.Structs;
using MEC;
using RoundModifiers.API;
using UnityEngine;

namespace RoundModifiers.Modifiers
{
    public class Scp249 : Modifier
    {
        private CoroutineHandle _doorMovementCoroutine;
        private CoroutineHandle _teleportCoroutine;
        
        public Door[] Doors { get; private set; }
        
        public Primitive[] DoorMarkers { get; private set; }
        
        public int PlacingDoor { get; private set; }

        public int DoorCount => RoundModifiers.Instance.Config.Scp249_DoorCount;
        public float LastPlaceTime { get; private set; }
        
        public Scp249()
        {
            Doors = new Door[DoorCount];
            DoorMarkers = new Primitive[DoorCount];
        }

        public void Start()
        {
            PlacingDoor = 0;
            Doors = new Door[DoorCount];
            DoorMarkers = new Primitive[DoorCount];
            
            for(int i = 0; i < DoorCount; i++)
            {
                Doors[i] = GetRandomDoor();
                PrimitiveSettings settings = new PrimitiveSettings(PrimitiveType.Cube, new Color(10f,10f,10f, 0.15f),
                    Doors[i].Position + new Vector3(0,1.2f,0)-(Doors[i].Rotation*new Vector3(0,0,0.001f)),
                    Doors[i].Base.transform.eulerAngles*-1,
                new Vector3(-1.4f, -2.5f, -0.005f), true);
                DoorMarkers[i] = Primitive.Create(settings);
            }
            LastPlaceTime = Time.time;
            _doorMovementCoroutine = Timing.RunCoroutine(MoveDoors());
            _teleportCoroutine = Timing.RunCoroutine(TeleportPlayers());
        }

        public void Stop()
        {
            for(int i = 0; i < DoorCount; i++)
            {
                //Doors[i].Destroy();
                DoorMarkers[i].Destroy();
                
                Doors[i] = null;
                DoorMarkers[i] = null;
            }
            Timing.KillCoroutines(_doorMovementCoroutine);
            Timing.KillCoroutines(_teleportCoroutine);
        }
        
        public IEnumerator<float> MoveDoors()
        {
            yield return Timing.WaitForSeconds(1f);
            while (true)
            {
                if(Time.time-LastPlaceTime > RoundModifiers.Instance.Config.Scp249_MoveInterval)
                {
                    Doors[PlacingDoor] = GetRandomDoor();
                    DoorMarkers[PlacingDoor].Destroy();
                    PrimitiveSettings settings = new PrimitiveSettings(PrimitiveType.Cube, new Color(10f,10f,10f, 0.15f),
                        Doors[PlacingDoor].Position + new Vector3(0,1.2f,0)-(Doors[PlacingDoor].Rotation*new Vector3(0,0,0.001f)),
                        Doors[PlacingDoor].Base.transform.eulerAngles*-1,
                        new Vector3(-1.4f, -2.5f, -0.005f), true);
                    DoorMarkers[PlacingDoor] = Primitive.Create(settings);
                    LastPlaceTime = Time.time;
                    PlacingDoor = (PlacingDoor + 1) % DoorCount;
                }
                
                
                
                if(Time.time-LastPlaceTime > RoundModifiers.Instance.Config.Scp249_MoveInterval-1f)
                {
                    //Want to start fading the door that is changing to red to indicate it will be the next door to move
                    DoorMarkers[PlacingDoor].Color = Color.Lerp(DoorMarkers[PlacingDoor].Color, new Color(0f,0f,0f, 0f), 0.1f);
                } else if(Time.time-LastPlaceTime > RoundModifiers.Instance.Config.Scp249_MoveInterval-10f)
                {
                    //Want to start fading the door that is changing to red to indicate it will be the next door to move
                    DoorMarkers[PlacingDoor].Color = Color.Lerp(DoorMarkers[PlacingDoor].Color, new Color(10f,0f,0f, 0.15f), 0.01f);
                }
                
                yield return Timing.WaitForOneFrame;
            }
        }
        
        public IEnumerator<float> TeleportPlayers()
        {
            yield return Timing.WaitForSeconds(1f);
            while (true)
            {
                yield return Timing.WaitForSeconds(RoundModifiers.Instance.Config.Scp249_TeleportCheckInterval);
                foreach (Player player in Player.List)
                {
                    for (int i = 0; i < DoorCount; i++)
                    {
                        if(Vector3.Distance(player.Position, Doors[i].Position) < 3.3f)
                        {
                            Door targetDoor = Doors[(i + 1) % DoorCount];
                            player.Teleport(targetDoor.Position+Vector3.up+(targetDoor.Rotation*Vector3.forward));//Teleport to the next door
                        }
                    }
                    foreach (Door door in Doors)
                    {
                        
                    }
                }
            }
        }
        
        
        public void OnRoundStart()
        {
            Start();
        }

        

        public Door GetRandomDoor()
        {
            Door newDoor = null;
            if(Map.DecontaminationState != DecontaminationState.Countdown && Map.DecontaminationState != DecontaminationState.Lockdown && Map.DecontaminationState != DecontaminationState.Finish)
                newDoor = Door.Random(ZoneType.LightContainment);
            if(newDoor.Rooms.Count!=2)
                newDoor = Door.Random(ZoneType.HeavyContainment);
            if(newDoor.Rooms.Count!=2)
                newDoor = Door.Random(ZoneType.Entrance);
            if (newDoor.Rooms.Count != 2)
                newDoor = Door.Random(ZoneType.Surface);
            if (newDoor.IsElevator || newDoor.IsGate) //Don't want to block an elevator, or use a gate since the marker will be the wrong size
                newDoor=Door.Random(ZoneType.HeavyContainment); //Issue finding new door
            return newDoor;
        }
        
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        }

        protected override void UnregisterModifier()
        {
            Stop();
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "SCP249",
            Aliases = new []{"249"},
            Description = "SCP-249 is loose in the facility.",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false
        };
    }
}