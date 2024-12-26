using System.Collections.Generic;
using Exiled.API.Extensions;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Scp914;
using RoundModifiers.API;
using Scp914;
using Scp914.Processors;

namespace RoundModifiers.Modifiers;

public class RandomScp914 : Modifier
{
    
    public List<ItemType> SafeItems = new List<ItemType>
    {
        ItemType.KeycardJanitor,
        ItemType.KeycardScientist,
        ItemType.KeycardO5,
        ItemType.KeycardContainmentEngineer,
        ItemType.KeycardGuard,
        ItemType.KeycardChaosInsurgency,
        ItemType.KeycardZoneManager,
        ItemType.KeycardFacilityManager,
        ItemType.Adrenaline,
        ItemType.Coin,
        ItemType.Flashlight,
        ItemType.Jailbird,
        ItemType.Lantern,
        ItemType.Medkit,
        ItemType.Painkillers,
        ItemType.Radio,
        ItemType.ArmorCombat,
        ItemType.ArmorHeavy,
        ItemType.ArmorLight,
        ItemType.GrenadeFlash,
        ItemType.GrenadeHE,
        ItemType.GunA7,
        ItemType.GunCom45,
        ItemType.GunCrossvec,
        ItemType.GunLogicer,
        ItemType.GunRevolver,
        ItemType.GunShotgun,
        ItemType.GunAK,
        ItemType.ParticleDisruptor,
        ItemType.SCP018,
        ItemType.SCP207,
        ItemType.SCP244a,
        ItemType.SCP244b,
        ItemType.SCP268,
        ItemType.SCP500,
        ItemType.SCP1576,
        ItemType.SCP1853,
        ItemType.SCP2176,
        ItemType.AntiSCP207,
        ItemType.MicroHID,
        ItemType.GunFRMG0,
        ItemType.GunFSP9,
    };
        
    
    private void OnUpgradingPickup(UpgradingPickupEventArgs ev)
    {
        // Easiest way to handle this to to get the item, setting, and output location
        // Cancel event if you want to make a change
        // Move the pickup to the new location if there is no change
        // Destroy the pickup and create a new one at the new location if you want to change the item
        if (ev.Pickup.Base.Info.ItemId.IsAmmo()) return;
        Pickup.CreateAndSpawn(SafeItems.RandomItem(), ev.OutputPosition, ev.Pickup.Base.transform.rotation);
        ev.Pickup.Destroy();
        ev.IsAllowed = false;
    }

    private void OnUpgradingPlayer(UpgradingPlayerEventArgs ev)
    {
        
    }

    private void OnUpgradingInventoryItem(UpgradingInventoryItemEventArgs ev)
    {
        if(ev.Item.IsAmmo || ev.Item.IsArmor) return;
        ev.Player.RemoveItem(ev.Item);
        ev.Player.AddItem(SafeItems.RandomItem());
        ev.IsAllowed = false;
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Scp914.UpgradingPickup += OnUpgradingPickup;
        Exiled.Events.Handlers.Scp914.UpgradingPlayer += OnUpgradingPlayer;
        Exiled.Events.Handlers.Scp914.UpgradingInventoryItem += OnUpgradingInventoryItem;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Scp914.UpgradingPickup -= OnUpgradingPickup;
        Exiled.Events.Handlers.Scp914.UpgradingPlayer -= OnUpgradingPlayer;
        Exiled.Events.Handlers.Scp914.UpgradingInventoryItem -= OnUpgradingInventoryItem;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "RandomScp914",
        FormattedName = "<color=red>Random SCP-914</color>",
        Aliases = new []{"random914"},
        Description = "SCP-914 will upgrade items to random items.",
        Impact = ImpactLevel.MajorGameplay,
        MustPreload = false,
        Balance = 3,
        Category = Category.Scp914
    };
}