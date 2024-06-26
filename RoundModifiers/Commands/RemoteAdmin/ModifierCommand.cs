﻿using System;
using CommandSystem;

namespace RoundModifiers.Commands.RemoteAdmin
{
    [CommandHandler(typeof(RemoteAdminCommandHandler))]
    public class ModifierCommand : ParentCommand
    {
        
        public ModifierCommand()
        {
            LoadGeneratedCommands();
        }
        
        public override void LoadGeneratedCommands()
        {
            RegisterCommand(new ModAddCommand());
            RegisterCommand(new ModClearCommand());
            RegisterCommand(new ModListCommand());
            RegisterCommand(new ModSetCommand());
            RegisterCommand(new ModActiveCommand());
            RegisterCommand(new ModNextCommand());
            RegisterCommand(new ModRandomCommand());
            RegisterCommand(new ModHideCommand());
        }

        protected override bool ExecuteParent(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            response = "Invalid subcommand! Valid subcommands : set, add, clear, all, active, next, random, hide";
            return false;
        }

        public override string Command { get; } = "modifier";
        public override string[] Aliases { get; } = new[] {"mod"};
        public override string Description { get; } = "Control the round modifiers.";
    }
}