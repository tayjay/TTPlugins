using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using PlayerRoles.PlayableScps.Scp079.Cameras;
using RoundModifiers.API;
using RoundModifiers.Modifiers.RogueAI.Abilities;
using TTCore.Extensions;
using TTCore.Utilities;
using UnityEngine;
using Camera = Exiled.API.Features.Camera;

namespace RoundModifiers.Modifiers.RogueAI
{
    public class RogueAI : Modifier
    {
        /*
         * - Call elevators
         * - Make announcements giving away positions of players/scps
         * - Will eventually pick a player to target and will try to kill them
         *      - Will lock doors when they are close to an SCP to trap them
         *      - If they're hiding in a room, will turn off lights and open the door
         *
         * At the start of the game the AI will pick a team that it will attempt to help. It will start by giving them assistance.
         *      - If it picks ClassD, it will try to help them escape, first by opening their cell doors to let them know it's helping them. Then giving them passage to the exit.
         *      - If it picks Foundation, it will attempt to help get the scientists out, and guide the guards to them.
         *      - If it picks SCPs, it will start by showing them the route to light containment.
         * It won't start taking action to harm until later in the game.
         */
        
        public int AggressionLevel = 0;
        public int MaxAggressionLevel = 10;
        public int CurrentTick = 0;
        public List<Ability> Abilities;
        public List<WeightedItem<Ability>> WeightedAbilities;
        public Ability CurrentAbility = null;
        //Which side the AI is currently helping
        public Side CurrentSide = Side.None;
        public CoroutineHandle AITickHandle, FollowHandler;
        public Npc npc;
        public Scp079Role Scp079 => npc.Role as Scp079Role;

        public RogueAI()
        {
            
        }
        
        public void OnRoundStart()
        {
            CurrentSide = Side.None;
            AggressionLevel = 0;
            CurrentTick = 0;
            CurrentAbility = null;
            AITickHandle = Timing.RunCoroutine(AITick());
            //FollowHandler = Timing.RunCoroutine(FollowPlayer());
            //TTCore.TTCore.Instance.NpcManager.SpawnNpc("RogueAI", RoleTypeId.Scp079, Vector3.zero, out npc);
        }

        public IEnumerator<float> AITick()
        {
            //Log.Info("Starting AI Tick");
            while (true)
            {
                CurrentTick++;
                if (CurrentTick % 60 == 0)
                {
                    //Log.Info("Changing aggression level.");
                    if(AggressionLevel < MaxAggressionLevel)
                        AggressionLevel++;
                    CurrentSide = GetLowestSideNonZero();
                    string sideName = "All";
                    switch (CurrentSide)
                    {
                        case Side.Mtf:
                            sideName = "Foundation";
                            break;
                        case Side.ChaosInsurgency:
                            sideName = "Chaos";
                            break;
                        case Side.Scp:
                            sideName = "SCP";
                            break;
                    }
                    new AnnouncementAbility($"{sideName} . Level {AggressionLevel}").Start();
                }

                if (CurrentAbility == null)
                {
                    Log.Info("Ability is Null");
                    ChooseNewAbility();
                } else if (!CurrentAbility.IsEnabled)
                {
                    //Log.Info("Ability is done.");
                    ChooseNewAbility();
                }
                
                yield return Timing.WaitForSeconds(1f);
            }
        }
        
