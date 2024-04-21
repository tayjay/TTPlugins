using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp914;
using InventorySystem.Items.Usables.Scp330;
using PlayerRoles;
using RoundModifiers.API;
using RoundModifiers.Modifiers.LevelUp.Boosts;
using RoundModifiers.Modifiers.LevelUp.HUD;
using RoundModifiers.Modifiers.LevelUp.Interfaces;
using RoundModifiers.Modifiers.LevelUp.XPs;
using TTCore.HUDs;

namespace RoundModifiers.Modifiers.LevelUp
{
    public class LevelUp : Modifier
    {
        //Store XP for players
        public Dictionary<uint,float> PlayerXP = new Dictionary<uint, float>();
        public Dictionary<uint, int> PlayerLevel = new Dictionary<uint, int>();
        
        public List<Boost> _boosts = new List<Boost>();
        public List<XP> _xp = new List<XP>();

        public LevelUp()
        {
            _xp.Add(new EscapeXP());
            _xp.Add(new DealDamageXP());
            _xp.Add(new ExploreZonesXP());
            _xp.Add(new Scp914UpgradingPlayerXP());
            _xp.Add(new EscapePocketDimensionXP());
            //_xp.Add(new DeathXP()); Review this as leveling up on death will give nothing.
            _xp.Add(new AliveXP());
            _xp.Add(new HandcuffXP());
            _xp.Add(new UsedItemXP());
            
            _boosts.Add(new GiveItemBoost(ItemType.Jailbird, Tier.Rare));
            _boosts.Add(new GiveItemBoost(ItemType.Adrenaline));
            _boosts.Add(new GiveItemBoost(ItemType.GrenadeHE, Tier.Uncommon));
            _boosts.Add(new GiveItemBoost(ItemType.Lantern)); 
            _boosts.Add(new GiveItemBoost(ItemType.Painkillers));
            _boosts.Add(new GiveItemBoost(ItemType.Medkit));
            _boosts.Add(new GiveItemBoost(ItemType.Radio));
            _boosts.Add(new GiveItemBoost(ItemType.ArmorHeavy, Tier.Rare));
            _boosts.Add(new GiveItemBoost(ItemType.ArmorLight, Tier.Uncommon));
            _boosts.Add(new GiveItemBoost(ItemType.GunRevolver, Tier.Rare));
            _boosts.Add(new GiveItemBoost(ItemType.KeycardFacilityManager, Tier.Epic));
            //_boosts.Add(new GiveItemBoost(ItemType.KeycardGuard, Tier.Uncommon));
            //_boosts.Add(new GiveItemBoost(ItemType.KeycardZoneManager));
            //_boosts.Add(new GiveItemBoost(ItemType.KeycardScientist));
            _boosts.Add(new GiveCandyBoost(CandyKindID.Pink, Tier.Epic));
            _boosts.Add(new GiveCandyBoost(CandyKindID.Blue, Tier.Uncommon));
            _boosts.Add(new GiveCandyBoost(CandyKindID.Rainbow, Tier.Uncommon));
            _boosts.Add(new GiveCandyBoost(CandyKindID.Red, Tier.Uncommon));
            _boosts.Add(new GiveCandyBoost(CandyKindID.Green, Tier.Uncommon));
            _boosts.Add(new GiveCandyBoost(CandyKindID.Yellow, Tier.Uncommon));
            _boosts.Add(new StatusEffectBoost(EffectType.Ghostly, 1, true, Tier.Epic));
            _boosts.Add(new StatusEffectBoost(EffectType.Vitality, 1, true, Tier.Uncommon));
            _boosts.Add(new StatusEffectBoost(EffectType.DamageReduction, 1, true, Tier.Uncommon));
            _boosts.Add(new StatusEffectBoost(EffectType.SilentWalk, 1, true, Tier.Legendary));
            _boosts.Add(new StatusEffectBoost(EffectType.Scp207, 1, true, Tier.Rare));
            _boosts.Add(new StatusEffectBoost(EffectType.AntiScp207, 1, true, Tier.Rare));
            _boosts.Add(new UpgradeKeycardBoost());
            _boosts.Add(new UpgradeKeycardBoost());
            _boosts.Add(new UpgradeKeycardBoost());
            _boosts.Add(new LightFootedBoost());
            _boosts.Add(new NoInstaKillBoost(Tier.Epic));
            _boosts.Add(new HealAOEBoost(Tier.Epic));
            _boosts.Add(new BecomeScpBoost(Tier.Legendary));
            _boosts.Add(new UpgradeWeaponBoost(Tier.Common));
            _boosts.Add(new UpgradeWeaponBoost(Tier.Uncommon));
            _boosts.Add(new ChangeSizeBoost());
        }
        
