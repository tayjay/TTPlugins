using System;
using System.IO;
using CommandSystem;
using Exiled.Permissions.Extensions;
using MoonSharp.Interpreter;
using SCriPt.API.Lua;

namespace SCriPt.Commands.RemoteAdmin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ScriptFCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!sender.CheckPermission("scriptf"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "Usage: scriptf <file_name.lua>";
                return false;
            }
            Script script = new Script();
            ScriptLoader.RegisterAPI(script);
            
            foreach(string file in Directory.GetFiles("Scripts"))
            {
                if(file.ToLower().EndsWith(".lua"))
                {
                    if(file.ToLower().Equals(arguments.At(0)))
                    {
                        //Script script = new Script();
                        script.DoFile(file);
                        //SCriPt.Instance.LoadedScripts.Add(script);
                        response = "Loaded script: "+file;
                        return true;
                    }
                }
            }
            
            response = "File not found.";
            return false;
        }

        public string Command { get; } = "scriptf";
        public string[] Aliases { get; } = {"luaf"};
        public string Description { get; } = "Runs a lua file.";
    }
}