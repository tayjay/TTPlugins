using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Permissions.Extensions;
using RoundModifiers.API;
using Random = UnityEngine.Random;

namespace RoundModifiers.Commands.RemoteAdmin;

public class ModRandomCommand : ICommand
{
    public const int MinModifiers = 2;
    public const int MaxModifiers = 6;
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        if (sender.CheckPermission("RoundEvents") || sender.CheckPermission("modifier") || sender.CheckPermission("RoundEvents*"))
        {
            List<string> modifiers = ListPool<string>.Pool.Get();
            List<ModInfo> modInfos = ListPool<ModInfo>.Pool.Get();
            int balance = 0;
            
            if (arguments.Count == 0)
            {
                if(Round.InProgress)
                {
                    response = "You cannot set modifiers while a round is in progress. Please use 'mod random next' to set modifiers for the next round.";
                    return false;
                }
                
                //Modifiy this round
                RoundModifiers.Instance.RoundManager.ClearRoundModifiers();
                var mods = RoundModifiers.Instance.Modifiers.Where(pair => !pair.Key.MustPreload && pair.Key.Impact!=ImpactLevel.Gamemode); 
                int attempts = 0;
                
                //New balanced random
                while(modInfos.Count < MaxModifiers || attempts < 10)
                {
                    attempts++;
                    ModInfo nextMod;
                    //Pick a new modifier based on current balance
                    // If the balance is greater than 1, pick a negative modifier
                    // If the balance is less than -1, pick a positive modifier
                    // If the balance is greater than 0, pick a negative or 0 modifier
                    // If the balance is less than 0, pick a positive or 0 modifier
                    // If the balance is 0, pick any modifier
                    if (balance > 1)
                    {
                        //Pick an exclusive negative modifier
                        nextMod = mods.GetRandomValue(m=>m.Key.Balance < 0).Key;
                    }
                    else if (balance < -1)
                    {
                        //Pick an exclusive positive modifier
                        nextMod = mods.GetRandomValue(m=>m.Key.Balance > 0).Key;
                    }
                    else if (balance > 0)
                    {
                        //Pick a negative modifier
                        nextMod = mods.GetRandomValue(m=>m.Key.Balance <= 0).Key;
                    } else if (balance < 0)
                    {
                        //Pick a positive modifier
                        nextMod = mods.GetRandomValue(m=>m.Key.Balance >= 0).Key;
                    }
                    else
                    {
                        //Pick any modifier
                        nextMod = mods.GetRandomValue().Key;
                    }
                    if(modInfos.Contains(nextMod))
                        continue;
                    // Check if the modifier category conflicts with any already selected.
                    bool valid = true;
                    foreach(var mod in modInfos)
                    {
                        if(nextMod.Category.HasFlag(mod.Category))
                            valid = false;
                    }
                    if(!valid)
                        continue;
                    modInfos.Add(nextMod);
                    balance += nextMod.Balance;
                    if(modInfos.Count >= MinModifiers && balance == 0)
                        break;
                }
                foreach (var mod in modInfos)
                {
                    modifiers.Add(mod.Name);
                    RoundModifiers.Instance.RoundManager.AddRoundModifier(mod);
                }
                

            } else if (arguments.Count == 1 && arguments.At(0).ToLower() == "next")
            {
                //Modify next round
                RoundModifiers.Instance.RoundManager.ClearNextRoundModifiers();
                var mods = RoundModifiers.Instance.Modifiers.Where(pair => pair.Key.Impact!=ImpactLevel.Gamemode); 
                int attempts = 0;
                while(modInfos.Count < MaxModifiers || attempts < 10)
                {
                    attempts++;
                    ModInfo nextMod;
                    //Pick a new modifier based on current balance
                    // If the balance is greater than 1, pick a negative modifier
                    // If the balance is less than -1, pick a positive modifier
                    // If the balance is greater than 0, pick a negative or 0 modifier
                    // If the balance is less than 0, pick a positive or 0 modifier
                    // If the balance is 0, pick any modifier
                    if (balance > 1)
                    {
                        //Pick an exclusive negative modifier
                        nextMod = mods.GetRandomValue(m=>m.Key.Balance < 0).Key;
                    }
                    else if (balance < -1)
                    {
                        //Pick an exclusive positive modifier
                        nextMod = mods.GetRandomValue(m=>m.Key.Balance > 0).Key;
                    }
                    else if (balance > 0)
                    {
                        //Pick a negative modifier
                        nextMod = mods.GetRandomValue(m=>m.Key.Balance <= 0).Key;
                    } else if (balance < 0)
                    {
                        //Pick a positive modifier
                        nextMod = mods.GetRandomValue(m=>m.Key.Balance >= 0).Key;
                    }
                    else
                    {
                        //Pick any modifier
                        nextMod = mods.GetRandomValue().Key;
                    }
                    if(modInfos.Contains(nextMod))
                        continue;
                    // Check if the modifier category conflicts with any already selected.
                    bool valid = true;
                    foreach(var mod in modInfos)
                    {
                        if(nextMod.Category.HasFlag(mod.Category))
                            valid = false;
                    }
                    if(!valid)
                        continue;
                    modInfos.Add(nextMod);
                    balance += nextMod.Balance;
                    if(modInfos.Count >= MinModifiers && balance == 0)
                        break;
                }
                
                foreach (var mod in modInfos)
                {
                    modifiers.Add(mod.Name);
                    RoundModifiers.Instance.RoundManager.AddNextRoundModifier(mod);
                }
            } else
            {
                response = "Usage: mod random [next]";
                return false;
            }
            response = $"Added modifiers: {string.Join(", ", modifiers)} to the {(arguments.Count == 0 ? "current" : "next")} round.\nBalance: {balance}";
            ListPool<string>.Pool.Return(modifiers);
            ListPool<ModInfo>.Pool.Return(modInfos);
            return true;
        }

        response = "You do not have permission to use this random command.";
        return false;
    }

    public string Command { get; } = "random";
    public string[] Aliases { get; } = { "rand" };
    public string Description { get; } = "Randomly selects modifiers for this or next round.";
    public bool SanitizeResponse => true;
}