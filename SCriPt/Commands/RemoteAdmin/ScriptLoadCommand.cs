using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using SCriPt.API.Lua;

namespace SCriPt.Commands.RemoteAdmin
{
    public class ScriptLoadCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!sender.CheckPermission("SCriPt.load"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }
            
            if (arguments.Count == 0)
            {
                response = "Usage: script load <script_name>";
                return false;
            }
            
            string scriptName = arguments.At(0);
            if (SCriPt.Instance.LoadedScripts.ContainsKey(scriptName))
            {
                response = $"Script {scriptName} is already loaded.";
                return false;
            }
            
            if (!ScriptLoader.LoadFile(scriptName))
            {
                response = $"Failed to load script {scriptName}.";
                return false;
            }
            
            response = $"Loaded script {scriptName}.";
            return true;
        }

        public string Command { get; } = "load";
        public string[] Aliases { get; } = {"start"};
        public string Description { get; } = "Loads a script from a file.";
    }
}