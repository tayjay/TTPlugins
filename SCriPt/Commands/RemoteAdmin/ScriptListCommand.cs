using System;
using CommandSystem;
using Exiled.Permissions.Extensions;

namespace SCriPt.Commands.RemoteAdmin
{
    public class ScriptListCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(sender.CheckPermission("SCriPt.list"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }
            
            response = "Loaded scripts:\n";
            foreach (string script in SCriPt.Instance.LoadedScripts.Keys)
            {
                response += script + "\n";
            }
            return true;
        }

        public string Command { get; } = "list";
        public string[] Aliases { get; } = {"active, loaded", "running"};
        public string Description { get; } = "Lists all loaded scripts.";
    }
}