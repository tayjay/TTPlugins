using System;
using CommandSystem;
using RoundModifiers.API;

namespace RoundModifiers.Commands.RemoteAdmin
{
    public class ModActiveCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (arguments.Count != 0)
            {
                response = "Usage: mod active";
                return false;
            }

            response = "Modifiers: ";
            foreach (ModInfo modifier in RoundModifiers.Instance.RoundManager.ActiveModifiers)
            {
                response += $"{modifier.Name}, ";
            }

            response = response.Remove(response.Length - 2);
            return true;
        }

        public string Command { get; } = "active";
        public string[] Aliases { get; } = new[] { "current", "cur" };
        public string Description { get; } = "Lists all active round modifiers.";
    }
}