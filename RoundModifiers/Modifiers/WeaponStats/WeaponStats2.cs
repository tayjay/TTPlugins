using System.Collections.Generic;
using CustomPlayerEffects;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.API;
using TTCore.Events.EventArgs;
using UnityEngine;

namespace RoundModifiers.Modifiers.WeaponStats;

public class WeaponStats2 : Modifier
{
    /*
     * This modifier will change the stats of all weapons in the game.
     * Adds a type and rarity tag to all weapons.
     * Possible changes include:
     * DamageMultiplier
     * InaccuracyReduction
     * MovementSpeedMultiplier
     * AmmoMultiplier
     * ReloadSpeedMultiplier
     * FireRateMultiplier
     * DrawSpeedMultiplier
     * 
     * 
     */
    
    public Dictionary<ushort, Stats> WeaponStats { get; set; }
    
    public void OnInspectFirearm(InspectFirearmEventArgs ev)
    {
        throw new System.NotImplementedException();
    }
    
    
    //Adding the modifiers to the weapons
    //Player ItemAddedEventArgs
    public void OnItemAdded(ItemAddedEventArgs ev)
    {
        
        if (ev.Item is Firearm firearm)
        {
            if (!WeaponStats.ContainsKey(ev.Item.Serial))
            {
                Stats normalStats = new Stats()
                {
                    Type = "Normal",
                    Rarity = 1
                };
                WeaponStats.Add(ev.Item.Serial, normalStats);
            }
            firearm.FireRate *= WeaponStats[ev.Item.Serial].FireRateMultiplier;
            firearm.MaxAmmo = (byte)(firearm.MaxAmmo* WeaponStats[ev.Item.Serial].AmmoMultiplier);
            firearm.Scale = new Vector3(0.5f, 0.5f, 0.5f);
        }
    }
    //Map SpawningItemEventArgs
    public void OnSpawningItem(SpawningItemEventArgs ev)
    {
        if (ev.Pickup is FirearmPickup)
        {
            if (!WeaponStats.ContainsKey(ev.Pickup.Serial))
            {
                Stats normalStats = new Stats()
                {
                    Type = "Normal",
                    Rarity = 1
                };
                WeaponStats.Add(ev.Pickup.Serial, normalStats);
            }
        }
    }

    public void OnRoundStart()
    {
        foreach (Player player in Player.List)
        {
            foreach (StatusEffectBase effect in player.ReferenceHub.playerEffectsController.AllEffects)
            {
                Log.Debug(effect.GetType().Name);
            }
            //player.EnableEffect<WeaponStatsEffect>();
        }
    }
    
    
    protected override void RegisterModifier()
    {
        WeaponStats = DictionaryPool<ushort, Stats>.Pool.Get();
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        DictionaryPool<ushort, Stats>.Pool.Return(WeaponStats);
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "WeaponStats",
        Description = "Adds stats to weapons",
        Aliases = new []{"stats"},
        FormattedName = "<color=green>Weapon</color><color=red>Stats</color>",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false
    };
    
    public struct Stats
    {
        public string Type;
        public int Rarity;
        public float DamageMultiplier;
        public float InaccuracyReduction;
        public float MovementSpeedMultiplier;
        public float AmmoMultiplier;
        public float ReloadSpeedMultiplier;
        public float FireRateMultiplier;
        public float DrawSpeedMultiplier;

        public Stats()
        {
            InaccuracyReduction = 1;
            MovementSpeedMultiplier = 1;
            AmmoMultiplier = 1;
            ReloadSpeedMultiplier = 1;
            FireRateMultiplier = 1;
            DrawSpeedMultiplier = 1;
        }
    }
}