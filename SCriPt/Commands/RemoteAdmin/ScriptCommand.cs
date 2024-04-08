using System;
using CommandSystem;
using Exiled.Permissions.Extensions;
using MoonSharp.Interpreter;
using SCriPt.API.Lua;

namespace SCriPt.Commands.RemoteAdmin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ScriptCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(!sender.CheckPermission("script"))
            {
                response = "You do not have permission to use this command.";
                return false;
            }

            if (arguments.Count == 0)
            {
                response = "Usage: script <lua>";
                return false;
            }
            
            string lua = string.Join(" ", arguments);
            
            try
            {
                Script script = new Script();
                ScriptLoader.RegisterAPI(script);
                DynValue res = script.DoString(lua);
                response = "Result: " + res.String;
                return true;
            }
            catch (Exception e)
            {
                response = e.Message;
                return false;
            }
        }

        public string Command { get; } = "script";
        public string[] Aliases { get; } = {"lua"};
        public string Description { get; } = "Runs a lua string.";
    }
}