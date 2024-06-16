using System;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using Exiled.API.Features;
using PlayerRoles;
using RoundModifiers.Modifiers.Scp507;

namespace RoundModifiers.Commands.Client;

[CommandHandler(typeof(ClientCommandHandler))]
public class RoleResetCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        Log.Debug("Running Command507...");
        Player player = Player.Get(sender);
        if(player == null)
        {
            response = "You must be a player to use this command.";
            return false;
        }

        if (!Scp507.Scp507Role.Check(player))
        {
            response = "You must be SCP-507 to use this command.";
            return false;
        }
        
        player.RoleManager.ServerSetRole(RoleTypeId.ClassD, RoleChangeReason.RemoteAdmin, RoleSpawnFlags.None);
        response = "Resetting role...";
        return true;
    }

    public string Command { get; } = "rolereset";
    public string[] Aliases { get; } = { "rreset", "507" };
    public string Description { get; } = "Helps fix issues with SCP-507.";
    public bool SanitizeResponse { get; } = true;
}