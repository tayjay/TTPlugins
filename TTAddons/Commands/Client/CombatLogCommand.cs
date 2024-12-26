using System;
using CommandSystem;
using PluginAPI.Core;

namespace TTAddons.Commands.Client
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class CombatLogCommand  : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(Player.Get(sender)==null) {
                response = "You must be a player to execute this command.";
                return false;
            }
            
            if (arguments.Count != 0)
            {
                response = "Usage: combatlog";
                return false;
            }
            
            Player player = Player.Get(sender);
            if(TTAddons.Instance.CombatLogHandler.CombatLogs.TryGetValue(player, out var logs))
            {
                //Sort logs by reverse time
                logs.Sort((x, y) => y.Time.CompareTo(x.Time));
                response = "Combat log:\n";
                foreach (var log in logs)
                {
                    if (log.Target == player.Nickname)
                    {
                        response+= "<color=red>";
                    } else if (log.Attacker == player.Nickname)
                    {
                        response+= "<color=green>";
                    } else
                    {
                        response+= "<color=white>";
                    }
                    response += $"| Time: {log.Time} | Attacker: {log.Attacker} ({log.AttackerHealth}) | Target: {log.Target} ({log.TargetHealth}) | Damage: {log.Damage} ({log.DamageType}) |</color>\n";
                }
            }
            else
            {
                response = "No combat log found.";
                return false;
            }
            response = response.Trim();
            return true;
        }

        public string Command { get; } = "combatlog";
        public string[] Aliases { get; } = { "cl" };
        public string Description { get; } = "View combat log.";
    }
}