        //Store actions they have completed so they aren't completed more than once
        
        
        //Events to track player actions
        
        //On Deal Damage
        public void OnHurting(HurtingEventArgs ev)
        {
            //During player taking damage
            foreach (XP xp in _xp)
            {
                if(xp is IHurtingEvent)
                    ((IHurtingEvent) xp).OnHurting(ev);
            }

            foreach (Boost boost in _boosts)
            {
                if(boost is IHurtingEvent)
                    ((IHurtingEvent) boost).OnHurting(ev);
            }
        }

        public void OnHurt(HurtEventArgs ev)
        {
            
            //After player takes damage
            foreach (XP xp in _xp)
            {
                if(xp is IHurtEvent)
                    ((IHurtEvent) xp).OnHurt(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IHurtEvent)
                    ((IHurtEvent) boost).OnHurt(ev);
            }
        }
        
        //On Tick tracker register at player spawn
        
        //On Player Death
        public void OnDied(DiedEventArgs ev)
        {
            //Perform any actions after the player has actually died
            foreach (XP xp in _xp)
            {
                if(xp is IDiedEvent)
                    ((IDiedEvent) xp).OnDied(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IDiedEvent)
                    ((IDiedEvent) boost).OnDied(ev);
            }
        }

        public void OnDying(DyingEventArgs ev)
        {
            //Control player death
            foreach (XP xp in _xp)
            {
                if(xp is IDyingEvent)
                    ((IDyingEvent) xp).OnDying(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IDyingEvent)
                    ((IDyingEvent) boost).OnDying(ev);
            }
        }
        
        public void OnEscaping(EscapingEventArgs ev)
        {
            //Control player escape
            foreach (XP xp in _xp)
            {
                if(xp is IEscapingEvent)
                    ((IEscapingEvent) xp).OnEscaping(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IEscapingEvent)
                    ((IEscapingEvent) boost).OnEscaping(ev);
            }
        }
        
        public void OnEscapingPocketDimension(EscapingPocketDimensionEventArgs ev)
        {
            //Control player escape
            foreach (XP xp in _xp)
            {
                if(xp is IEscapingPocketDimensionEvent)
                    ((IEscapingPocketDimensionEvent) xp).OnEscapingPocketDimension(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IEscapingPocketDimensionEvent)
                    ((IEscapingPocketDimensionEvent) boost).OnEscapingPocketDimension(ev);
            }
        }
        
        public void OnMakingNoise(MakingNoiseEventArgs ev)
        {
            //Control player noise, stops footsteps from 939
            foreach (XP xp in _xp)
            {
                if(xp is IMakingNoiseEvent)
                    ((IMakingNoiseEvent) xp).OnMakingNoise(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IMakingNoiseEvent)
                    ((IMakingNoiseEvent) boost).OnMakingNoise(ev);
            }
        }
        
        //On interact with item
        public void OnUsingItem(UsingItemEventArgs ev)
        {
            //Control player item use
            foreach (XP xp in _xp)
            {
                if(xp is IUsingItemEvent)
                    ((IUsingItemEvent) xp).OnUsingItem(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IUsingItemEvent)
                    ((IUsingItemEvent) boost).OnUsingItem(ev);
            }
        }
        
        public void OnUsedItem(UsedItemEventArgs ev)
        {
            //Control player item use
            foreach (XP xp in _xp)
            {
                if(xp is IUsedItemEvent)
                    ((IUsedItemEvent) xp).OnUsedItem(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IUsedItemEvent)
                    ((IUsedItemEvent) boost).OnUsedItem(ev);
            }
        }
        
        //On finish healing
        
        
        //On use SCP-914 first time
        
