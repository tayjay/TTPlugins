using System;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace RoundModifiers.Commands.RemoteAdmin
{
    public class ModClearCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(sender.CheckPermission("RoundEvents") || sender.CheckPermission("modifier") || sender.CheckPermission("RoundEvents*"))
            {
                if (arguments.Count != 0)
                {
                    response = "Usage: mod clear";
                    return false;
                }

                RoundModifiers.Instance.RoundManager.ClearRoundModifiers();
                response = $"Removed all round modifiers.";
                return true;
            }
            else
            {
                response = "You do not have permission to use this command.";
                return false;
            }
        }

        public string Command { get; } = "clear";
        public string[] Aliases { get; } = new[] { "clearmodifiers" };
        public string Description { get; } = "Clears all round modifiers.";
    }
}