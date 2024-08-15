using System.ComponentModel;
using System.Linq;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Firearms.Attachments;
using MapGeneration.Distributors;
using RoundModifiers.API;
using TTCore.HUDs;
using UnityEngine;

namespace RoundModifiers.Modifiers;

public class PayToWin : Modifier
{
    
    public static Config PayToWinConfig => RoundModifiers.Instance.Config.PayToWin;

    public void OnInteractLocker(InteractingLockerEventArgs ev)
    {
        if(ev.Locker is PedestalScpLocker pedestalScpLocker)
        {
            if (ev.Chamber.IsOpen)
            {
                ev.IsAllowed = false; // Prevent closing a paid locker
                return;
            }

            if (ev.Player.CurrentItem.Type == ItemType.Coin)
            {
                ev.IsAllowed = true;
                ev.Player.RemoveHeldItem();
            }
            else
            {
                ev.Player.ShowHUDHint("You need a coin to open this locker!");
                ev.IsAllowed = false;
            }
        }
    }
    
    public void OnInteractGenerator(UnlockingGeneratorEventArgs ev)
    {
        if (ev.Player.CurrentItem.Type == ItemType.Coin)
        {
            ev.IsAllowed = true;
            ev.Player.RemoveHeldItem();
        }
        else
        {
            ev.Player.ShowHUDHint("You need a coin to unlock this generator!");
            ev.IsAllowed = false;
        }
    }

    public void OnRoundStart()
    {
        foreach(WorkstationController workstation in WorkstationController.AllWorkstations)
        {
            var transform = workstation.transform;
            for (int i = 0; i < PayToWinConfig.CoinsPerWorkstation; i++)
            {
                Pickup.CreateAndSpawn(ItemType.Coin, transform.position + Vector3.up, transform.rotation);
            }
        }
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.InteractingLocker += OnInteractLocker;
        Exiled.Events.Handlers.Player.UnlockingGenerator += OnInteractGenerator;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.InteractingLocker -= OnInteractLocker;
        Exiled.Events.Handlers.Player.UnlockingGenerator -= OnInteractGenerator;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "PayToWin",
        FormattedName = "<color=yellow>Pay To Win</color>",
        Aliases = new []{"ptw"},
        Description = "Players need to pay to open SCP lockers.",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false,
        Balance = -1,
        Category = Category.Utility
    };
    
    
    public class Config : ModConfig
    {
        [Description("The amount of coins to spawn in each workstation. Default is 1.")]
        public int CoinsPerWorkstation { get; set; } = 1;
    }
}