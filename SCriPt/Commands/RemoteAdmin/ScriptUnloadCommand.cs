using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using MoonSharp.Interpreter;
using SCriPt.API.Lua;

namespace SCriPt.Commands.RemoteAdmin
{
    public class ScriptUnloadCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(sender.CheckPermission("SCriPt.unload"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }
            
            if (arguments.Count == 0)
            {
                response = "Usage: script unload <script_name>";
                return false;
            }
            
            string scriptName = arguments.At(0);
            if (!SCriPt.Instance.LoadedScripts.ContainsKey(scriptName))
            {
                response = $"Script {scriptName} is not loaded.";
                return false;
            }
            Script scriptToUnload = SCriPt.Instance.LoadedScripts[scriptName];
            ScriptLoader.ExecuteUnload(scriptToUnload);
            SCriPt.Instance.LoadedScripts.Remove(scriptName);
            response = $"Unloaded script {scriptName}.";
            return true;
        }

        public string Command { get; } = "unload";
        public string[] Aliases { get; } = {"stop"};
        public string Description { get; } = "Unloads a loaded script.";
    }
}