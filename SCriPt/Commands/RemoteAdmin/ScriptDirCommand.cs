using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using SCriPt.API.Lua;

namespace SCriPt.Commands.RemoteAdmin
{
    public class ScriptDirCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("SCriPt.dir"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }

            string[] files = ScriptLoader.GetScriptsDir();
            response = $"Files in scripts directory:\n {string.Join("\n", files)}";
            return true;
        }

        public string Command { get; } = "dir";
        public string[] Aliases { get; } = new[] { "files" };
        public string Description { get; } = "Lists all files in the scripts directory.";
    }
}