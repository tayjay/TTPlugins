using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.Attachments;
using RoundModifiers.API;
using TTCore.Utilities;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RoundModifiers.Modifiers;

public class MysteryBox : Modifier
{
    public List<ItemType> PossibleItems { get; private set;}
    public List<WeightedItem<ItemType>> WeightedItems { get; private set; }
    //OnFlipCoin
    public void OnFlipCoin(FlippingCoinEventArgs ev)
    {
        if(NearMysteryBox(ev.Player, out WorkstationController controller) && !ev.IsTails)
        {
            ev.IsAllowed = false;
            ev.Player.RemoveItem(ev.Item);
            ItemType random = new WeightedRandomSelector<ItemType>(WeightedItems).SelectItem();
            ev.Player.AddItem(random);
        }
    }
    
    protected bool NearMysteryBox(Player player, out WorkstationController workstation)
    {
        foreach (WorkstationController controller in WorkstationController.AllWorkstations)
        {
            if (controller.IsInRange(player.ReferenceHub))
            {
                workstation = controller;
                return true;
            }
        }
        workstation = null;
        return false;
    }
    
    
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.FlippingCoin += OnFlipCoin;
        PossibleItems = ListPool<ItemType>.Pool.Get();
        WeightedItems = ListPool<WeightedItem<ItemType>>.Pool.Get();
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.GunCOM15, 1));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.GunE11SR, 1));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.GunLogicer, 1));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.Adrenaline, 5));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.Medkit, 4));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.Flashlight, 3));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.Painkillers, 4));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.Radio, 2));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.KeycardO5, 1));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.SCP018, 1));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.SCP207, 1));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.SCP268, 1));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.SCP500, 1));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.SCP1576, 1));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.SCP2176, 1));
        WeightedItems.Add(new WeightedItem<ItemType>(ItemType.Jailbird, 1));
        
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.FlippingCoin -= OnFlipCoin;
        
        ListPool<ItemType>.Pool.Return(PossibleItems);
        ListPool<WeightedItem<ItemType>>.Pool.Return(WeightedItems);
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "Mystery Box",
        FormattedName = "Mystery Box",
        Aliases = new[] { "box" },
        Description = "A mystery box that gives you a random item.",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false,
        Balance = 2
    };
}