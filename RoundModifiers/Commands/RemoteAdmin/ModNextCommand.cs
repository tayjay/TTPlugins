using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features.Pools;
using Exiled.Permissions.Extensions;
using RoundModifiers.API;
using RoundModifiers.Handlers;

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
                response = "Usage: mod next <modifier>/clear\n" +
                           "Next modifiers: \n";
                foreach (ModInfo mod in RoundModifiers.Instance.RoundManager.NextRoundModifiers)
                {
                    response += $"{mod.Name}, ";
                }
                return false;
            }
            if(arguments.At(0).ToLower() == "clear")
            {
                RoundModifiers.Instance.RoundManager.ClearNextRoundModifiers();
                response = "Cleared modifiers for next round.";
                return true;
            }

            if (arguments.At(0).ToLower() == "none")
            {
                List<ModInfo> noneInfo = ListPool<ModInfo>.Pool.Get();
                noneInfo.Add(RoundManager.NoneInfo);
                RoundModifiers.Instance.RoundManager.SetNextRoundModifiers(noneInfo);
                response = "Set next round to have No modifiers.";
                ListPool<ModInfo>.Pool.Return(noneInfo);
                return true;
            }

            List<ModInfo> modInfo = ListPool<ModInfo>.Pool.Get();
                
            foreach (string strMod in arguments)
            {
                if(RoundModifiers.Instance.TryGetModifier(strMod, out Modifier mod1))
                {
                    modInfo.Add(mod1.ModInfo);
                }
            }
            RoundModifiers.Instance.RoundManager.SetNextRoundModifiers(modInfo);
            response = $"Set next round modifier(s) to: ";
            ListPool<ModInfo>.Pool.Return(modInfo);
            foreach (ModInfo mod in RoundModifiers.Instance.RoundManager.NextRoundModifiers)
            {
                response += $"{mod.Name}, ";
            }
            return true;
        }

        public string Command { get; } = "next";
        public string[] Aliases { get; } = new[] { "setnext" };
        public string Description { get; } = "Sets the modifier for the next round.";
        public bool SanitizeResponse => true;
    }
}