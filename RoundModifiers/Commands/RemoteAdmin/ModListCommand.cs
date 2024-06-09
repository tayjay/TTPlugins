
using System;
using System.Linq;
using CommandSystem;
using RoundModifiers.API;

namespace RoundModifiers.Commands.RemoteAdmin
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
            
            foreach (ModInfo modifier in RoundModifiers.Instance.Modifiers.Keys.OrderBy(info=>info.Name))
            {
                response += $"\n{modifier.Name}, ";
            }

            response = response.Remove(response.Length - 2);
            return true;
        }

        public string Command { get; } = "list";
        public string[] Aliases { get; } = new[] { "listmodifiers", "all" };
        public string Description { get; } = "Lists all round modifiers.";
        public bool SanitizeResponse => true;
    }
}