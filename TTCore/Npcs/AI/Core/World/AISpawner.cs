using System;
using System.Collections.Generic;
using Exiled.API.Features.Items;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Firearms.Attachments;
using PlayerRoles;
using TTCore.API;
using TTCore.Npcs.AI.Core.Management;
using TTCore.Utilities;
using Firearm = Exiled.API.Features.Items.Firearm;

namespace TTCore.Npcs.AI.Core.World;

public class AISpawner : SpawnerBase
{
    public int Limit;
    public RoleTypeId Role;
    public readonly List<ItemType> Items = [];

    protected readonly List<AIPlayer> NPCs = [];

    public override bool SetSpawnee(string[] value, out string feedback) // [limit] [role] [items...]
    {
        if (!value.TryGet(0, out string arg1) || !int.TryParse(arg1, out int i) || i <= 0)
        {
            feedback = "Please set a positive integer limit of NPCs! ";

            return false;
        }

        Limit = i;

        if (value.TryGet(1, out string arg2) && Enum.TryParse(arg2, out RoleTypeId r))
            Role = r;

        if (value.Length > 2)
        {
            Items.Clear();
            List<string> item = [.. value];
            item.RemoveRange(0, 2);
            foreach (string s in item)
            {
                if (Enum.TryParse(s, out ItemType ite))
                    Items.Add(ite);
            }
        }

        feedback = "NPC Spawner: " + ToString();

        return true;
    }

    public override void Spawn()
    {
        NPCs.RemoveAll((n) => n == null);

        if (NPCs.Count >= Limit)
            return;

        AIPlayerProfile prof = NpcUtilities.CreateBasicAI(Role, Position);

        foreach (ItemType i in Items)
        {
            Item ite = prof.Player.AddItem(i);
            if (ite is Firearm f)
                f.Base.Status = new FirearmStatus(f.Base.AmmoManagerModule.MaxAmmo, f.Base.Status.Flags, AttachmentsUtils.GetRandomAttachmentsCode(i));
        }

        NPCs.Add(prof.WorldPlayer);
    }

    public override string ToString() => base.ToString() + "\nSpawner Role: " + RoleTranslations.GetRoleName(Role) + "\nLimit: " + Limit + "\nLoadout: " + string.Join(", ", Items);
}