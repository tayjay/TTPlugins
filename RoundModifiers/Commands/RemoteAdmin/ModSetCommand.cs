﻿
using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Permissions.Extensions;
using RoundModifiers.API;

namespace RoundModifiers.Commands.RemoteAdmin
{
    public class ModSetCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Player player = Player.Get((CommandSender)sender);
            if(sender.CheckPermission("RoundEvents") || sender.CheckPermission("modifier") || sender.CheckPermission("RoundEvents*"))
            {
                if (arguments.Count < 1)
                {
                    response = "Usage: mod set <modifier>";
                    return false;
                }
                
                if(Round.InProgress)
                {
                    response = "You cannot set modifiers while a round is in progress. Please use 'mod next' to set modifiers for the next round.";
                    return false;
                }

                List<ModInfo> modInfo = ListPool<ModInfo>.Pool.Get();
                
                foreach (string strMod in arguments)
                {
                    if(RoundModifiers.Instance.TryGetModifier(strMod, out Modifier mod1))
                    {
                        modInfo.Add(mod1.ModInfo);
                    }
                }
                RoundModifiers.Instance.RoundManager.SetRoundModifiers(modInfo);
                response = $"Set modifier(s) to: ";
                foreach (ModInfo mod in modInfo)
                {
                    response += $"{mod.Name} {(mod.MustPreload?"(Must be preloaded)":"")}, ";
                }
                ListPool<ModInfo>.Pool.Return(modInfo);
                return true;
                
            }
            else
            {
                response = "You do not have permission to use this set command.";
                return false;
            }
        }

        public string Command { get; } = "set";
        public string[] Aliases { get; } = new[] { "setmodifier" };
        public string Description { get; } = "Sets the round modifier.";
        public bool SanitizeResponse => true;
    }
}