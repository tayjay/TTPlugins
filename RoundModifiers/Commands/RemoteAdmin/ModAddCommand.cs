using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using TayTaySCPSL.Handlers;

namespace TayTaySCPSL.Commands
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

                if (Enum.TryParse(arguments.At(0), out RoundModifiers modifier))
                {
                    Plugin.Instance.RoundManager.AddRoundModifier(modifier);
                    response = $"Added modifier {modifier}.";
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
    }
}