        public IEnumerator<float> FollowPlayer()
        {
            Player target = null;
            float lastUpdate = 0;
            while (true)
            {
                if (npc == null)
                {
                    yield return Timing.WaitForSeconds(1f);
                    continue;
                }
                if (Time.time - lastUpdate > 10f )
                {
                    lastUpdate = Time.time;
                    target = Player.List.FirstOrDefault(x => x.Role.IsAlive && x.Role.Type!=RoleTypeId.Scp079);
                    Log.Info("Looking for new target. "+target?.Nickname);
                }

                if (target == null)
                {
                    yield return Timing.WaitForSeconds(1f);
                    continue;
                }
                Camera camera = target?.CurrentRoom.Cameras.FirstOrDefault();
                if (camera != null)
                {
                    Scp079.Camera = camera;
                }

                try
                {
                    Log.Info("Starting to check for rotation.");
                    Vector3 direction = target.Position - Scp079.CameraPosition;
                    Quaternion quat = Quaternion.LookRotation(direction, Vector3.up);
                    (ushort horizontal, ushort vertical) = quat.ToClientUShorts();
                    npc.Rotation = quat;
                    Scp079.Camera.Rotation = quat;
                    Scp079.Camera.Transform.SetPositionAndRotation(Scp079.CameraPosition, quat);
                    Scp079.Camera.Transform.rotation = quat;
                    npc.ReferenceHub.PlayerCameraReference.rotation = quat;
                    Scp079.CurrentCameraSync.CurrentCamera.HorizontalRotation = horizontal;
                    Scp079.CurrentCameraSync.CurrentCamera.VerticalRotation = vertical;
                    Log.Info("Finished rotation. "+Scp079.Camera.Rotation.eulerAngles+" in "+Scp079.Camera.Room.Type);
                    
                } catch (System.Exception e)
                {
                    Log.Error("Error rotating to target: "+e);
                }
                yield return Timing.WaitForSeconds(0.1f);
            }
        }

        public void ChooseNewAbility()
        {
            //Log.Info("Looking for new ability to use.");
            Ability NewAbility = null;
            //Get all abilities that are of or below the current aggression level
            //List<Ability> possibleAbilities = Abilities.Where(x => x.AggressionLevel <= AggressionLevel && (x.HelpingSide==CurrentSide|| x.HelpingSide==Side.None)).ToList();
            
            //Pick a random ability
            try
            {
                List<WeightedItem<Ability>> WeightedOptions = WeightedAbilities.Where(x => x.Item.AggressionLevel <= AggressionLevel && (x.Item.HelpingSide==CurrentSide|| x.Item.HelpingSide==Side.None)).ToList();
                WeightedRandomSelector<Ability> selector = new WeightedRandomSelector<Ability>(WeightedOptions);
                while(WeightedOptions.Count>0)
                {
                    NewAbility = selector.SelectItem();
                    //Log.Info("Trying to setup new ability "+NewAbility.Name);
                    if (NewAbility.Setup())
                    {
                        Log.Info("Setting up new ability "+NewAbility.Name);
                        CurrentAbility = NewAbility;
                        CurrentAbility.Start();
                        break;
                    }
                    WeightedOptions.Remove(WeightedOptions.First(x => x.Item == NewAbility));
                }
            } catch (System.Exception e)
            {
                Log.Error("Error setting up new ability: "+e);
            }
            
        }

        public Side GetLeadingSide()
        {
            Dictionary<Side,int> sidePoints = new Dictionary<Side, int>();
            foreach (Player player in Player.List)
            {
                (Side side, int points) = RolePointValue(player.Role);
                if(side==Side.None) continue;
                if (sidePoints.ContainsKey(side))
                {
                    sidePoints[side] += points;
                }
                else
                {
                    sidePoints[side] = points;
                }
            }
            //Return which of the 3 sides have the most points
            int mostPoints = 0;
            Side leadingSide = Side.None;
            foreach(Side side in sidePoints.Keys)
            {
                if (sidePoints[side] > mostPoints)
                {
                    mostPoints = sidePoints[side];
                    leadingSide = side;
                }
            }
            return leadingSide;
        }
        
        public Side GetLowestSideNonZero()
        {
            Dictionary<Side,int> sidePoints = new Dictionary<Side, int>();
            sidePoints[Side.ChaosInsurgency] = 0;
            sidePoints[Side.Scp] = 0;
            sidePoints[Side.Mtf] = 0;   
            foreach (Player player in Player.List)
            {
                (Side side, int points) = RolePointValue(player.Role);
                if(side==Side.None) continue;
                if (sidePoints.ContainsKey(side))
                {
                    sidePoints[side] += points;
                }
                else
                {
                    sidePoints[side] = points;
                }
            }
            //Return which of the 3 sides have the least points
            int leastPoints = int.MaxValue;
            Side lowestSide = Side.None;
            foreach(Side side in sidePoints.Keys)
            {
                if (sidePoints[side]!=0 && sidePoints[side] < leastPoints)
                {
                    leastPoints = sidePoints[side];
                    lowestSide = side;
                }
            }
            return lowestSide;
        }

