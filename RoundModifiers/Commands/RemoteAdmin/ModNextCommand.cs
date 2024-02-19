using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.Permissions.Extensions;
using RoundModifiers.API;

namespace RoundModifiers.Commands.RemoteAdmin
{
    public class ModNextCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!sender.CheckPermission("RoundEvents") && !sender.CheckPermission("modifier") && !sender.CheckPermission("RoundEvents*"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }
            if (arguments.Count < 1)
            {
                response = "Usage: mod next <modifier>/clear";
                return false;
            }
            if(arguments.At(0).ToLower() == "clear")
            {
                RoundModifiers.Instance.RoundManager.ClearNextRoundModifiers();
                response = "Cleared modifiers for next round.";
                return true;
            }
            List<ModInfo> modInfo = new List<ModInfo>();
                
            foreach (string strMod in arguments)
            {
                if(RoundModifiers.Instance.TryGetModifier(strMod, out Modifier mod1))
                {
                    modInfo.Add(mod1.ModInfo);
                }
            }
            RoundModifiers.Instance.RoundManager.SetNextRoundModifiers(modInfo);
            response = $"Set next round modifier(s) to: ";
            foreach (ModInfo mod in modInfo)
            {
                response += $"{mod.Name}, ";
            }
            return true;
        }

        public string Command { get; } = "next";
        public string[] Aliases { get; } = new[] { "setnext" };
        public string Description { get; } = "Sets the modifier for the next round.";
    }
}