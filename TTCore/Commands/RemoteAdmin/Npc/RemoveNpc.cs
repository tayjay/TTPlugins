using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using TTCore.Extensions;

namespace TTCore.Commands.RemoteAdmin.Npc
{
    public class RemoveNpc : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if (!Player.Get(sender).HasNpcPermissions())
            {
                response = "You do not have the needed permissions to run this command! Needed perms : npc";
                return false;
            }
            if (arguments.Count != 1)
            {
                if (Player.Get(sender).TryGetNpcOnSight(20,out Exiled.API.Features.Npc npc))
                {
                    if(npc.IsNPC)           
                    {
                        npc.ReferenceHub.OnDestroy();

                        LeftEventArgs newLeft = new(npc);
                        Exiled.Events.Handlers.Player.OnLeft(newLeft);

                        response = $"Removed {npc.Nickname} on sight!";
                        return true;
                    }
                }
                else
                {
                    response = "Insufficient arguments. Usage: npc remove [dummyID]";
                    return false;
                }
            }
            Player Npc = Player.Get(arguments.At(0));
            if(Npc == null)
            {
                response = $"The player with the specified ID, '{arguments.At(0)}', doesn't exist!";
                return false;
            }
            if(Npc.IsNPC)           
            {
                Npc.ReferenceHub.OnDestroy();

                LeftEventArgs newLeft = new(Npc);
                Exiled.Events.Handlers.Player.OnLeft(newLeft);

                response = $"Removed {Npc.Nickname}!";
                return true;
            }
            else
            {
                response = $"ID : '{Npc.Id}', Nickname : '{Npc.Nickname}' is not a dummy or you entered a incorrect ID!";
                return false;
            }
        }

        public string Command { get; } = "remove";
        public string[] Aliases { get; } = new []{"kill", "delete", "del"};
        public string Description { get; } = "Removes an NPC";
        public bool SanitizeResponse => true;
    }
}