        public (Side, int) RolePointValue(RoleTypeId roleTypeId)
        {
            switch (roleTypeId)
            {
                //Chaos Roles
                case RoleTypeId.ClassD:
                    return (Side.ChaosInsurgency, 1);
                case RoleTypeId.ChaosConscript:
                case RoleTypeId.ChaosMarauder:
                case RoleTypeId.ChaosRepressor:
                case RoleTypeId.ChaosRifleman:
                    return (Side.ChaosInsurgency, 2);
                //Scp Roles
                case RoleTypeId.Scp0492:
                    return (Side.Scp, 1);
                case RoleTypeId.Scp079:
                case RoleTypeId.Scp3114:
                    return (Side.Scp, 2);
                case RoleTypeId.Scp096:
                case RoleTypeId.Scp049:
                case RoleTypeId.Scp106:
                case RoleTypeId.Scp173:
                case RoleTypeId.Scp939:
                    return (Side.Scp, 3);
                //Foundation Roles
                case RoleTypeId.Scientist:
                    return (Side.Mtf, 1);
                case RoleTypeId.FacilityGuard:
                case RoleTypeId.NtfSpecialist:
                case RoleTypeId.NtfPrivate:
                case RoleTypeId.NtfSergeant:
                    return (Side.Mtf, 2);
                case RoleTypeId.NtfCaptain:
                    return (Side.Mtf, 3);
                //Other
                default:
                    return (Side.None, 0);
            }
        }

        public void TryOpenDoor(InteractingDoorEventArgs ev)
        {
            if (AggressionLevel < 2) return;
            if (ev.Player.Role.Side == CurrentSide)
            {
                //Log.Info("Player trying to open a door.");
                //Help open the door if it's locked for them
                bool isOpen = ev.Door.IsOpen;
                if(!ev.Door.AllowsScp106) return; //Don't open doors that 106 can't go through, like 079 gate
                MEC.Timing.CallDelayed(1f, () =>
                {
                    if(ev.Door.IsOpen != isOpen)
                        return;
                    ev.Door.IsOpen = !isOpen;
                });
            }

            if (ev.Player.Role.Side == GetLeadingSide() && CurrentSide != Side.None)
            {
                //Occasionally close doors that they open
                bool isOpen = ev.Door.IsOpen;
                
                if (Random.Range(0, 100) < AggressionLevel)
                {
                    MEC.Timing.CallDelayed(1f, () =>
                    {
                        ev.Door.IsOpen = isOpen;//Revert to what it was before after 1 second
                    });
                }
            }
        }


        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Player.InteractingDoor += TryOpenDoor;

            Abilities = ListPool<Ability>.Pool.Get();
            Abilities.Add(new LockDoorAbility());
            //Abilities.Add(new LockDoorAbility());
            //Abilities.Add(new LockDoorAbility());
            Abilities.Add(new LockdownRoomAbility());
            Abilities.Add(new TouchRandomDoorAbility());
            //Abilities.Add(new TouchRandomDoorAbility());
            //Abilities.Add(new TouchRandomDoorAbility());
            //Abilities.Add(new TouchRandomDoorAbility());
            //Abilities.Add(new TouchRandomDoorAbility());
            Abilities.Add(new TeslaGateAbility());
            Abilities.Add(new ChangeLightAbility());
            //Abilities.Add(new ChangeLightAbility());
            //Abilities.Add(new ChangeLightAbility());
            Abilities.Add(new Scp914Ability());
            //Abilities.Add(new Scp914Ability());
            Abilities.Add(new AnnouncementAbility(".", isNoisy:true, aggressionLevel: 4));
            Abilities.Add(new AnnouncementAbility(".g1", isHeld: true));
            Abilities.Add(new AnnouncementAbility(".g7", aggressionLevel: 4));
            Abilities.Add(new AnnouncementAbility("XMAS_BOUNCYBALLS", aggressionLevel: 6, oneShot:true));
            //Abilities.Add(new AnnouncementAbility("You think your safe . I see you there", aggressionLevel: 7));
            Abilities.Add(new AnnouncementAbility("security seriously needs to get their heads checked . psi", aggressionLevel: 8, oneShot:true));
            Abilities.Add(new TattleAbility());
            Abilities.Add(new ActivateNukeAbility(Side.ChaosInsurgency));
            Abilities.Add(new ActivateNukeAbility(Side.Mtf));
            Abilities.Add(new BlackoutAbility());
            Abilities.Add(new BlackoutAbility(Side.Scp, 7));
            Abilities.Add(new ElevatorAbility());
            
            
            
            
            WeightedAbilities = ListPool<WeightedItem<Ability>>.Pool.Get();
            
