
using System;
using CommandSystem;
using TayTaySCPSL.Handlers;

namespace TayTaySCPSL.Commands
{
    public class ModListCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 0)
            {
                response = "Usage: mod list";
                return false;
            }

            response = "All Modifiers: ";
            
            foreach (RoundModifiers modifier in Enum.GetValues(typeof(RoundModifiers)))
            {
                response += $"\n{modifier}, ";
            }

            response = response.Remove(response.Length - 2);
            return true;
        }

        public string Command { get; } = "list";
        public string[] Aliases { get; } = new[] { "listmodifiers", "all" };
        public string Description { get; } = "Lists all round modifiers.";
    }
}