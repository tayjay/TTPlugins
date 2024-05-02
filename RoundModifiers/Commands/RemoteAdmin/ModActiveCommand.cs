using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Features.Pools;
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
            List<ModInfo> activeModifiers =
                ListPool<ModInfo>.Pool.Get(RoundModifiers.Instance.RoundManager.ActiveModifiers);
            foreach (ModInfo modifier in activeModifiers)
            {
                response += $"{modifier.Name}, ";
            }
            ListPool<ModInfo>.Pool.Return(activeModifiers);

            response = response.Remove(response.Length - 2);
            return true;
        }

        public string Command { get; } = "active";
        public string[] Aliases { get; } = new[] { "current", "cur" };
        public string Description { get; } = "Lists all active round modifiers.";
    }
}