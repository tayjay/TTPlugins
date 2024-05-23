using System.Collections.Generic;
using System.Linq;
using CustomPlayerEffects;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using HarmonyLib;
using InventorySystem.Items.Firearms.Attachments;
using InventorySystem.Items.Firearms.Attachments.Components;
using MEC;
using RoundModifiers.API;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;
using TTCore.Extensions;
using TTCore.HUDs;
using TTCore.Utilities;
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
    
    public Dictionary<ushort, Stat> WeaponStats { get; set; }
    
    public List<Stat> AllStats { get; set; }
    
    public List<WeightedItem<Stat>> WeightedStats { get; set; }
    
    public void OnInspectFirearm(InspectFirearmEventArgs ev)
    {
        
        
        if(WeaponStats.TryGetValue(ev.Serial, out var stat) && stat is IInspecting inspecting)
        {
            inspecting.Inspecting(ev);
        }
        else
        {
            string description = $"<size=75%>Name: {WeaponStats[ev.Serial].Name}\n" +
                                 $"Description: {WeaponStats[ev.Serial].Description}</size>";
            ev.Player.ShowHUDHint(description, 5f);
        }
    }
    
    
    //Adding the modifiers to the weapons
    //Player ItemAddedEventArgs
    public void OnItemAdded(ItemAddedEventArgs ev)
    {
        if(ev.Item is Firearm firearm)
        {
            if (!WeaponStats.ContainsKey(ev.Item.Serial))
            {
                WeightedRandomSelector<Stat> selector = new WeightedRandomSelector<Stat>(WeightedStats);
                Stat stat = selector.SelectItem();
                WeaponStats.Add(ev.Item.Serial, stat);
                if(stat is IInitializing initializing)
                {
                    initializing.Initializing(firearm);
                }
            }
        }
    }
    //Map SpawningItemEventArgs
    public void OnSpawningItem(SpawningItemEventArgs ev)
    {
        if (ev.Pickup is FirearmPickup fp)
        {
            if (!WeaponStats.ContainsKey(ev.Pickup.Serial))
            {
                WeightedRandomSelector<Stat> selector = new WeightedRandomSelector<Stat>(WeightedStats);
                Stat stat = selector.SelectItem();
                WeaponStats.Add(ev.Pickup.Serial, stat);
                if(stat is IInitializing initializing)
                {
                    initializing.Initializing(Firearm.Get(ev.Pickup.Serial) as Firearm);
                }
            }
        }
    }

    public void OnChangedItem(ChangedItemEventArgs ev)
    {
        if (ev.Item is Firearm oldFirearm)
        {
            if(WeaponStats.TryGetValue(oldFirearm.Serial, out var stat) && stat is IAdding adding)
            {
                adding.Removing(oldFirearm, ev.Player);
            }
        }
        if (ev.Item is Firearm firearm)
        {
            if (WeaponStats.TryGetValue(firearm.Serial, out var stat))
            {
                if (stat is IAdding adding)
                {
                    adding.Adding(firearm, ev.Player);
                }
                ev.Player.ShowHUDHint($"<size=75%>{stat.Name}</size>", 2f);
            }
            
        }
    }

    public void OnShot(ShotEventArgs ev)
    {
        //if(ev.Target != null){Log.Debug(ev.Target.Nickname);} 
        
        if (WeaponStats.TryGetValue(ev.Firearm.Serial, out var stat) && stat is IShooting shooting)
        {
            shooting.OnShot(ev);
        }
    }
    
    public void OnShooting(ShootingEventArgs ev)
    {
        if (WeaponStats.TryGetValue(ev.Firearm.Serial, out var stat) && stat is IShooting shooting)
        {
            shooting.OnShooting(ev);
        }
    }
    
    public void OnRoundStart()
    {
        _tickHandle = Timing.RunCoroutine(Tick());
    }

    public void OnHurt(HurtEventArgs ev)
    {
        if(ev.Attacker?.CurrentItem is Firearm attFirearm)
        {
            if (WeaponStats.TryGetValue(attFirearm.Serial, out var stat) && stat is IHurting hurting)
            {
                hurting.OnTargetHurt(ev);
            }
        }
        if(ev.Player?.CurrentItem is Firearm vicFirearm)
        {
            if (WeaponStats.TryGetValue(vicFirearm.Serial, out var stat) && stat is IHurting hurting)
            {
                hurting.OnOwnerHurt(ev);
            }
        }
    }
    
    public void OnHurting(HurtingEventArgs ev)
    {
        if(ev.Attacker?.CurrentItem is Firearm attFirearm)
        {
            if (WeaponStats.TryGetValue(attFirearm.Serial, out var stat) && stat is IHurting hurting)
            {
                hurting.OnTargetHurting(ev);
            }
        }
        if(ev.Player?.CurrentItem is Firearm vicFirearm)
        {
            if (WeaponStats.TryGetValue(vicFirearm.Serial, out var stat) && stat is IHurting hurting)
            {
                hurting.OnOwnerHurting(ev);
            }
        }
    }

    public void OnDying(DyingEventArgs ev)
    {
        if(ev.Attacker?.CurrentItem is Firearm attFirearm)
        {
            if (WeaponStats.TryGetValue(attFirearm.Serial, out var stat) && stat is IDying dying)
            {
                dying.OnTargetDying(ev);
            }
        }
        if(ev.Player?.CurrentItem is Firearm vicFirearm)
        {
            if (WeaponStats.TryGetValue(vicFirearm.Serial, out var stat) && stat is IDying dying)
            {
                dying.OnOwnerDying(ev);
            }
        }
    }
    
    public void OnDied(DiedEventArgs ev)
    {
        if(ev.Attacker?.CurrentItem is Firearm attFirearm)
        {
            if (WeaponStats.TryGetValue(attFirearm.Serial, out var stat) && stat is IDying dying)
            {
                dying.OnTargetDied(ev);
            }
        }
        if(ev.Player?.CurrentItem is Firearm vicFirearm)
        {
            if (WeaponStats.TryGetValue(vicFirearm.Serial, out var stat) && stat is IDying dying)
            {
                dying.OnOwnerDied(ev);
            }
        }
    }

    public void OnReloading(ReloadingWeaponEventArgs ev)
    {
        if (WeaponStats.TryGetValue(ev.Firearm.Serial, out var stat) && stat is IReloading reloading)
        {
            reloading.OnReloading(ev);
        }
    }
    
    static CoroutineHandle _tickHandle;
    public IEnumerator<float> Tick()
    {
        yield return Timing.WaitForSeconds(1f);
        while (true)
        {
            foreach(Player player in Player.List.Where(p=>p.IsHuman))
            {
                try
                {
                    if(player.TryGetPickupOnSight(10,out Pickup pickup))
                    {
                        if(!(Item.Get(pickup.Serial) is Firearm firearm)) continue;
                        if(WeaponStats.TryGetValue(pickup.Serial, out var stat))
                        {
                            string output = $"<size=75%>{stat.Name}\n" +
                                            $"{stat.Description}</size>";
                            player.ShowHUDHint(output,2f);
                        }
                    }
                } catch (System.Exception e)
                {
                    Log.Error($"Error in WeaponStats tick: {e}");
                }

                yield return Timing.WaitForOneFrame;
            }
            yield return Timing.WaitForSeconds(0.5f);
        }
    }
    
    private void WeighStats()
    {
        foreach (Stat stat in AllStats)
        {
            WeightedStats.Add(new WeightedItem<Stat>(stat, stat.CalculateWeight()));
        }
    }
    
    
    protected override void RegisterModifier()
    {
        WeaponStats = DictionaryPool<ushort, Stat>.Pool.Get();
        AllStats = ListPool<Stat>.Pool.Get();
        WeightedStats = ListPool<WeightedItem<Stat>>.Pool.Get();
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Map.SpawningItem += OnSpawningItem;
        
        Exiled.Events.Handlers.Player.ItemAdded += OnItemAdded;
        Exiled.Events.Handlers.Player.ChangedItem += OnChangedItem;
        Exiled.Events.Handlers.Player.Hurt += OnHurt;
        Exiled.Events.Handlers.Player.Hurting += OnHurting;
        Exiled.Events.Handlers.Player.Dying += OnDying;
        Exiled.Events.Handlers.Player.Died += OnDied;
        Exiled.Events.Handlers.Player.ReloadingWeapon += OnReloading;
        Exiled.Events.Handlers.Player.Shooting += OnShooting;
        Exiled.Events.Handlers.Player.Shot += OnShot;
        
        Custom.InspectFirearm += OnInspectFirearm;
        
        AllStats.Add(new NoneStat());
        //AllStats.Add(new EffectStat(EffectType.Ghostly, 1));
        AllStats.Add(new ShootEffectStat(EffectType.Scp1853, 2, 1));
        AllStats.Add(new ShootEffectStat(EffectType.Scp207, 1,1));
        AllStats.Add(new ShootEffectStat(EffectType.DamageReduction, 1,1));
        AllStats.Add(new ShootEffectStat(EffectType.Ghostly, 1,1));
        AllStats.Add(new ShootEffectStat(EffectType.AntiScp207, 1,1));
        AllStats.Add(new ShootEffectStat(EffectType.RainbowTaste, 1,1));
        AllStats.Add(new SharpStat());
        AllStats.Add(new TargetShotEffectStat(effectFriendly:true, friendlyEffectType:EffectType.AntiScp207));
        AllStats.Add(new TargetShotEffectStat(effectEnemy:true, enemyEffectType:EffectType.SinkHole));
        AllStats.Add(new SlipperyStat());
        AllStats.Add(new CleansingStat());
        //AllStats.Add(new AnyAmmoStat());
        AllStats.Add(new CloneStat());
        AllStats.Add(new DefibStat());
        AllStats.Add(new ExtraAmmoStat(2));
        AllStats.Add(new ExtraAmmoStat(1.5f));
        AllStats.Add(new ExtraAmmoStat(1.75f));
        AllStats.Add(new ExtraDamageStat());
        AllStats.Add(new InstantReloadStat());
        AllStats.Add(new DangerStat());
        
        WeighStats();
        
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Map.SpawningItem -= OnSpawningItem;
        
        Exiled.Events.Handlers.Player.ItemAdded -= OnItemAdded;
        Exiled.Events.Handlers.Player.ChangedItem -= OnChangedItem;
        Exiled.Events.Handlers.Player.Hurt -= OnHurt;
        Exiled.Events.Handlers.Player.Hurting -= OnHurting;
        Exiled.Events.Handlers.Player.Dying -= OnDying;
        Exiled.Events.Handlers.Player.Died -= OnDied;
        Exiled.Events.Handlers.Player.ReloadingWeapon -= OnReloading;
        Exiled.Events.Handlers.Player.Shooting -= OnShooting;
        Exiled.Events.Handlers.Player.Shot -= OnShot;
        
        Custom.InspectFirearm -= OnInspectFirearm;
        
        
        
        DictionaryPool<ushort, Stat>.Pool.Return(WeaponStats);
        ListPool<Stat>.Pool.Return(AllStats);
        ListPool<WeightedItem<Stat>>.Pool.Return(WeightedStats);
        Timing.KillCoroutines(_tickHandle);
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "WeaponStats",
        Description = "Adds stats to weapons",
        Aliases = new []{"stats"},
        FormattedName = "<color=green>Weapon Stats</color>",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = true
    };
}