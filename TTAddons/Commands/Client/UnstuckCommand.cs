using System;
using System.Collections.Generic;
using CommandSystem;
using Exiled.API.Enums;
using Exiled.API.Features;
using PlayerRoles;
using UnityEngine;

namespace TTAddons.Commands.Client
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class UnstuckCommand : ICommand
    {
        
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            Log.Info("Attempting to Unstuck.");
            Player player  = Player.Get(sender);
            if (player == null)
            {
                response = "Player not found.";
                return false;
            }

            return TTAddons.Instance.Unstuck.DoUnstuck(player, out response);
        }

        public string Command { get; } = "unstuck";
        public string[] Aliases { get; } = new[] {"stuck"};
        public string Description { get; } = "Unstucks the player. (Only works for SCPs at the start of a round.)";
        public bool SanitizeResponse => true;
    }
}