using System;
using CommandSystem;

namespace TTCore.Commands.RemoteAdmin
{
    public class NpcCommand : ParentCommand
    {
        
        public NpcCommand()
        {
            LoadGeneratedCommands();
        }
        
        public override void LoadGeneratedCommands()
        {
            throw new NotImplementedException();
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Invalid subcommand! Valid subcommands : LookAt, Stats, Remove, Spawn";
            return false;
        }

        public override string Command { get; }
        public override string[] Aliases { get; }
        public override string Description { get; }
    }
}