using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Pickups;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp914;
using InventorySystem.Items.Firearms;
using InventorySystem.Items.Usables;
using MEC;
using Scp914;
using TTCore.Events.EventArgs;
using TTCore.Extensions;
using TTCore.HUDs;
using UnityEngine;
using Firearm = Exiled.API.Features.Items.Firearm;
using Random = UnityEngine.Random;
/*
namespace TTAddons.Handlers
{
    public class WeaponStats
    {
        public static Dictionary<ushort, WeaponStats> WeaponStatsDict = new Dictionary<ushort, WeaponStats>();

        public static WeaponStats GetWeaponStats(Firearm item)
        {
            if(item==null) return null;
            if (WeaponStatsDict.TryGetValue(item.Serial, out var stats))
            {
                return stats;
            }
            else
            {
                WeaponStatsDict[item.Serial] = new WeaponStats(item);
                return WeaponStatsDict[item.Serial];
            }
        }


        public Dictionary<VariableType, Modification> Modifications = new Dictionary<VariableType, Modification>();

        private Modification AdsInaccuracyMod {
            get => Modifications[VariableType.AdsInaccuracy]; set => Modifications[VariableType.AdsInaccuracy] = value;
        }
        private Modification BaseDamageMod {
            get => Modifications[VariableType.BaseDamage]; set => Modifications[VariableType.BaseDamage] = value;
        }
        private Modification BaseDrawTimeMod {
            get => Modifications[VariableType.BaseDrawTime]; set => Modifications[VariableType.BaseDrawTime] = value;
        }
        private Modification BasePenetrationPercentMod {
            get => Modifications[VariableType.BasePenetrationPercent]; set => Modifications[VariableType.BasePenetrationPercent] = value;
        }
        private Modification BulletInaccuracyMod {
            get => Modifications[VariableType.BulletInaccuracy]; set => Modifications[VariableType.BulletInaccuracy] = value;
        }
        private Modification DamageFalloffMod {
            get => Modifications[VariableType.DamageFalloff]; set => Modifications[VariableType.DamageFalloff] = value;
        }
        private Modification FullDamageDistanceMod {
            get => Modifications[VariableType.FullDamageDistance]; set => Modifications[VariableType.FullDamageDistance] = value;
        }
        private Modification HipInaccuracyMod {
            get => Modifications[VariableType.HipInaccuracy]; set => Modifications[VariableType.HipInaccuracy] = value;
        }
        /*private Modification RateOfFireMod {
            get => Modifications[VariableType.RateOfFire]; set => Modifications[VariableType.RateOfFire] = value;
        }
        private Modification AmmoCapacityMod {
            get => Modifications[VariableType.AmmoCapacity]; set => Modifications[VariableType.AmmoCapacity] = value;
        }#1#

        private bool _isChanged = false;

        public ushort Serial { get; private set; }

        public Firearm Firearm => (Firearm)Item.Get(Serial);

        public string Prefix { get; private set; }
        public byte Rarity { get; private set; }

        public WeaponStats(Firearm firearm)
        {
            Serial = firearm.Serial;
            Initialize(firearm);
        }

        public void Initialize(Firearm item)
        {
            Modifications[VariableType.AdsInaccuracy] = new Modification(ModType.Multiplicative, VariableType.AdsInaccuracy, 1f);
            Modifications[VariableType.BaseDamage] = new Modification(ModType.Multiplicative, VariableType.BaseDamage, 1f);
            Modifications[VariableType.BaseDrawTime] = new Modification(ModType.Multiplicative, VariableType.BaseDrawTime, 1f);
            Modifications[VariableType.BasePenetrationPercent] = new Modification(ModType.Multiplicative, VariableType.BasePenetrationPercent, 1f);
            Modifications[VariableType.BulletInaccuracy] = new Modification(ModType.Multiplicative, VariableType.BulletInaccuracy, 1f);
            Modifications[VariableType.DamageFalloff] = new Modification(ModType.Multiplicative, VariableType.DamageFalloff, 1f);
            Modifications[VariableType.FullDamageDistance] = new Modification(ModType.Multiplicative, VariableType.FullDamageDistance, 1f);
            Modifications[VariableType.HipInaccuracy] = new Modification(ModType.Multiplicative, VariableType.HipInaccuracy, 1f);
            //Modifications[VariableType.RateOfFire] = new Modification(ModType.Multiplicative, VariableType.RateOfFire, 1f);
            //Modifications[VariableType.AmmoCapacity] = new Modification(ModType.Multiplicative, VariableType.AmmoCapacity, 1f);


            int random = Random.Range(0, 6);
            switch (random)
            {
                case 0:
                    SetupHarmful();
                    break;
                case 1:
                    SetupAccurate();
                    break;
                case 2:
                    SetupBalanced();
                    break;
                case 3:
                    SetupSpeedy();
                    break;
                case 4:
                    SetupPowerful();
                    break;
                default:
                    SetupRandom();
                    break;
            }
            _isChanged = false;
        }

        public void SetupHarmful()
        {
            int rarity = Random.Range(0, 5);
            Prefix = "Harmful";
            Rarity = (byte)(rarity+1);

            float rangeMin = 1+ 0.1f*rarity;
            float rangeMax = 1+ 0.5f*rarity;

            Modifications[VariableType.BaseDamage] = new Modification(ModType.Multiplicative, VariableType.BaseDamage, rangeMax);
            Modifications[VariableType.BulletInaccuracy] = new Modification(ModType.Multiplicative, VariableType.BulletInaccuracy, 1+(0.2f*rarity));
            //Modifications[VariableType.AmmoCapacity] = new Modification(ModType.Multiplicative, VariableType.AmmoCapacity, rangeMin);

        }

        public void SetupSpeedy()
        {
            int rarity = Random.Range(0, 5);
            Prefix = "Speedy";
            Rarity = (byte)(rarity+1);

            Modifications[VariableType.BaseDrawTime] = new Modification(ModType.Multiplicative, VariableType.BaseDrawTime, 1-(0.2f*rarity));
            //Modifications[VariableType.RateOfFire] = new Modification(ModType.Multiplicative, VariableType.RateOfFire, 1+(0.1f*rarity));
            Modifications[VariableType.BaseDamage] = new Modification(ModType.Additive, VariableType.BaseDamage, -2-(1*rarity));
        }

        public void SetupBalanced()
        {
            int rarity = Random.Range(0, 5);
            Prefix = "Balanced";
            Rarity = (byte)(rarity+1);

            Modifications[VariableType.AdsInaccuracy] = new Modification(ModType.Multiplicative, VariableType.AdsInaccuracy, 1-(0.05f*rarity));
            Modifications[VariableType.BaseDamage] = new Modification(ModType.Multiplicative, VariableType.BaseDamage, 1f+(0.05f*rarity));
            Modifications[VariableType.BaseDrawTime] = new Modification(ModType.Multiplicative, VariableType.BaseDrawTime, 1f-(0.05f*rarity));
            Modifications[VariableType.BasePenetrationPercent] = new Modification(ModType.Multiplicative, VariableType.BasePenetrationPercent, 1f+(0.05f*rarity));
            Modifications[VariableType.BulletInaccuracy] = new Modification(ModType.Multiplicative, VariableType.BulletInaccuracy, 1f-(0.05f*rarity));
            Modifications[VariableType.DamageFalloff] = new Modification(ModType.Multiplicative, VariableType.DamageFalloff, 1f-(0.05f*rarity));
            Modifications[VariableType.FullDamageDistance] = new Modification(ModType.Multiplicative, VariableType.FullDamageDistance, 1f+(0.05f*rarity));
            Modifications[VariableType.HipInaccuracy] = new Modification(ModType.Multiplicative, VariableType.HipInaccuracy, 1f-(0.05f*rarity));
            //Modifications[VariableType.RateOfFire] = new Modification(ModType.Multiplicative, VariableType.RateOfFire, 1f+(0.05f*rarity));
            //Modifications[VariableType.AmmoCapacity] = new Modification(ModType.Multiplicative, VariableType.AmmoCapacity, 1f+(0.05f*rarity));

        }

        public void SetupAccurate()
        {
            int rarity = Random.Range(0, 5);
            Prefix = "Accurate";
            Rarity = (byte)(rarity+1);

            Modifications[VariableType.BulletInaccuracy] = new Modification(ModType.Multiplicative, VariableType.BulletInaccuracy, 0.8f-(0.2f*rarity));
            Modifications[VariableType.AdsInaccuracy] = new Modification(ModType.Multiplicative, VariableType.AdsInaccuracy, 0.8f-(0.2f*rarity));
            Modifications[VariableType.HipInaccuracy] = new Modification(ModType.Multiplicative, VariableType.HipInaccuracy, 0.8f-(0.2f*rarity));
            Modifications[VariableType.FullDamageDistance] =
                new Modification(ModType.Additive, VariableType.FullDamageDistance, 200);
            Modifications[VariableType.BaseDamage] = new Modification(ModType.Multiplicative, VariableType.BaseDamage, 1f+(0.2f*rarity));
            Modifications[VariableType.BaseDrawTime] = new Modification(ModType.Multiplicative, VariableType.BaseDrawTime, 1.2f+(0.5f*rarity));
        }

        public void SetupPowerful()
        {
            int rarity = Random.Range(0, 5);
            Prefix = "Powerful";
            Rarity = (byte)(rarity+1);

            Modifications[VariableType.AdsInaccuracy] = new Modification(ModType.Multiplicative, VariableType.AdsInaccuracy, 1-(0.1f*rarity));
            Modifications[VariableType.BaseDamage] = new Modification(ModType.Multiplicative, VariableType.BaseDamage, 1f+(0.1f*rarity));
            Modifications[VariableType.BaseDrawTime] = new Modification(ModType.Multiplicative, VariableType.BaseDrawTime, 1f-(0.1f*rarity));
            Modifications[VariableType.BasePenetrationPercent] = new Modification(ModType.Multiplicative, VariableType.BasePenetrationPercent, 1f+(0.1f*rarity));
            Modifications[VariableType.BulletInaccuracy] = new Modification(ModType.Multiplicative, VariableType.BulletInaccuracy, 1f-(0.1f*rarity));
            Modifications[VariableType.DamageFalloff] = new Modification(ModType.Multiplicative, VariableType.DamageFalloff, 1f-(0.1f*rarity));
            Modifications[VariableType.FullDamageDistance] = new Modification(ModType.Multiplicative, VariableType.FullDamageDistance, 1f+(0.1f*rarity));
            Modifications[VariableType.HipInaccuracy] = new Modification(ModType.Multiplicative, VariableType.HipInaccuracy, 1f-(0.1f*rarity));
            //Modifications[VariableType.RateOfFire] = new Modification(ModType.Multiplicative, VariableType.RateOfFire, 1f+(0.1f*rarity));
            //Modifications[VariableType.AmmoCapacity] = new Modification(ModType.Multiplicative, VariableType.AmmoCapacity, 1f+(0.1f*rarity));
        }

        public void SetupRandom()
        {
            Prefix = "Random ";
            Rarity = (byte)Random.Range(0, 5);
            for (int i = 0; i < 10; i++)
            {
                VariableType type = (VariableType)Random.Range(0, 10);
                ModType mod = (ModType)Random.Range(0, 3);
                float value = Random.Range(0.1f, 2f);
                Modifications[type] = new Modification(mod, type, value);
            }
        }


        public void Change()
        {
            if(_isChanged) return;
            //Firearm.FireRate = (RateOfFireMod.Mod == ModType.Set) ? RateOfFireMod.Value : (RateOfFireMod.Mod == ModType.Additive) ? Firearm.FireRate + RateOfFireMod.Value : Firearm.FireRate * RateOfFireMod.Value;
            //Firearm.MaxAmmo = (byte)((AmmoCapacityMod.Mod == ModType.Set) ? AmmoCapacityMod.Value : (AmmoCapacityMod.Mod == ModType.Additive) ? Firearm.FireRate + AmmoCapacityMod.Value : Firearm.FireRate * AmmoCapacityMod.Value);

            _isChanged = true;
        }


        //====================================================================================================================//

        public static void OnRoundStart()
        {
            _tickHandle = Timing.RunCoroutine(Tick());
        }

        public static void OnRoundRestart()
        {
            WeaponStatsDict.Clear();
            Timing.KillCoroutines(_tickHandle);
        }

        static CoroutineHandle _tickHandle;
        public static IEnumerator<float> Tick()
        {
            yield return Timing.WaitForSeconds(1f);
            while (true)
            {
                foreach(Player player in Player.List.Where(p=>p.IsHuman))
                {
                    if(player.TryGetPickupOnSight(10,out Pickup pickup))
                    {
                        if(!(Item.Get(pickup.Serial) is Firearm firearm)) continue;
                        WeaponStats stats = GetWeaponStats(firearm);
                        if(stats==null) continue;

                        string output = $"<size=70%>{stats.Prefix} {firearm.FirearmType} </size><size=90%>";
                        for(int i=0;i<5;i++)
                        {
                            if(i<stats.Rarity)
                                output += "<color=#e6c300>*</color>";
                            else
                                output += "<color=#616161>*</color>";
                        }
                        output+=$"</size>";

                        player.ShowHUDHint(output,2f);
                    }

                    yield return Timing.WaitForOneFrame;
                }
                yield return Timing.WaitForSeconds(0.5f);
            }
        }

        public static void OnGiveItem(ItemAddedEventArgs ev)
        {
            if (ev.Item is Firearm firearm)
            {
                GetWeaponStats(firearm)?.Change();
            }
        }

        public static void OnSpawnPickup(PickupAddedEventArgs ev)
        {
            if(Item.Get(ev.Pickup.Serial) is Firearm firearm)
            {
                GetWeaponStats(firearm)?.Change();
            }
        }

        public static void OnInspectFirearm(InspectFirearmEventArgs ev)
        {
            Log.Debug("Handling inspect firearm event");
            WeaponStats weaponStats = GetWeaponStats(ev.Firearm);
            FirearmBaseStats baseStats = ev.Firearm.Base.BaseStats;
            if(weaponStats == null) return;
            string stats = $"<size=50%>{weaponStats.Prefix} {ev.Firearm.FirearmType} </size><size=70%>";
                           for(int i=0;i<5;i++)
                           {
                               if(i<weaponStats.Rarity)
                                   stats += "<color=#e6c300>*</color>";
                               else
                                   stats += "<color=#616161>*</color>";
                           }
                           stats+=$":\n</size><size=40%>";
            foreach (var mod in weaponStats.Modifications)
            {
                stats += $"{mod.Key}: {((mod.Value.Mod==ModType.Multiplicative) ? "x" : (mod.Value.Mod == ModType.Additive ? (mod.Value.Value>0?"+":"") : "="))} {mod.Value.Value}\n";
            }
            stats += "</size>";
            ev.Player.ShowHUDHint(stats, 10f);
        }



        public static void ChangeWeaponStats(AccessFirearmBaseStatsEventArgs ev)
        {
            if(ev.Firearm == null) return;
            WeaponStats stats = GetWeaponStats(ev.Firearm);
            if(stats == null) return;
            float AdsInaccuracy = stats.AdsInaccuracyMod.Mod == ModType.Set ? stats.AdsInaccuracyMod.Value :
                stats.AdsInaccuracyMod.Mod == ModType.Additive ? ev.BaseStats.AdsInaccuracy + stats.AdsInaccuracyMod.Value :
                ev.BaseStats.AdsInaccuracy * stats.AdsInaccuracyMod.Value;
            float BaseDamage = stats.BaseDamageMod.Mod == ModType.Set ? stats.BaseDamageMod.Value :
                stats.BaseDamageMod.Mod == ModType.Additive ? ev.BaseStats.BaseDamage + stats.BaseDamageMod.Value :
                ev.BaseStats.BaseDamage * stats.BaseDamageMod.Value;
            float BaseDrawTime = stats.BaseDrawTimeMod.Mod == ModType.Set ? stats.BaseDrawTimeMod.Value :
                stats.BaseDrawTimeMod.Mod == ModType.Additive ? ev.BaseStats.BaseDrawTime + stats.BaseDrawTimeMod.Value :
                ev.BaseStats.BaseDrawTime * stats.BaseDrawTimeMod.Value;
            int BasePenetrationPercent = (int)(stats.BasePenetrationPercentMod.Mod == ModType.Set ? stats.BasePenetrationPercentMod.Value :
                stats.BasePenetrationPercentMod.Mod == ModType.Additive ? ev.BaseStats.BasePenetrationPercent + stats.BasePenetrationPercentMod.Value :
                ev.BaseStats.BasePenetrationPercent * stats.BasePenetrationPercentMod.Value);
            float BulletInaccuracy = stats.BulletInaccuracyMod.Mod == ModType.Set ? stats.BulletInaccuracyMod.Value :
                stats.BulletInaccuracyMod.Mod == ModType.Additive ? ev.BaseStats.BulletInaccuracy + stats.BulletInaccuracyMod.Value :
                ev.BaseStats.BulletInaccuracy * stats.BulletInaccuracyMod.Value;
            float DamageFalloff = stats.DamageFalloffMod.Mod == ModType.Set ? stats.DamageFalloffMod.Value :
                stats.DamageFalloffMod.Mod == ModType.Additive ? ev.BaseStats.DamageFalloff + stats.DamageFalloffMod.Value :
                ev.BaseStats.DamageFalloff * stats.DamageFalloffMod.Value;
            float FullDamageDistance = stats.FullDamageDistanceMod.Mod == ModType.Set ? stats.FullDamageDistanceMod.Value :
                stats.FullDamageDistanceMod.Mod == ModType.Additive ? ev.BaseStats.FullDamageDistance + stats.FullDamageDistanceMod.Value :
                ev.BaseStats.FullDamageDistance * stats.FullDamageDistanceMod.Value;
            float HipInaccuracy = stats.HipInaccuracyMod.Mod == ModType.Set ? stats.HipInaccuracyMod.Value :
                stats.HipInaccuracyMod.Mod == ModType.Additive ? ev.BaseStats.HipInaccuracy + stats.HipInaccuracyMod.Value :
                ev.BaseStats.HipInaccuracy * stats.HipInaccuracyMod.Value;



            //WeaponStats stats = GetWeaponStats(ev.Firearm);

            ev.BaseStats = new FirearmBaseStats()
            {
                AdsInaccuracy = AdsInaccuracy,
                BaseDamage = BaseDamage,
                BaseDrawTime = BaseDrawTime,
                BasePenetrationPercent = BasePenetrationPercent,
                BulletInaccuracy = BulletInaccuracy,
                DamageFalloff = DamageFalloff,
                FullDamageDistance = FullDamageDistance,
                HipInaccuracy = HipInaccuracy,
            };

        }


        public static void UpgradeItemPickup(UpgradingPickupEventArgs ev)
        {
            if (ev.KnobSetting != Scp914KnobSetting.OneToOne) return;
            if (Item.Get(ev.Pickup.Serial) is Firearm firearm)
            {
                WeaponStats stats = GetWeaponStats(firearm);
                if(stats == null) return;
                stats.Initialize(firearm);
                ev.IsAllowed = false;
            }
        }

        public static void UpgradeItemInventory(UpgradingInventoryItemEventArgs ev)
        {
            if(ev.KnobSetting != Scp914KnobSetting.OneToOne) return;
            if(ev.Item is Firearm firearm)
            {
                WeaponStats stats = GetWeaponStats(firearm);
                if(stats == null) return;
                stats.Initialize(firearm);
                ev.IsAllowed = false;
            }
        }

        public static void Register()
        {
            TTCore.Events.Handlers.Custom.InspectFirearm += OnInspectFirearm;
            TTCore.Events.Handlers.Custom.AccessFirearmBaseStats += ChangeWeaponStats;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestart;
            Exiled.Events.Handlers.Player.ItemAdded += OnGiveItem;
            Exiled.Events.Handlers.Map.PickupAdded += OnSpawnPickup;
            //Exiled.Events.Handlers.Scp914.UpgradingPickup += UpgradeItemPickup;
            //Exiled.Events.Handlers.Scp914.UpgradingInventoryItem += UpgradeItemInventory;
        }

        public static void Unregister()
        {
            TTCore.Events.Handlers.Custom.InspectFirearm -= OnInspectFirearm;
            TTCore.Events.Handlers.Custom.AccessFirearmBaseStats -= ChangeWeaponStats;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
            Exiled.Events.Handlers.Player.ItemAdded -= OnGiveItem;
            Exiled.Events.Handlers.Map.PickupAdded -= OnSpawnPickup;
            //Exiled.Events.Handlers.Scp914.UpgradingPickup -= UpgradeItemPickup;
            //Exiled.Events.Handlers.Scp914.UpgradingInventoryItem -= UpgradeItemInventory;

            WeaponStatsDict.Clear();
            Timing.KillCoroutines(_tickHandle);
        }



        //====================================================================================================================//

        public struct Modification
        {
            public ModType Mod { get; set; }
            public VariableType Variable { get; set; }
            public float Value { get; set; }

            public Modification(ModType type, VariableType variable, float value)
            {
                Mod = type;
                Variable = variable;
                Value = value;
            }
        }

        public enum ModType
        {
            Additive,
            Multiplicative,
            Set
        }

        public enum VariableType
        {
            AdsInaccuracy,
            BaseDamage,
            BaseDrawTime,
            BasePenetrationPercent,
            BulletInaccuracy,//Lower Better
            DamageFalloff,
            FullDamageDistance,
            HipInaccuracy,
            //RateOfFire,
            //AmmoCapacity
        }
    }
}*/