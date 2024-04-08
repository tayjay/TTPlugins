using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.Attachments;
using RoundModifiers.API;
using UnityEngine;
using Random = UnityEngine.Random;

namespace RoundModifiers.Modifiers;

public class MysteryBox : Modifier
{
    public List<ItemType> PossibleItems { get; } = new List<ItemType>();
    //OnFlipCoin
    public void OnFlipCoin(FlippingCoinEventArgs ev)
    {
        if(NearMysteryBox(ev.Player, out WorkstationController controller) && !ev.IsTails)
        {
            ev.IsAllowed = false;
            ev.Player.RemoveItem(ev.Item);
            //Pickup.CreateAndSpawn(ItemType.Coin, controller.transform.position+Vector3.up, controller.transform.rotation);
            int randomNum = Random.Range(1, Random.Range(2, PossibleItems.Count));
            ItemType random = PossibleItems.ElementAt(randomNum);
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
        
        PossibleItems.Add(ItemType.Adrenaline);
        PossibleItems.Add(ItemType.Adrenaline);
        PossibleItems.Add(ItemType.Adrenaline);
        PossibleItems.Add(ItemType.Adrenaline);
        PossibleItems.Add(ItemType.Medkit);
        PossibleItems.Add(ItemType.Medkit);
        PossibleItems.Add(ItemType.Medkit);
        PossibleItems.Add(ItemType.Medkit);
        PossibleItems.Add(ItemType.Flashlight);
        PossibleItems.Add(ItemType.Painkillers);
        PossibleItems.Add(ItemType.Painkillers);
        PossibleItems.Add(ItemType.Painkillers);
        PossibleItems.Add(ItemType.Painkillers);
        PossibleItems.Add(ItemType.Radio);
        PossibleItems.Add(ItemType.KeycardO5);
        PossibleItems.Add(ItemType.SCP018);
        PossibleItems.Add(ItemType.SCP207);
        PossibleItems.Add(ItemType.SCP268);
        PossibleItems.Add(ItemType.SCP500);
        PossibleItems.Add(ItemType.SCP1576);
        PossibleItems.Add(ItemType.SCP2176);
        PossibleItems.Add(ItemType.Jailbird);
        
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.FlippingCoin -= OnFlipCoin;
        
        PossibleItems.Clear();
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "Mystery Box",
        FormattedName = "Mystery Box",
        Aliases = new[] { "box" },
        Description = "A mystery box that gives you a random item.",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false
    };
}