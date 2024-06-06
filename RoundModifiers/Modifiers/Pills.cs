using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.Usables.Scp330;
using NorthwoodLib.Pools;
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
            List<CandyKindID> candies = Exiled.API.Features.Pools.ListPool<CandyKindID>.Pool.Get();
            candies.Add(CandyKindID.Rainbow);
            candies.Add(CandyKindID.Blue);
            candies.Add(CandyKindID.Green);
            candies.Add(CandyKindID.Purple);
            candies.Add(CandyKindID.Red);
            candies.Add(CandyKindID.Yellow);
            if(pillCount[player.NetId] >= 1)
            {
                for(int i =0;i<pillCount[player.NetId];i++)
                {
                    candies.Add(CandyKindID.None);
                }
            }
            if(pillCount[player.NetId] >= 5)
            {
                candies.Add(CandyKindID.Pink);
            }
            CandyKindID candy = candies.RandomItem();
            if (candy == CandyKindID.None)
            {
                List<EffectCategory> categories = Exiled.API.Features.Pools.ListPool<EffectCategory>.Pool.Get();
                categories.Add(EffectCategory.Positive);
                if (pillCount[player.NetId] >= 2)
                {
                    categories.Add(EffectCategory.Positive);
                    categories.Add(EffectCategory.Movement);
                }
                if(pillCount[player.NetId] >= 3)
                {
                    categories.Add(EffectCategory.Positive);
                    categories.Add(EffectCategory.Negative);
                }
                if(pillCount[player.NetId] >= 4)
                {
                    categories.Add(EffectCategory.Positive);
                    categories.Add(EffectCategory.Harmful);
                }
                EffectCategory category = categories.RandomItem();
                float duration = 0f;
                if (category == EffectCategory.Harmful)
                {
                    duration = 5f;
                }
                else if (category == EffectCategory.Negative || category == EffectCategory.Movement)
                {
                    duration = 30f;
                }
                else if (category == EffectCategory.Positive)
                {
                    duration = 120f;
                }
                EffectType effect = player.ApplyRandomEffect(categories.RandomItem(), duration);
                Exiled.API.Features.Pools.ListPool<EffectCategory>.Pool.Return(categories);
            }
            else
            {
                Scp330.AvailableCandies[candy].ServerApplyEffects(player.ReferenceHub);
                player.ShowHUDHint("You taste the colour "+candy.ToString(), 5f);
            }

            Exiled.API.Features.Pools.ListPool<CandyKindID>.Pool.Return(candies);
            
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
        MustPreload = false,
        Balance = 2,
        Category = Category.Utility
    };
}