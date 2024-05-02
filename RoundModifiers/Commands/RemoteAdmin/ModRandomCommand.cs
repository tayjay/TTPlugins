using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features.Pools;
using Exiled.Permissions.Extensions;
using RoundModifiers.API;
using Random = UnityEngine.Random;

namespace RoundModifiers.Commands.RemoteAdmin;

public class ModRandomCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        if (sender.CheckPermission("RoundEvents") || sender.CheckPermission("modifier") || sender.CheckPermission("RoundEvents*"))
        {
            List<string> modifiers = ListPool<string>.Pool.Get();
            
            if (arguments.Count == 0)
            {
                //Modifiy this round
                RoundModifiers.Instance.RoundManager.ClearRoundModifiers();
                var mods = RoundModifiers.Instance.Modifiers.Where(pair => !pair.Key.MustPreload); 
                int randomCount = Random.Range(2, 6);
                while(randomCount-- > 0)
                {
                    var mod = mods.GetRandomValue();
                    if(RoundModifiers.Instance.RoundManager.ActiveModifiers.Contains(mod.Key))
                        continue;
                    modifiers.Add(mod.Key.Name);
                    RoundModifiers.Instance.RoundManager.AddRoundModifier(mod.Value.ModInfo);
                }

            } else if (arguments.Count == 1 && arguments.At(0).ToLower() == "next")
            {
                //Modify next round
                RoundModifiers.Instance.RoundManager.ClearNextRoundModifiers();
                var mods = RoundModifiers.Instance.Modifiers;
                int randomCount = Random.Range(2, 6);
                while(randomCount-- > 0)
                {
                    var mod = mods.GetRandomValue();
                    if(RoundModifiers.Instance.RoundManager.NextRoundModifiers.Contains(mod.Key))
                        continue;
                    modifiers.Add(mod.Key.Name);
                    RoundModifiers.Instance.RoundManager.AddNextRoundModifier(mod.Value.ModInfo);
                }
            } else
            {
                response = "Usage: mod random [next]";
                return false;
            }
            response = $"Added modifiers: {string.Join(", ", modifiers)} to the {(arguments.Count == 0 ? "current" : "next")} round.";
            ListPool<string>.Pool.Return(modifiers);
            return true;
        }

        response = "You do not have permission to use this random command.";
        return false;
    }

    public string Command { get; } = "random";
    public string[] Aliases { get; } = { "rand" };
    public string Description { get; } = "Randomly selects modifiers for this or next round.";
}