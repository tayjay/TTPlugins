using System;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using Exiled.API.Features;
using RoundModifiers.Modifiers.LevelUp;
using RoundModifiers.Modifiers.LevelUp.Boosts;
using RoundModifiers.Modifiers.LevelUp.XPs;

namespace RoundModifiers.Commands.Client;

[CommandHandler(typeof(ClientCommandHandler))]
public class LevelUpCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        if (!RoundModifiers.Instance.IsModifierActive("LevelUp") && Round.InProgress)
        {
            response = "LevelUp is not active this round.";
            return false;
        }

        if (!RoundModifiers.Instance.GetModifier<LevelUp>().PlayerLevel.ContainsKey(Player.Get(sender).NetId))
        {
            response = "You are not in the LevelUp system.";
            return true;
        }
        int level = RoundModifiers.Instance.GetModifier<LevelUp>().PlayerLevel[Player.Get(sender).NetId];
        float xp = RoundModifiers.Instance.GetModifier<LevelUp>().PlayerXP[Player.Get(sender).NetId];
        float xpNeeded = XP.GetXPNeeded(level);

        response = $"Your current Level is {level} and your current XP is {xp}/{xpNeeded}";
        response += "\n Your current boosts are: ";
        foreach (Boost boost in RoundModifiers.Instance.GetModifier<LevelUp>()._boosts)
        {
            if (boost.HasBoost.ContainsKey(Player.Get(sender).NetId))
                response += "\n"+boost.GetName();
        }
        return true;
    }

    public string Command { get; } = "levelup";
    public string[] Aliases { get; } = { "level", "lvl", "xp" };
    public string Description { get; } = "Client command for the LevelUp modifier.";
} 