using PluginAPI.Events;

namespace TTAdmin.Data.Events;

public class ConsoleEventExecutedData : EventData
{
    public override string EventName => "console_command_executed";
    
    public CommandData Command { get; set; }

    public ConsoleEventExecutedData(ConsoleCommandExecutedEvent ev)
    {
        Command = new CommandData(ev.Sender.LogName, ev.Command, ev.Arguments, ev.Result, ev.Response);
    }
}