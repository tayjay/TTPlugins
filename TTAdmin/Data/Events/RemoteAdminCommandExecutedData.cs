using System.Collections.Generic;
using PluginAPI.Events;

namespace TTAdmin.Data.Events;

public class RemoteAdminCommandExecutedData : EventData
{
    public override string EventName => "remote_admin_command_executed";
    public CommandData Command { get; set; }

    public RemoteAdminCommandExecutedData(RemoteAdminCommandExecutedEvent ev)
    {
        Command = new CommandData(ev.Sender.LogName, ev.Command, ev.Arguments, ev.Result, ev.Response);
    }
}