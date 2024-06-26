﻿using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RoundModifiers.API;

namespace RoundModifiers.Commands.RemoteAdmin
{
    public class ModAddCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(sender.CheckPermission("RoundEvents") || sender.CheckPermission("modifier") || sender.CheckPermission("RoundEvents*"))
            {
                if (arguments.Count != 1)
                {
                    response = "Usage: mod add <modifier>";
                    return false;
                }
                
                if(Round.InProgress)
                {
                    response = "You cannot add modifiers while a round is in progress. Please use 'mod next' to set modifiers for the next round.";
                    return false;
                }

                if (RoundModifiers.Instance.TryGetModifier(arguments.At(0), out Modifier modifier))
                {
                    RoundModifiers.Instance.RoundManager.AddRoundModifier(modifier.ModInfo);
                    if (modifier.ModInfo.MustPreload)
                    {
                        response = $"Added modifier {modifier.ModInfo.Name}. This modifier must be preloaded to work.";
                        return false;
                    }
                    response = $"Added modifier {modifier.ModInfo.Name}.";
                    return true;
                }
                else
                {
                    response = "Invalid modifier.";
                    return false;
                }
            }
            else
            {
                response = "You do not have permission to use this command.";
                return false;
            }
        }

        public string Command { get; } = "add";
        public string[] Aliases { get; } = new[] { "addmodifier" };
        public string Description { get; } = "Add a round modifier to the list.";
        public bool SanitizeResponse => true;
    }
}