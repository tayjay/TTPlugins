using System;
using CommandSystem;
using Exiled.API.Features;

namespace TTAddons.Commands.Client
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class HumanCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!TTAddons.Instance.Config.AllowRoleCommands)
            {
                response = "Role commands are disabled.";
                return false;
            }
            Player player = Player.Get(sender);
            if (player == null)
            {
                response = "Player not found.";
                return false;
            }
            
            if (Round.IsStarted)
            {
                response = "You can't request to be a human after the round has started.";
                return false;
            }

            TTAddons.Instance.RoleSelectorHandler.SetHuman(player);
            response = "You have requested to be a Human next round.";
            return true;
        }

        public string Command { get; } = "human";
        public string[] Aliases { get; }
        public string Description { get; } = "Prefer to be a human this round.";
    }
}