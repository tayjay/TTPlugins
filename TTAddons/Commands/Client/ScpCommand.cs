﻿using System;
using CommandSystem;
using Exiled.API.Features;
using Round = PluginAPI.Core.Round;

namespace TTAddons.Commands.Client
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class ScpCommand : ICommand
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

            if (Round.IsRoundStarted)
            {
                response = "You can't request to be an SCP after the round has started.";
                return false;
            }

            if (TTAddons.Instance.RoleSelectorHandler.HasRequestedSCP(player))
            {
                TTAddons.Instance.RoleSelectorHandler.Reset(player,out int newTickets1);
                response = "You have reset your SCP request.\nYou have "+newTickets1+" tickets.";
                return true;
            }

            if (TTAddons.Instance.RoleSelectorHandler.SetSCP(player, out int oldTickets, out int newTickets))
            {
                response = "You have requested to be an SCP next round.\nYou have "+newTickets+" tickets.\nYou had "+oldTickets+" tickets.";
                return true;
            }
            else
            {
                response = "You were an SCP too recently to request to be one again.\nYou have "+newTickets+" tickets.";
                return false;
            }
            
        }

        public string Command { get; } = "scp";
        public string[] Aliases { get; }
        public string Description { get; } = "Prefer to be an SCP this round.";
    }
}