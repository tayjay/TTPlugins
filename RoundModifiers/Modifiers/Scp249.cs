using System;
using System.Collections.Generic;
using System.ComponentModel;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pickups.Projectiles;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Roles;
using Exiled.API.Features.Toys;
using Exiled.API.Structs;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Warhead;
using MEC;
using PlayerRoles.FirstPersonControl;
using RoundModifiers.API;
using TTCore.Components;
using TTCore.Events.EventArgs;
using UnityEngine;

namespace RoundModifiers.Modifiers
{
    public class Scp249 : Modifier
    {
        //todo: Add check to remove door from light after decontamination
        //todo: see if there's anything to be done with transparency of the door not working right
        
        private CoroutineHandle _doorMovementCoroutine;
        private CoroutineHandle _teleportCoroutine;
        
        public Dictionary<GameObject, float> LastTeleportTime { get; private set; }
        
        public Door[] Doors { get; private set; }
        
        public Primitive[] DoorMarkers { get; private set; }
        
        public int PlacingDoor { get; private set; }

        
        
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
                new Vector3(1.4f, 2.5f, 0.005f), true);
                DoorMarkers[i] = Primitive.Create(settings);
                DoorMarkers[i].Collidable = false;
                DoorMarkers[i].AdminToyBase.gameObject.AddComponent<AdminToyCollisionHandler>().Init(DoorMarkers[i].AdminToyBase);
                Log.Info("Door in "+Doors[i].Room.Type);
            }
            LastPlaceTime = Time.time;
            _doorMovementCoroutine = Timing.RunCoroutine(MoveDoors());
            //_teleportCoroutine = Timing.RunCoroutine(TeleportPlayers());
        }

        public void Stop()
        {
            if(DoorMarkers == null || Doors == null) return;
            for(int i = 0; i < DoorCount; i++)
            {
                if(DoorMarkers[i] == null || Doors[i] == null) continue;
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
                try
                {
                    if(Time.time-LastPlaceTime > MoveInterval)
                    {
                        Door newDoor = GetRandomDoor();
                        if (newDoor != null)
                        {
                            Doors[PlacingDoor] = newDoor;
                            DoorMarkers[PlacingDoor].Destroy();
                            PrimitiveSettings settings = new PrimitiveSettings(PrimitiveType.Cube, new Color(10f,10f,10f, 0.15f),
                                Doors[PlacingDoor].Position + new Vector3(0,1.2f,0)-(Doors[PlacingDoor].Rotation*new Vector3(0,0,0.001f)),
                                Doors[PlacingDoor].Base.transform.eulerAngles*-1,
                                new Vector3(1.4f, 2.5f, 0.05f), true, true);
                            DoorMarkers[PlacingDoor] = Primitive.Create(settings);
                            DoorMarkers[PlacingDoor].Collidable = false;
                            DoorMarkers[PlacingDoor].AdminToyBase.gameObject.AddComponent<AdminToyCollisionHandler>().Init(DoorMarkers[PlacingDoor].AdminToyBase);
                            LastPlaceTime = Time.time;
                            Doors[PlacingDoor].IsOpen = true;
                            Log.Info("New door in "+Doors[PlacingDoor].Room.Type);
                            PlacingDoor = (PlacingDoor + 1) % DoorCount;
                        }
                        else
                        {
                            LastPlaceTime = Time.time;
                            Doors[PlacingDoor].IsOpen = true;
                        }
                    }
                
                
                
                    if(Time.time-LastPlaceTime > MoveInterval-1f)
                    {
                        //Want to start fading the door that is changing to red to indicate it will be the next door to move
                        DoorMarkers[PlacingDoor].Color = Color.Lerp(DoorMarkers[PlacingDoor].Color, new Color(0f,0f,0f, 0f), 0.1f);
                        Doors[PlacingDoor].IsOpen = false;
                    } else if(Time.time-LastPlaceTime > MoveInterval-10f)
                    {
                        //Want to start fading the door that is changing to red to indicate it will be the next door to move
                        DoorMarkers[PlacingDoor].Color = Color.Lerp(DoorMarkers[PlacingDoor].Color, new Color(10f,0f,0f, 0.15f), 0.01f);
                    }
                } catch(Exception e)
                {
                    Log.Error("Error moving doors");
                    Log.Error(e);
                }
                
                
                yield return Timing.WaitForOneFrame;
            }
        }

        public void OnCollideDoor(AdminToyCollisionEventArgs ev)
        {
            /*if(ev.Player==null)
            {
                
                return;
            }*/
            if (ev.GameObject == null) return;
            if(LastTeleportTime.TryGetValue(ev.GameObject, out float time) && Time.time - time < 3f) return;
            //Log.Info("Hit on "+ev.Player?.Nickname);
            for (int i = 0; i < DoorCount; i++)
            {
                if(ev.AdminToy == DoorMarkers[i].AdminToyBase)
                {
                    Door targetDoor = Doors[(i + 1) % DoorCount];
                    if (ev.Player != null)
                    {
                        //Player has touched a door
                        Log.Info("Teleporting player");
                        ev.Player?.Teleport(targetDoor.Position+Vector3.up);
                        /*if (ev.Player.Role is FpcRole role) todo: Rotation appears to be controlled by client, see if there is a way to sync it back to the client
                        {
                            role.FirstPersonController.FpcModule.MouseLook.LookAtDirection(targetDoor.Rotation.eulerAngles, 360);
                        }*/
                    } else if (Pickup.Get(ev.GameObject) != null)
                    {
                        Log.Info("Pickup touched door");
                        Pickup pickup = Pickup.Get(ev.GameObject);
                        pickup.Position = targetDoor.Position + Vector3.up;
                        //Pickup has touched a door
                    } else if(Projectile.Get(ev.GameObject) != null)
                    {
                        Log.Info("Projectile touched door");
                        Pickup projectile = Projectile.Get(ev.GameObject);
                        projectile.Position = targetDoor.Position + Vector3.up;
                        //Projectile has touched a door
                    }
                    LastTeleportTime[ev.GameObject] = Time.time;
                    
                    return;
                }
            }
        }
        
        public IEnumerator<float> TeleportPlayers()
        {
            yield return Timing.WaitForSeconds(1f);
            while (true)
            {
                yield return Timing.WaitForSeconds(TeleportCheckInterval);
                foreach (Player player in Player.List)
                {
                    try
                    {
                        for (int i = 0; i < DoorCount; i++)
                        {
                            if (Vector3.Distance(player.Position, Doors[i].Position + Vector3.up) < 0.65f)
                            {
                                //Stop player from teleporting again if they just did
                                if(player.SessionVariables.ContainsKey("Scp249_LastTeleport") && Time.time - (float)player.SessionVariables["Scp249_LastTeleport"] < 3f) continue;
                                Log.Info("Attempting to teleport " + player.Nickname);
                                Door targetDoor = Doors[(i+1) % (Doors.Length)];
                                player.Teleport(targetDoor.Position + Vector3.up);//+ (targetDoor.Rotation * Vector3.forward)); //Teleport to the next door
                                player.Rotation = targetDoor.Rotation;
                                player.SessionVariables["Scp249_LastTeleport"] = Time.time;
                                break;
                            }
                        }
                    }
                    catch
                    {
                        Log.Error("Error teleporting "+player.Nickname);
                    }

                    yield return Timing.WaitForOneFrame;
                }
            }
        }
        
        public void OnDetonated(DetonatingEventArgs ev)
        {
            Stop();
        }
        
        public void OnRoundStart()
        {
            Start();
        }

        

        public Door GetRandomDoor()
        {
            Door newDoor = null;
            if(Map.DecontaminationState != DecontaminationState.Countdown && Map.DecontaminationState != DecontaminationState.Lockdown && Map.DecontaminationState != DecontaminationState.Finish && Map.DecontaminationState != DecontaminationState.Remain1Minute)
                newDoor = Door.Random(ZoneType.LightContainment);
            if(newDoor.Rooms.Count!=2)
                newDoor = Door.Random(ZoneType.HeavyContainment);
            if(newDoor.Rooms.Count!=2)
                newDoor = Door.Random(ZoneType.Entrance);
            if (newDoor.Rooms.Count != 2)
                newDoor = Door.Random(ZoneType.Surface);
            if (newDoor.IsElevator ||
                newDoor.IsGate) //Don't want to block an elevator, or use a gate since the marker will be the wrong size
                newDoor = null; //Issue finding new door
            return newDoor;
        }
        
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            TTCore.Events.Handlers.Custom.AdminToyCollision += OnCollideDoor;
            Exiled.Events.Handlers.Warhead.Detonating += OnDetonated;

            LastTeleportTime = DictionaryPool<GameObject, float>.Pool.Get();
        }

        protected override void UnregisterModifier()
        {
            Stop();
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            TTCore.Events.Handlers.Custom.AdminToyCollision -= OnCollideDoor;
            Exiled.Events.Handlers.Warhead.Detonating -= OnDetonated;
            
            DictionaryPool<GameObject, float>.Pool.Return(LastTeleportTime);
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "SCP249",
            FormattedName = "<color=purple>SCP-249</color>",
            Aliases = new []{"249"},
            Description = "SCP-249 is loose in the facility.",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false,
            Balance = 1,
            Category = Category.Facility
        };
        
        public Config Scp249Config => RoundModifiers.Instance.Config.Scp249;
        public int DoorCount => Scp249Config.DoorCount;
        public float MoveInterval => Scp249Config.MoveInterval;
        public float TeleportCheckInterval => Scp249Config.TeleportCheckInterval;
        
        public class Config
        {
            [Description("The amount of time in seconds between door movements during the Scp249 modifier. Default is 30f. Suggest lowering this with larget DoorCount values.")]
            public float MoveInterval { get; set; } = 30f;
            [Description("The amount of time in seconds between testing for telepoting a player. Default is 0.1f. Lower numbers will be more accurate but more resource intensive.")]
            public float TeleportCheckInterval { get; set; } = 0.1f;
            [Description("Number of doors to place during the Scp249 modifier. Default is 2. Must be 2 or greater.")]
            public int DoorCount { get; set; } = 2;
        }
    }
}