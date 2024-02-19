using System;
using CommandSystem;
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

                if (RoundModifiers.Instance.TryGetModifier(arguments.At(0), out Modifier modifier))
                {
                    RoundModifiers.Instance.RoundManager.AddRoundModifier(modifier.ModInfo);
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