using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using Exiled.API.Features;
using Exiled.Permissions.Extensions;
using InventorySystem.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using PlayerRoles;
using TTCore.Npcs.AI.Core.Management;
using TTCore.Utilities;

namespace TTCore.Commands.RemoteAdmin.Npc;

[CommandHandler(typeof(RemoteAdminCommandHandler))]
public class SpawnAICommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        if (!sender.CheckPermission("ai"))
        {
            if (sender is not ServerConsoleSender)
            {
                response = "You do not have the needed permissions to run this command! Needed perms : ai";
                return false;
            }
        }
        RoleTypeId role = RoleTypeId.ClassD;
        List<ItemType> items = [];
        Player player = Player.Get(sender);
        if (player == null)
        {
            response = "You must be  player to run this command.";
            return false;
        }
        AIPlayerProfile prof = NpcUtilities.CreateBasicAI(role, player.Position);

        response = "Created AI Player! Inventory: ";

        /*foreach (ItemType i in items)
        {
            ItemBase ite = prof.Player.AddItem(i);
            if (ite is Firearm f)
                f.Status = new FirearmStatus(f.AmmoManagerModule.MaxAmmo, f.Status.Flags, AttachmentsUtils.GetRandomAttachmentsCode(i));
        }

        response += string.Join(", ", items);
        */
        return true;
    }

    public string Command { get; } = "spawnai";
    public string[] Aliases { get; } = new[] {"spawnbot"};
    public string Description { get; } = "Spawns an AI npc.";
    public bool SanitizeResponse { get; } = true;
}