            WeightedAbilities.Add(new WeightedItem<Ability>(new LockDoorAbility(), 3));
            WeightedAbilities.Add(new WeightedItem<Ability>(new LockdownRoomAbility(), 2));
            WeightedAbilities.Add(new WeightedItem<Ability>(new TouchRandomDoorAbility(), 5));
            WeightedAbilities.Add(new WeightedItem<Ability>(new TeslaGateAbility(), 2));
            WeightedAbilities.Add(new WeightedItem<Ability>(new ChangeLightAbility(), 4));
            WeightedAbilities.Add(new WeightedItem<Ability>(new Scp914Ability(), 4));
            WeightedAbilities.Add(new WeightedItem<Ability>(new AnnouncementAbility(".", isNoisy:true, aggressionLevel: 4),2));
            WeightedAbilities.Add(new WeightedItem<Ability>(new AnnouncementAbility(".g1", isHeld: true), 3));
            WeightedAbilities.Add(new WeightedItem<Ability>(new AnnouncementAbility(".g7", aggressionLevel: 4), 2));
            //WeightedAbilities.Add(new WeightedItem<Ability>(new AnnouncementAbility("XMAS_BOUNCYBALLS", aggressionLevel: 6, oneShot:true), 2));
            WeightedAbilities.Add(new WeightedItem<Ability>(new BallsAbility("Balls", "Spawns bouncy balls in random rooms", Side.None), 2));
            WeightedAbilities.Add(new WeightedItem<Ability>(new AnnouncementAbility("security seriously needs to get their heads checked . psi", aggressionLevel: 8, oneShot:true), 2));
            WeightedAbilities.Add(new WeightedItem<Ability>(new TattleAbility(), 2));
            WeightedAbilities.Add(new WeightedItem<Ability>(new ActivateNukeAbility(Side.ChaosInsurgency), 1));
            WeightedAbilities.Add(new WeightedItem<Ability>(new ActivateNukeAbility(Side.Mtf), 1));
            WeightedAbilities.Add(new WeightedItem<Ability>(new BlackoutAbility(), 2));
            WeightedAbilities.Add(new WeightedItem<Ability>(new BlackoutAbility(Side.Scp, 7), 2));
            WeightedAbilities.Add(new WeightedItem<Ability>(new ElevatorAbility(), 4));
            
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Player.InteractingDoor -= TryOpenDoor;
            
            CurrentAbility?.End();
            CurrentAbility = null;
            CurrentSide = Side.None;
            
            Timing.KillCoroutines(AITickHandle);
            //Timing.KillCoroutines(FollowHandler);
            
            ListPool<Ability>.Pool.Return(Abilities);
            ListPool<WeightedItem<Ability>>.Pool.Return(WeightedAbilities);
        }

        

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "RogueAI",
            FormattedName = "<color=red>Rogue AI</color>",
            Aliases = new []{"ai","rougeai"},
            Description = "An AI that will try to help or hinder the players based on who is currently winning.",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false,
            Balance = 0,
            Category = Category.Facility
        };
            
    }
}