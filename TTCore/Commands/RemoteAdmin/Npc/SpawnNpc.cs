using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using PlayerRoles;
using TTCore.Extensions;
using UnityEngine;

namespace TTCore.Commands.RemoteAdmin.Npc
{
    public class SpawnNpc : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            string name = "D-Boi";
            string roleString = "ClassD";
            Player player = null;
            if(Player.Get(sender)!=null)
                player = Player.Get(sender);

            switch (arguments.Count)
            {
                case 1:
                    name = arguments.At(0);
                    break;
                case 2:
                    name = arguments.At(0);
                    roleString = arguments.At(1);
                    break;
                case 3:
                    name = arguments.At(0);
                    roleString = arguments.At(1);
                    player = Player.Get(arguments.At(2));
                    break;
            }
            
            if (!Enum.TryParse(roleString, out RoleTypeId role))
            {
                response = $"Invalid role: {roleString}";
                return false;
            }

            Vector3 position;
            if (player == null)
            {
                if (sender is not ServerConsoleSender)
                {
                    response = $"Invalid player with the specified ID or Nickname: {arguments.At(2)}";
                    return false;
                }
                position = Vector3.zero;
            } else
            {
                position = player.Position;
            }
            if (!sender.CheckPermission("devdummies"))
            {
                if (sender is not ServerConsoleSender)
                {
                    response = "You do not have the needed permissions to run this command! Needed perms : devdummies";
                    return false;
                }
            }
            if(role.IsAlive())
                TTCore.Instance.NpcManager.SpawnNpc(name, role, position,out Exiled.API.Features.Npc npc);
            else
                TTCore.Instance.NpcManager.SpawnNpc(name, role, Vector3.zero, out Exiled.API.Features.Npc npc);
            response = $"Spawned NPC with name '{name}', role '{role}', for player '{sender.LogName}'";
            return true;
        }

        public string Command { get; } = "spawn";
        public string[] Aliases { get; } = new []{ "create"};
        public string Description { get; } = "Spawns an NPC";
        public bool SanitizeResponse => true;
    }
}