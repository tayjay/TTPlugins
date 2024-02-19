using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.Permissions.Extensions;
using TayTaySCPSL.Handlers;

namespace TayTaySCPSL.Commands
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
                response = "Usage: mod next <modifier>";
                return false;
            }
            if(arguments.At(0).ToLower() == "clear")
            {
                Plugin.Instance.RoundManager.ClearNextRoundModifiers();
                response = "Cleared modifiers for next round.";
                return true;
            }
            response = "Set modifiers for next round: \n";
            List<RoundModifiers> modifiers = new List<RoundModifiers>();
            foreach (string strMod in arguments)
            {
                if(Enum.TryParse(strMod, out RoundModifiers modifier))
                {
                    response+= $"{modifier}, ";
                    modifiers.Add(modifier);
                }
            }
            response = response.Remove(response.Length - 2);
            Plugin.Instance.RoundManager.SetNextRoundModifiers(modifiers);
            return true;
        }

        public string Command { get; } = "next";
        public string[] Aliases { get; } = new[] { "setnext" };
        public string Description { get; } = "Sets the modifier for the next round.";
    }
}