﻿using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MEC;
using RoundModifiers.API;
using TTCore.API;
using Random = UnityEngine.Random;

namespace RoundModifiers.Handlers
{
    public class RoundManager : IRegistered
    {
        public List<ModInfo> ActiveModifiers { get; private set; }
        
        public List<ModInfo> NextRoundModifiers { get; private set; }

        public bool ModifiersHidden { get; set; }
        public bool NextModifiersHidden { get; set; }

        public static ModInfo NoneInfo = new ModInfo
        {
            Name = "None",
            Description = "No modifier this round.",
            FormattedName = "None"
        };

        public int[] ModifierOrder;
        public int RoundNumber;

        private Dictionary<string, ModInfo> VotingPlayers;
        
        private List<string> BlacklistedModifiers;

        public bool VotingPaused { get; set; }
        
        public int NextRoundModCount { get; set; }

        public RoundManager()
        {
            ActiveModifiers = new List<ModInfo>();
            NextRoundModifiers = new List<ModInfo>();
            VotingPlayers = new Dictionary<string, ModInfo>();
            BlacklistedModifiers = new List<string>(); //RoundModifiers.Instance.Config.DisabledModifiers.ToList();
            ModifiersHidden = false;
            NextModifiersHidden = false;
            VotingPaused = false;
            NextRoundModCount = 1;
        }
        
        //**********************************************
        // Voting handlers
        //**********************************************

        public bool VotingEnabled()
        {
            return RoundModifiers.Instance.Config.EnableVoting && !VotingPaused;
        }
        
        public bool RemoveVote(Player player, out string response)
        {
            if (VotingEnabled())
            {
                if (VotingPlayers.ContainsKey(player.UserId))
                {
                    VotingPlayers.Remove(player.UserId);
                    response = "Removed vote.";
                    return true;
                }
                else
                {
                    response = "You have not voted yet.";
                    return false;
                }
            }
            else
            {
                response = "Voting is disabled.";
                return false;
            }
        }
        
        public bool RandomVote(Player player, out string response)
        {
            return TakeVote(player, RoundModifiers.Instance.Modifiers.Where(f=>!f.Key.Hidden).GetRandomValue().Key, out response);
        }
        
        public bool TakeVote(Player player, ModInfo modifier, out string response)
        {
            if (VotingEnabled())
            {
                //Do nothing if mod is blacklisted
                if(BlacklistedModifiers.Contains(modifier.Name) || modifier.Hidden)
                {
                    response = "This modifier is disabled.";
                    return false;
                }
                //If the player has already voted, remove their vote to take a new one
                if(VotingPlayers.ContainsKey(player.UserId))
                {
                    VotingPlayers.Remove(player.UserId);
                }

                VotingPlayers[player.UserId] = modifier;
                response= "Voted for modifier \""+modifier.Name+"\" this round.\n" +
                          "Current Votes: " + VotingPlayers.Count + "/" + Player.List.Count + " (" + Math.Round((double)VotingPlayers.Count/Player.List.Count*100,2) + "%)\n";
                return true;
            }
            else
            {
                response = "Voting is disabled.";
                return false;
            }
        }
        
        public string GetVoteInfo(Player player)
        {
            if (VotingEnabled())
            {
                string votes = "Current Votes: " + VotingPlayers.Count + "/" + Player.List.Count + " (" + Math.Round((double)VotingPlayers.Count/Player.List.Count*100,2) + "%)\n";
                votes+= "You have voted for:\n";
                if (VotingPlayers.ContainsKey(player.UserId))
                {
                    votes+= VotingPlayers[player.UserId].Name;
                }
                return votes;
            }
            else
            {
                return "Voting is disabled.";
            }
        }

