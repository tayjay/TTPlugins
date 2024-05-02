using System;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace TTCore.Commands.RemoteAdmin;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class VoiceModCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        if (!sender.CheckPermission("scale"))
        {
            response = "You do not have permission to use this command.";
            return false;
        }
        
        TTCore.Instance.VoiceHandler.ShouldModifyVoice = !TTCore.Instance.VoiceHandler.ShouldModifyVoice;
        response = $"Voice Mod is now {(TTCore.Instance.VoiceHandler.ShouldModifyVoice ? "enabled" : "disabled")}";
        return true;
    }

    public string Command { get; } = "voicemod";
    public string[] Aliases { get; } = { "vm" };
    public string Description { get; } = "Toggles voice mod for players.";
}