        //Finish player action
        public void OnHandcuffing(HandcuffingEventArgs ev)
        {
            //Control player handcuffing
            foreach (XP xp in _xp)
            {
                if(xp is IHandcuffingEvent)
                    ((IHandcuffingEvent) xp).OnHandcuffing(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IHandcuffingEvent)
                    ((IHandcuffingEvent) boost).OnHandcuffing(ev);
            }
        }
        
        //On player level up
        public void OnLevelUp(Player player)
        {
            Boost newBoost = null;
            int tries = 0;
            while (tries++< 20)
            {
                newBoost = GetBoostByLevel(PlayerLevel[player.NetId]);
                if (newBoost.AssignBoost(player))
                {
                    //newBoost.ApplyBoost(player);
                    break;
                }
            }
            
            //Control player level up
            foreach (XP xp in _xp)
            {
                if(xp is ILevelUpEvent)
                    ((ILevelUpEvent) xp).OnLevelUp(player);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is ILevelUpEvent)
                    ((ILevelUpEvent) boost).OnLevelUp(player);
            }
            
            if(newBoost != null)
                player.ShowHUDHint("You have leveled up!\n" +
                                    "You got a new boost: "+newBoost.GetColouredName()+"\n"+
                                    newBoost.GetDescription(), 5f);
            else
            {
                player.ShowHUDHint("There was an error getting your boost. Please tell your local Taylar.", 5f);
            }
        }

        public Boost GetBoostByLevel(int playerLevel)
        {
            Tier selectedTier = Tier.Common;
            
            if(playerLevel >= 7)
                selectedTier = Tier.Legendary;
            else if(playerLevel >= 5)
                selectedTier = Tier.Epic;
            else if(playerLevel >= 4)
                selectedTier = Tier.Rare;
            else if(playerLevel >= 2)
                selectedTier = Tier.Uncommon;
            else if(playerLevel >= 1)
                selectedTier = Tier.Common;

            List<Boost> sortedBoosts = _boosts.Where(b => b.Tier <= selectedTier).ToList();
            
            int rand = UnityEngine.Random.Range(0, sortedBoosts.Count);
            return sortedBoosts[rand];
        }

        public void OnScp914UpgradingPlayer(UpgradingPlayerEventArgs ev)
        {
            foreach (XP xp in _xp)
            {
                if(xp is IUpgradingPlayerEvent)
                    ((IUpgradingPlayerEvent) xp).OnScp914UpgradingPlayer(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IUpgradingPlayerEvent)
                    ((IUpgradingPlayerEvent) boost).OnScp914UpgradingPlayer(ev);
            }
        }

        public void OnGameTick()
        {
            foreach (XP xp in _xp)
            {
                try
                {
                    if(xp is IGameTickEvent)
                        ((IGameTickEvent) xp).OnGameTick();
                } catch (System.Exception e)
                {
                    Log.Error("Error in LevelUpHandler XP Update: "+e);
                }
            }
            
            foreach (Boost boost in _boosts)
            {
                try
                {
                    if(boost is IGameTickEvent)
                        ((IGameTickEvent) boost).OnGameTick();
                } catch (System.Exception e)
                {
                    Log.Error("Error in LevelUpHandler Boost Update: "+e);
                }
            }

            foreach (Player player in Player.List.ToList())
            {
                try
                {
                    if(player.IsNPC) continue;
                    if(player.IsDead) continue;
                    if (player.IsScp)
                    {
                        TTCore.HUDs.HUD.RemoveHUD(player);
                        continue;
                    }
                    
                    if (!player.TryGetHUD(out TTCore.HUDs.HUD hud))
                    {
                        hud = TTCore.HUDs.HUD.SetupHUD(player, new LevelUpHUDLayout(hud));
                    }
                    if (hud == null) continue;
                    LevelUpHUDLayout layout = hud.GetLayout<LevelUpHUDLayout>();
                    if (layout == null)
                    {
                        hud.SetLayout(new LevelUpHUDLayout(hud));
                        layout = hud.GetLayout<LevelUpHUDLayout>();
                    }
                    
                    layout.XP = (float)Math.Round(PlayerXP[player.NetId],1);
                    layout.Level = PlayerLevel[player.NetId];
                    layout.XPNeeded = XP.GetXPNeeded(layout.Level);
                    layout.ActivePerks = string.Join("\n", _boosts.Where(b => b.HasBoost.ContainsKey(player.NetId)).Select(b => b.GetColouredName()));
                    
                } catch (System.Exception e)
                {
                    Log.Error("Error in LevelUpHandler HUD Update: "+e);
                }
            }
            
        }
        