        public void SelectNewModifiers()
        {
            //Hide modifiers if needed.
            ModifiersHidden = false;
            if(NextModifiersHidden) ModifiersHidden = true;
            NextModifiersHidden = false;
            
            Log.Debug("Next round mod count: " + NextRoundModifiers.Count);
            //First see if admin has set mods for next round
            if (NextRoundModifiers.Count != 0)
            {
                Log.Debug("Next round set:");
                foreach (ModInfo mod in NextRoundModifiers)
                {
                    Log.Debug(mod.Name);
                }
                if (NextRoundModifiers.Contains(NoneInfo))
                {
                    //ClearRoundModifiers();
                    NextRoundModifiers.Clear();
                    return;
                }
                
                SetRoundModifiers(NextRoundModifiers);
                NextRoundModifiers.Clear();
                return;
            }

            foreach (var mod in VotingPlayers)
            {
                Log.Info("Player: " + mod.Key + " voted for: " + mod.Value.Name);
            }



            int votes = 0;
            int players = Player.List.Count;

            foreach (string userId in VotingPlayers.Keys.ToList())
            {
                Player player = Player.Get(userId);
                if (player == null)
                {
                    VotingPlayers.Remove(userId);
                }
                else
                {
                    votes++;
                }
            }

            int count = Random.Range(0, 4);
            while (count-- > 0)
            {
                if (votes > 0)
                {
                    if (votes > players / 2)
                    {
                        //Majority vote. Guaranteed to be a winning modifier
                        ModInfo winningMod = VotingPlayers.Values.GetRandomValue();
                        if(CheckModifierValid(winningMod))
                            AddRoundModifier(winningMod);
                    }
                    else
                    {
                        //No majority vote, but still have a chance to pick a winning modifier
                        if(Random.Range(0f,1f) < ((float)votes/(float)players))
                        {
                            ModInfo winningMod = VotingPlayers.Values.GetRandomValue();
                            if(CheckModifierValid(winningMod))
                                AddRoundModifier(winningMod);
                        }
                    }
                }
                else
                {
                    //No votes, no modifier
                }
                //VotingPlayers.Clear();
                foreach(var pair in VotingPlayers.ToList())
                {
                    if (pair.Value.Name == NoneInfo.Name)
                    {
                        VotingPlayers.Remove(pair.Key);
                    }
                    else if(ActiveModifiers.Contains(pair.Value))
                    {
                        VotingPlayers.Remove(pair.Key);
                    }
                    else
                    {
                        continue;
                    }
                    votes--;
                }
            }
            
            

        }
        
        //**********************************************
        // Modifier handlers
        //**********************************************
        public void AddRoundModifier(ModInfo modifier)
        {
            if (!ActiveModifiers.Contains(modifier) && RoundModifiers.Instance.TryGetModifier(modifier.Name, out Modifier mod, exact:true))
            {
                mod.Register();
                ActiveModifiers.Add(modifier);
            }
        }
        
        public void SetRoundModifiers(List<ModInfo> modifiers)
        {
            ClearRoundModifiers();
            foreach(ModInfo mod in modifiers)
            {
                AddRoundModifier(mod);
            }
        }

        public bool CheckModifierValid(ModInfo modifier)
        {
            foreach(ModInfo mod in ActiveModifiers)
            {
                if(mod.Category.HasFlag(modifier.Category))
                {
                    return false;
                }
            }

            return true;
        }
        
        public void RemoveRoundModifier(ModInfo modifier)
        {
            if (ActiveModifiers.Contains(modifier))
            {
                RoundModifiers.Instance.TryGetModifier(modifier.Name, out Modifier mod, exact:true);
                mod?.Unregister();
                ActiveModifiers.Remove(modifier);
            }
        }
        
        public void ClearRoundModifiers()
        {
            foreach (var mod in ActiveModifiers)
            {
                RoundModifiers.Instance.TryGetModifier(mod.Name, out Modifier modifier, exact:true);
                modifier?.Unregister();
            }
            ActiveModifiers.Clear();
        }
        
        public void AddNextRoundModifier(ModInfo modifier)
        {
            if (!NextRoundModifiers.Contains(modifier))
            {
                NextRoundModifiers.Add(modifier);
            }
        }
        
        public void SetNextRoundModifiers(List<ModInfo> modifiers)
        {
            NextRoundModifiers.Clear();
            foreach(ModInfo mod in modifiers)
            {
                AddNextRoundModifier(mod);
            }
        }
        
        public void RemoveNextRoundModifier(ModInfo modifier)
        {
            if (NextRoundModifiers.Contains(modifier))
            {
                NextRoundModifiers.Remove(modifier);
            }
        }
        
        public void ClearNextRoundModifiers()
        {
            NextRoundModifiers.Clear();
        }
        
        
        //**********************************************
        // Event handlers
        // To handle this we will need to first get the users input at the start of a round. We'll use the .vote command but need to find where in the lifecycle we set the winning modifier.
        // Then need to have it decided before players start spawning/teams are picked. Code needs to run on these events
        // Some things will also need periodic checks
        // Will then need to reset the modify on round end.
        //**********************************************
        
