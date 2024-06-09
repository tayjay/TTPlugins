using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using TTCore.Extensions;
using UnityEngine;

namespace TTCore.Commands.RemoteAdmin;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class ScaleCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
    {
        if (!sender.CheckPermission("scale") && !sender.CheckPermission("RoundEvents") && !sender.CheckPermission("RoundEvents*"))
        {
            response = "You do not have permission to use this command.";
            return false;
        }
            
        if (arguments.Count != 2 && arguments.Count != 4)
        {
            response = "Usage: Usage: scale <player> <Size or [X] [Y] [X]>";
            return false;
        }
            
        Player player = Player.Get(arguments.At(0));
        if (player == null)
        {
            response = "Player not found.";
            return false;
        }

        if (arguments.Count == 2)
        {
            if (!float.TryParse(arguments.At(1), out float size))
            {
                response = "Invalid size.";
                return false;
            }
            
            player.ChangeSize(size);
            response = $"Set {player.Nickname}'s scale to {size}.";
            return true;
        } else if (arguments.Count == 4)
        {
            if(!float.TryParse(arguments.At(1), out float x) || !float.TryParse(arguments.At(2), out float y) || !float.TryParse(arguments.At(3), out float z))
            {
                response = "Invalid size.";
                return false;
            }
            Vector3 size = new Vector3(x,y,z);
            player.ChangeSize(size);
            response = $"Set {player.Nickname}'s scale to {size}.";
            return true;
        }
        else
        {
            response = "Invalid Arguments!\nUsage: scale <player> <Size or [X] [Y] [X]>";
            return false;
        }
        
        response = "Usage: scale <player> <Size or [X] [Y] [X]>";
        return false;
            
    }

    public string Command { get; } = "scale";
    public string[] Aliases { get; } = {"size"};
    public string Description { get; } = "Scales the player to a designated size.";
    public bool SanitizeResponse => true;
}