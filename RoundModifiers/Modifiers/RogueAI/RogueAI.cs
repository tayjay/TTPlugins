﻿using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using RoundModifiers.API;
using RoundModifiers.Modifiers.RogueAI.Abilities;
using UnityEngine;

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
        public List<Ability> Abilities = new List<Ability>();
        public Ability CurrentAbility = null;
        //Which side the AI is currently helping
        public Side CurrentSide = Side.None;
        public CoroutineHandle AITickHandle;

        public RogueAI()
        {
            
            Abilities.Add(new LockDoorAbility());
            Abilities.Add(new LockDoorAbility());
            Abilities.Add(new LockDoorAbility());
            Abilities.Add(new LockdownRoomAbility());
            Abilities.Add(new TouchRandomDoorAbility());
            Abilities.Add(new TouchRandomDoorAbility());
            Abilities.Add(new TouchRandomDoorAbility());
            Abilities.Add(new TouchRandomDoorAbility());
            Abilities.Add(new TouchRandomDoorAbility());
            Abilities.Add(new TeslaGateAbility());
            Abilities.Add(new ChangeLightAbility());
            Abilities.Add(new ChangeLightAbility());
            Abilities.Add(new ChangeLightAbility());
            Abilities.Add(new Scp914Ability());
            Abilities.Add(new Scp914Ability());
            Abilities.Add(new AnnouncementAbility(".", isNoisy:true, aggressionLevel: 4));
            Abilities.Add(new AnnouncementAbility(".g1", isHeld: false));
            Abilities.Add(new AnnouncementAbility(".g7", aggressionLevel: 4));
            
            Abilities.Add(new AnnouncementAbility("XMAS_BOUNCYBALLS", aggressionLevel: 6));
            //Abilities.Add(new AnnouncementAbility("You think your safe . I see you there", aggressionLevel: 7));
            Abilities.Add(new AnnouncementAbility("security seriously needs to get their heads checked . psi", aggressionLevel: 8));
            Abilities.Add(new TattleAbility());
            Abilities.Add(new ActivateNukeAbility(Side.ChaosInsurgency));
            Abilities.Add(new ActivateNukeAbility(Side.Mtf));
            
            Abilities.Add(new BlackoutAbility());
            Abilities.Add(new BlackoutAbility(Side.Scp, 7));
        }
        
        public void OnRoundStart()
        {
            CurrentSide = Side.None;
            AggressionLevel = 0;
            CurrentTick = 0;
            CurrentAbility = null;
            AITickHandle = Timing.RunCoroutine(AITick());
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

        public void ChooseNewAbility()
        {
            //Log.Info("Looking for new ability to use.");
            Ability NewAbility = null;
            //Get all abilities that are of or below the current aggression level
            List<Ability> possibleAbilities = Abilities.Where(x => x.AggressionLevel <= AggressionLevel && (x.HelpingSide==CurrentSide|| x.HelpingSide==Side.None)).ToList();
            
            //Pick a random ability
            try
            {
                while(possibleAbilities.Count>0)
                {
                    NewAbility = possibleAbilities[Random.Range(0, possibleAbilities.Count)];
                    //Log.Info("Trying to setup new ability "+NewAbility.Name);
                    if (NewAbility.Setup())
                    {
                        Log.Info("Setting up new ability "+NewAbility.Name);
                        CurrentAbility = NewAbility;
                        CurrentAbility.Start();
                        break;
                    }
                    possibleAbilities.Remove(NewAbility);
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
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Player.InteractingDoor -= TryOpenDoor;
            
            CurrentAbility?.End();
            CurrentAbility = null;
            CurrentSide = Side.None;
            
            Timing.KillCoroutines(AITickHandle);
        }

        

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "RogueAI",
            Aliases = new []{"ai"},
            Description = "An AI that will try to help or hinder the players based on the current game state.",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false
        };
            
    }
}