        CoroutineHandle LobbyModViewCoroutine;

        public void OnWaitingForPlayers()
        {
            //ResetRoundModifier();
            if(RoundModifiers.Instance.Config.ShowRoundModInLobby)
                LobbyModViewCoroutine = Timing.RunCoroutine(LobbyModView());

            
            Log.Info("Lobby is starting.");
        }
        
        public void OnPlayerLeave(LeftEventArgs ev)
        {
            /*if (VotingPlayers.ContainsKey(ev.Player.UserId))
            {
                Log.Info("Clearing vote for leaving player " + ev.Player.Nickname);
                VotingPlayers.Remove(ev.Player.UserId);
            }*/
        }

        public IEnumerator<float> LobbyModView()
        {
            while (true)
            {
                if(!Round.IsLobby) break;
                string modString = "<size=75%>Modifiers: \n";
                if (ModifiersHidden)
                {
                    modString += "[REDACTED]";
                }
                else if (ActiveModifiers.Count > 0)
                {
                    int visibleMods = 0;
                    foreach(ModInfo modifier in ActiveModifiers)
                    {
                        if(modifier.Hidden) continue;
                        visibleMods++;
                        modString += $"{modifier.FormattedName}, ";
                    }
                    if(visibleMods > 0)
                        modString = modString.Remove(modString.Length - 2);
                    else
                    {
                        modString = "<size=75%>Modifiers: \nNone";//If no visible mods, show none
                    }
                }
                else
                {
                    modString += "None";
                }
                

                //todo: Add logic for when no voting will happen next round
                    modString +=
                        $"\n<size=50%>'.Vote' for next round: {VotingPlayers.Count}/{Player.List.Count} ({Math.Round((double)VotingPlayers.Count / Player.List.Count * 100, 2)}%)</size></size>";

                foreach (Player player in Player.List)
                {
                    if(player.IsNPC) continue;
                    string personalModString = modString;
                    if (VotingPlayers.ContainsKey(player.UserId))
                    {
                        personalModString += $"\n<size=50%>You voted for: {VotingPlayers[player.UserId].FormattedName}</size>";
                    } else
                    {
                        personalModString += $"\n<size=50%>You have not voted yet.</size>";
                    }
                    player.Broadcast(10, personalModString, Broadcast.BroadcastFlags.Normal, true);
                }   
                //Broadcast.Singleton.RpcAddElement(modString, 10, Broadcast.BroadcastFlags.Normal);
                yield return Timing.WaitForSeconds(10f);
            }
        }
        
        public void OnRoundStart()
        {
            try
            {
                Timing.KillCoroutines(LobbyModViewCoroutine);
                if (!RoundModifiers.Instance.Config.ShowRoundModInGame) return;
                string modString = "Modifiers:<size=75%> ";
                
                if (ModifiersHidden)
                {
                    modString += "[REDACTED]";
                }
                else if (ActiveModifiers.Count > 0)
                {
                    int addedMods = 0;
                    foreach(ModInfo modifier in ActiveModifiers)
                    {
                        if(modifier.Hidden) continue;
                        addedMods++;
                        modString += $"\n{modifier.FormattedName}";
                    }
                    if(addedMods == 0)
                        modString += "None";
                }
                else
                {
                    modString += "None";
                }
                modString += "</size>";
            
                Broadcast.Singleton.RpcAddElement(modString, 10, Broadcast.BroadcastFlags.Normal);
                Log.Debug("Round is starting.");
            } catch (Exception e)
            {
                Log.Error("Error in OnRoundStart: " + e);
            }
            
        }

        public void OnRoundRestart()
        {
            Log.Debug("Round is restarting.");
            try
            {
                ClearRoundModifiers();
                SelectNewModifiers();
            } catch (Exception e)
            {
                Log.Error("Error in OnRoundRestart: " + e);
            }
            
            Timing.KillCoroutines(LobbyModViewCoroutine);
        }

        public void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Left += OnPlayerLeave;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestart;
        }

        public void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Left -= OnPlayerLeave;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
            //ListPool<ModInfo>.Pool.Return(ActiveModifiers);
            //ListPool<ModInfo>.Pool.Return(NextRoundModifiers);
            //DictionaryPool<string, ModInfo>.Pool.Return(VotingPlayers);
            //ListPool<string>.Pool.Return(BlacklistedModifiers);
            Timing.KillCoroutines(LobbyModViewCoroutine);
        }
    }
}