        //On player spawn
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if(ev.Player.Role.Team == Team.SCPs) return; //Patches Zombie Coke
            if(ev.Player.Role.Team == Team.Dead) return; //Patches visual status effects as spectator
            //Control player spawn
            MEC.Timing.CallDelayed(1f, () =>
            {
                Log.Info("Checking for Boosts in LvLUp");
                foreach (Boost boost in _boosts)
                {
                    if (boost.HasBoost.ContainsKey(ev.Player.NetId))
                    {
                        boost.ApplyBoost(ev.Player);
                    }

                }
            });

        }

        public void OnRoundStart()
        {
            MEC.Timing.RunCoroutine(Tick(), "LevelUpTick");
            foreach (Player player in Player.List)
            {
                PlayerXP[player.NetId] = 0;
                PlayerLevel[player.NetId] = 1;
            }
        }
        
        public void OnPlayerJoin(JoinedEventArgs ev)
        {
            if(PlayerXP.ContainsKey(ev.Player.NetId) == false)
                PlayerXP[ev.Player.NetId] = 0;
            if(PlayerLevel.ContainsKey(ev.Player.NetId) == false)
                PlayerLevel[ev.Player.NetId] = 1;
        }
        
        public IEnumerator<float> Tick()
        {
            while (true)
            {
                yield return MEC.Timing.WaitForSeconds(1f);
                OnGameTick();
            }
        }

        public void OnRoundRestart()
        {
            MEC.Timing.KillCoroutines("LevelUpTick");
            PlayerXP.Clear();
            PlayerLevel.Clear();
        }
        
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Player.Hurting += OnHurting;
            Exiled.Events.Handlers.Player.Hurt += OnHurt;
            Exiled.Events.Handlers.Player.Died += OnDied;
            Exiled.Events.Handlers.Player.Dying += OnDying;
            Exiled.Events.Handlers.Player.Escaping += OnEscaping;
            Exiled.Events.Handlers.Player.EscapingPocketDimension += OnEscapingPocketDimension;
            Exiled.Events.Handlers.Player.MakingNoise += OnMakingNoise;
            Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
            Exiled.Events.Handlers.Player.UsedItem += OnUsedItem;
            Exiled.Events.Handlers.Player.Handcuffing += OnHandcuffing;
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestart;
            Exiled.Events.Handlers.Player.Joined += OnPlayerJoin;
            Exiled.Events.Handlers.Scp914.UpgradingPlayer += OnScp914UpgradingPlayer;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Player.Hurting -= OnHurting;
            Exiled.Events.Handlers.Player.Hurt -= OnHurt;
            Exiled.Events.Handlers.Player.Died -= OnDied;
            Exiled.Events.Handlers.Player.Dying -= OnDying;
            Exiled.Events.Handlers.Player.Escaping -= OnEscaping;
            Exiled.Events.Handlers.Player.EscapingPocketDimension -= OnEscapingPocketDimension;
            Exiled.Events.Handlers.Player.MakingNoise -= OnMakingNoise;
            Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
            Exiled.Events.Handlers.Player.UsedItem -= OnUsedItem;
            Exiled.Events.Handlers.Player.Handcuffing -= OnHandcuffing;
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
            Exiled.Events.Handlers.Player.Joined -= OnPlayerJoin;
            Exiled.Events.Handlers.Scp914.UpgradingPlayer -= OnScp914UpgradingPlayer;
            MEC.Timing.KillCoroutines("LevelUpTick");
            PlayerXP.Clear();
            PlayerLevel.Clear();
            
            foreach(XP xp in _xp)
                xp.Reset();
            foreach(Boost boost in _boosts)
                boost.Reset();
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "LevelUp",
            FormattedName = "<color=#FFD700>Level Up</color>",
            Aliases = new []{"lvlup"},
            Description = "Players can level up and gain boosts.",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false
        };
    }
}