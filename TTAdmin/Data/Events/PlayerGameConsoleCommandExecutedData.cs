using PluginAPI.Events;

namespace TTAdmin.Data.Events;

public class PlayerGameConsoleCommandExecutedData : EventData
{
    public override string EventName => "player_game_console_command_executed";
    public CommandData Command { get; set; }

    public PlayerGameConsoleCommandExecutedData(PlayerGameConsoleCommandExecutedEvent ev)
    {
        Command = new CommandData(ev.Player.LogName, ev.Command, ev.Arguments, ev.Result, ev.Response);
    }
}