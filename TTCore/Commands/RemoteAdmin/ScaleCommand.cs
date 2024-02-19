using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using TTCore.Extensions;

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
            
        if (arguments.Count != 2)
        {
            response = "Usage: scale <player> <size>";
            return false;
        }
            
        Player player = Player.Get(arguments.At(0));
        if (player == null)
        {
            response = "Player not found.";
            return false;
        }
            
        if (!float.TryParse(arguments.At(1), out float size))
        {
            response = "Invalid size.";
            return false;
        }
            
        player.ChangeSize(size);
        response = $"Set {player.Nickname}'s scale to {size}.";
        return true;
            
    }

    public string Command { get; } = "scale";
    public string[] Aliases { get; } = {"size"};
    public string Description { get; } = "Scales the player to a designated size.";
}