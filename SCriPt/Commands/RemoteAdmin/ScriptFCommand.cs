using System;
using System.IO;
using CommandSystem;
using Exiled.API.Features;
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
            
            
            foreach(string file in Directory.GetFiles("Scripts"))
            {
                Log.Debug("Checking file: "+file);
                if(file.EndsWith(".lua"))
                {
                    if(file.ToLower().Contains(arguments.At(0).ToLower()))
                    {
                        try
                        {
                            Script script = new Script();
                            ScriptLoader.RegisterAPI(script);
                            //Script script = new Script();
                            script.DoFile(file);
                            //SCriPt.Instance.LoadedScripts.Add(script);
                            response = "Loaded script: " + file;
                            return true;
                        }
                        catch (ScriptRuntimeException e)
                        {
                            Log.Error(e.DecoratedMessage);
                        }
                    }
                }
            }
            
            response = $"File {arguments.At(0)} not found.";
            return false;
        }

        public string Command { get; } = "scriptf";
        public string[] Aliases { get; } = {"luaf"};
        public string Description { get; } = "Runs a lua file.";
    }
}