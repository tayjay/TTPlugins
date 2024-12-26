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
            
            if (TTAddons.Instance.RoleSelectorHandler.HasRequestedHuman(player))
            {
                TTAddons.Instance.RoleSelectorHandler.Reset(player,out int newTickets1);
                response = "You have reset your Human request.\nYou have "+newTickets1+" tickets.";
                return true;
            }

            TTAddons.Instance.RoleSelectorHandler.SetHuman(player, out int oldTickets, out int newTickets);
            response = "You have requested to be a Human next round.\nYou have "+newTickets+" tickets.\nYou had "+oldTickets+" tickets.";
            return true;
        }

        public string Command { get; } = "human";
        public string[] Aliases { get; }
        public string Description { get; } = "Prefer to be a human this round.";
    }
}