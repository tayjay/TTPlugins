using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Usables.Scp330;
using RoundModifiers.API;
using TTCore.HUDs;

namespace RoundModifiers.Modifiers;

public class Pills : Modifier
{
    private Dictionary<uint, int> pillCount = new Dictionary<uint, int>();
    
    public void OnRoundStart()
    {
        pillCount.Clear();
    }

    public void OnTakePills(UsedItemEventArgs ev)
    {
        if(ev.Item.Type != ItemType.Painkillers) return;
        if(pillCount.ContainsKey(ev.Player.NetId))
        {
            pillCount[ev.Player.NetId]++;
        }
        else
        {
            pillCount[ev.Player.NetId] = 1;
        }
        
        GiveEffect(ev.Player);
    }
    
    public void GiveEffect(Player player)
    {
        if(pillCount.ContainsKey(player.NetId))
        {
            //CandyKindID[] AllCandies = (CandyKindID[]) System.Enum.GetValues(typeof(CandyKindID));
            List<CandyKindID> candies = new List<CandyKindID>();
            candies.Add(CandyKindID.Rainbow);
            candies.Add(CandyKindID.Blue);
            candies.Add(CandyKindID.Green);
            candies.Add(CandyKindID.Purple);
            candies.Add(CandyKindID.Red);
            candies.Add(CandyKindID.Yellow);
            if(pillCount[player.NetId] >= 3)
            {
                candies.Add(CandyKindID.None);
                candies.Add(CandyKindID.None);
            }
            if(pillCount[player.NetId] >= 5)
            {
                candies.Add(CandyKindID.Pink);
            }
            CandyKindID candy = candies.RandomItem();
            if (candy == CandyKindID.None)
            {
                EffectCategory category = EffectCategory.Positive;
                EffectType effect = player.ApplyRandomEffect(category, 1, 0, true);
                player.ShowHUDHint("You feel "+effect.ToString(), 5f);
            }
            else
            {
                Scp330.AvailableCandies[candy].ServerApplyEffects(player.ReferenceHub);
                player.ShowHUDHint("You taste the colour "+candy.ToString(), 5f);
            }
            
        }
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.UsedItem += OnTakePills;
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.UsedItem -= OnTakePills;
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "Pills",
        Aliases = new [] {"pills"},
        Description = "Painkillers give you a random effect",
        FormattedName = "<color=yellow>Painkiller Roulette</color>",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false
    };
}