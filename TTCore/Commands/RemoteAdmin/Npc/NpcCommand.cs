using System;
using CommandSystem;

namespace TTCore.Commands.RemoteAdmin.Npc
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class NpcCommand : ParentCommand
    {
        
        public NpcCommand()
        {
            LoadGeneratedCommands();
        }
        
        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new SpawnNpc());
            RegisterCommand(new RemoveNpc());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Invalid subcommand! Valid subcommands : LookAt, Stats, Remove, Spawn";
            return false;
        }

        public override string Command { get; } = "npc";
        public override string[] Aliases { get; } = new[] {"dummy", "bot"};
        public override string Description { get; } = "Control the npcs.";
    }
}