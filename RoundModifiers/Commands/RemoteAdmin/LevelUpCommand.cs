using System;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using RoundModifiers.Modifiers.LevelUp;

namespace RoundModifiers.Commands.RemoteAdmin;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class LevelUpCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        if (!sender.CheckPermission("modifiers"))
        {
            response = "You do not have permission to use this command.";
            return false;
        }
        
        //Usage: levelup <xp/level> <player> <amount>
        if (arguments.Count < 3)
        {
            response = "Usage: levelup xp <player> <amount>";
            return false;
        }

        LevelUp levelUp = RoundModifiers.Instance.GetModifier<LevelUp>();
        
        string type = arguments.At(0).ToLower();
        Player player = Player.Get(arguments.At(1));
        if (player == null)
        {
            response = "Player not found.";
            return false;
        }
        float amount = Convert.ToSingle(arguments.At(2));
        
        switch (type)
        {
            case "xp":
                levelUp.PlayerXP[player.NetId] += amount;
                response = $"Added {amount} XP to {player.Nickname}.";
                return true;
            /*case "level":
                levelUp.PlayerLevel[player.NetId] += (int)amount;
                response = $"Added {amount} levels to {player.Nickname}.";
                return true;*/
            default:
                response = "Invalid type. Use xp or level.";
                return false;
        }
    }

    public string Command { get; } = "levelup";
    public string[] Aliases { get; } = { "lvl" };
    public string Description { get; } = "Manage the LevelUp mod.";
    public bool SanitizeResponse => true;
}