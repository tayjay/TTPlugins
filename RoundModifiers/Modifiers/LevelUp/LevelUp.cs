using System;
using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp0492;
using Exiled.Events.EventArgs.Scp096;
using Exiled.Events.EventArgs.Scp173;
using Exiled.Events.EventArgs.Scp914;
using Exiled.Events.EventArgs.Scp939;
using Exiled.Events.Features;
using HarmonyLib;
using InventorySystem.Items.Usables.Scp330;
using MEC;
using PlayerRoles;
using RoundModifiers.API;
using RoundModifiers.Modifiers.LevelUp.Boosts;
using RoundModifiers.Modifiers.LevelUp.Boosts.Scp;
using RoundModifiers.Modifiers.LevelUp.HUD;
using RoundModifiers.Modifiers.LevelUp.Interfaces;
using RoundModifiers.Modifiers.LevelUp.XPs;
using RoundModifiers.Modifiers.LevelUp.XPs.Scp;
using TTCore.Events.EventArgs;
using TTCore.Events.Handlers;
using TTCore.HUDs;

namespace RoundModifiers.Modifiers.LevelUp
{
    public class LevelUp : Modifier
    {
        //Store XP for players
        /*public Dictionary<uint,float> PlayerXP { get; set; }
        public Dictionary<uint, int> PlayerLevel { get; set; }*/
        //public Dictionary<uint, List<string>> PlayerBoostList { get; set; }
        
        //public Dictionary<string, List<XP>> XpByType { get; set; }
        //public Dictionary<string, List<Boost>> BoostsByType { get; set; }
        
        public List<Boost> _boosts { get; set; }
        public List<XP> _xp { get; set; }
        
        public CoroutineHandle TickHandle { get; set; }
        
        public Dictionary<Player, string> LastBoost { get; set; } = DictionaryPool<Player, string>.Pool.Get();

        public LevelUp()
        {
            //Setup();
        }

        public void Setup()
        {
            /*PlayerXP = DictionaryPool<uint, float>.Pool.Get();
            PlayerLevel = DictionaryPool<uint, int>.Pool.Get();*/
            //PlayerBoostList = DictionaryPool<uint, List<string>>.Pool.Get();
            
            _boosts = ListPool<Boost>.Pool.Get();
            _xp = ListPool<XP>.Pool.Get();

            //XpByType = DictionaryPool<string, List<XP>>.Pool.Get();
            //BoostsByType = DictionaryPool<string, List<Boost>>.Pool.Get();
            
            //XP
            _xp.Add(new EscapeXP());
            _xp.Add(new DealDamageXP());
            _xp.Add(new ExploreZonesXP());
            _xp.Add(new Scp914UpgradingPlayerXP());
            _xp.Add(new EscapePocketDimensionXP());
            //_xp.Add(new DeathXP()); Review this as leveling up on death will give nothing.
            _xp.Add(new AliveXP());
            _xp.Add(new HandcuffXP());
            _xp.Add(new UsedItemXP());
            _xp.Add(new NearScpXP());
            _xp.Add(new RespawnXP());
            //SCP XPs
            _xp.Add(new KillXP());
            _xp.Add(new Scp049XP());
            _xp.Add(new Scp0492XP());
            _xp.Add(new Scp096XP());
            _xp.Add(new Scp106XP());
            _xp.Add(new Scp173XP());
            _xp.Add(new Scp939XP());
            
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
            _boosts.Add(new StatusEffectBoost(EffectType.SilentWalk, 1, true, Tier.Legendary,false));
            _boosts.Add(new StatusEffectBoost(EffectType.Scp207, 1, true, Tier.Rare));
            _boosts.Add(new StatusEffectBoost(EffectType.AntiScp207, 1, true, Tier.Rare, false));
            _boosts.Add(new UpgradeKeycardBoost());
            _boosts.Add(new UpgradeKeycardBoost());
            _boosts.Add(new UpgradeKeycardBoost());
            _boosts.Add(new LightFootedBoost());
            //_boosts.Add(new NoInstaKillBoost(Tier.Epic));
            _boosts.Add(new HealAOEBoost(Tier.Epic));
            _boosts.Add(new BecomeScpBoost(Tier.Legendary));
            //_boosts.Add(new UpgradeWeaponBoost(Tier.Common));
            //_boosts.Add(new UpgradeWeaponBoost(Tier.Uncommon));
            _boosts.Add(new ChangeSizeBoost());
            _boosts.Add(new ChangeSizeBoost(0.8f,Tier.Rare));
            _boosts.Add(new ChangeSizeBoost(1.2f,Tier.Rare));
            //SCP Boosts
            _boosts.Add(new HealBoost(Tier.Common));
            _boosts.Add(new HealBoost(Tier.Uncommon));
            _boosts.Add(new HealBoost(Tier.Rare));
            _boosts.Add(new HealBoost(Tier.Epic));
            _boosts.Add(new HealBoost(Tier.Legendary));
            _boosts.Add(new AppearHumanBoost());
            _boosts.Add(new BlackoutBoost());
        }
        
        public void Shutdown()
        {
            /*DictionaryPool<uint, float>.Pool.Return(PlayerXP);
            DictionaryPool<uint, int>.Pool.Return(PlayerLevel);*/
            ListPool<Boost>.Pool.Return(_boosts);
            ListPool<XP>.Pool.Return(_xp);
            DictionaryPool<Player, string>.Pool.Return(LastBoost);
        }
        
        /*public void RegisterXP<T>(T xp) where T : XP
        {
            foreach(Type type in xp.GetType().GetInterfaces())
            {
                if (XpByType.ContainsKey(type.Name) == false)
                    XpByType[type.Name] = ListPool<XP>.Pool.Get();
                XpByType[type.Name].Add(xp);
            }
        }
        
        public List<XP> GetXP<T>()
        {
            return XpByType[typeof(T).Name];
        }*/
        
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
            /*foreach(XP xp in GetXP<IHurtingEvent>())
                ((IHurtingEvent) xp).OnHurting(ev);*/

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

        public void OnEnterRoom(EnterRoomEventArgs ev)
        {
            foreach(XP xp in _xp.Where(x=>x is IRoomEvent))
                ((IRoomEvent) xp).OnEnterRoom(ev);
            
            foreach(Boost boost in _boosts.Where(x=>x is IRoomEvent))
                ((IRoomEvent) boost).OnEnterRoom(ev);
        }
        
        public void OnExitRoom(ExitRoomEventArgs ev)
        {
            foreach(XP xp in _xp.Where(x=>x is IRoomEvent))
                ((IRoomEvent) xp).OnExitRoom(ev);
            
            foreach(Boost boost in _boosts.Where(x=>x is IRoomEvent))
                ((IRoomEvent) boost).OnExitRoom(ev);
        }

        public void OnPressKeybind(TogglingNoClipEventArgs ev)
        {
            foreach(XP xp in _xp.Where(x=>x is IKeybindAbility))
                ((IKeybindAbility) xp).OnPressKeybind(ev);
            
            foreach(Boost boost in _boosts.Where(x=>x is IKeybindAbility))
                ((IKeybindAbility) boost).OnPressKeybind(ev);
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
            while (tries++< 30)
            {
                newBoost = GetBoostByLevel((int)player.SessionVariables["levelup_level"]);
                if (newBoost.AssignBoost(player))
                {
                    //newBoost.ApplyBoost(player);
                    Log.Debug("Player "+player.Nickname+" got boost "+newBoost.GetName());
                    
                    break;
                }
                else
                {
                    newBoost = null;
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

            if (newBoost != null)
            {
                player.ShowHUDHint("You have leveled up!\n" +
                                   "You got a new boost: "+newBoost.GetColouredName()+"\n"+
                                   newBoost.GetDescription(), 5f);
                LastBoost[player] = newBoost.GetColouredName();
                if (!player.SessionVariables.ContainsKey("levelup_boosts"))
                    player.SessionVariables["levelup_boosts"] = ListPool<string>.Pool.Get();
                ((List<string>)player.SessionVariables["levelup_boosts"]).Add($"Level {(int)player.SessionVariables["levelup_level"]}: {newBoost.GetName()}");
            }
            else
            {
                player.ShowHUDHint("There was an error getting your boost. Please tell your local Taylar.", 5f);
                player.SessionVariables["levelup_xp"] = (float) player.SessionVariables["levelup_xp"] + XP.GetXPNeeded((int)player.SessionVariables["levelup_level"]-1);
                //PlayerXP[player.NetId] += XP.GetXPNeeded(PlayerLevel[player.NetId]-1);
            }
        }
        
        //SCPs
        
        public void OnEnteringPocketDimension(EnteringPocketDimensionEventArgs ev)
        {
            //Control player escape
            foreach (XP xp in _xp)
            {
                if(xp is IPocketDimensionEvent)
                    ((IPocketDimensionEvent) xp).OnEnteringPocketDimension(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IPocketDimensionEvent)
                    ((IPocketDimensionEvent) boost).OnEnteringPocketDimension(ev);
            }
        }

        public void OnBlinkRequest(BlinkingRequestEventArgs ev)
        {
            foreach (XP xp in _xp)
            {
                if(xp is IBlinkEvent)
                    ((IBlinkEvent) xp).OnBlinkingRequest(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IBlinkEvent)
                    ((IBlinkEvent) boost).OnBlinkingRequest(ev);
            }
        }
        
        public void OnConsumedCorpse(ConsumedCorpseEventArgs ev)
        {
            foreach (XP xp in _xp)
            {
                if(xp is IConsumeEvent)
                    ((IConsumeEvent) xp).OnConsumedCorpse(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IConsumeEvent)
                    ((IConsumeEvent) boost).OnConsumedCorpse(ev);
            }
        }
        
        public void OnEnraging(EnragingEventArgs ev)
        {
            foreach (XP xp in _xp)
            {
                if(xp is IEnragingEvent)
                    ((IEnragingEvent) xp).OnEnraging(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IEnragingEvent)
                    ((IEnragingEvent) boost).OnEnraging(ev);
            }
        }
        
        public void OnSavingVoice(SavingVoiceEventArgs ev)
        {
            foreach (XP xp in _xp)
            {
                if(xp is IVoiceEvent)
                    ((IVoiceEvent) xp).OnSavingVoice(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IVoiceEvent)
                    ((IVoiceEvent) boost).OnSavingVoice(ev);
            }
        }
        
        public void OnPlayingSound(PlayingSoundEventArgs ev)
        {
            foreach (XP xp in _xp)
            {
                if(xp is IVoiceEvent)
                    ((IVoiceEvent) xp).OnPlayingSound(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IVoiceEvent)
                    ((IVoiceEvent) boost).OnPlayingSound(ev);
            }
        }
        
        public void OnPlayingVoice(PlayingVoiceEventArgs ev)
        {
            foreach (XP xp in _xp)
            {
                if(xp is IVoiceEvent)
                    ((IVoiceEvent) xp).OnPlayingVoice(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IVoiceEvent)
                    ((IVoiceEvent) boost).OnPlayingVoice(ev);
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

        public void OnScp914UpgradingPickup(UpgradingPickupEventArgs ev)
        {
            foreach (XP xp in _xp)
            {
                if(xp is IUpgradingPickupEvent)
                    ((IUpgradingPickupEvent) xp).OnScp914UpgradingPickup(ev);
            }
            
            foreach (Boost boost in _boosts)
            {
                if(boost is IUpgradingPickupEvent)
                    ((IUpgradingPickupEvent) boost).OnScp914UpgradingPickup(ev);
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
                    if (player.Role==RoleTypeId.Scp079)
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
                    
                    layout.XP = (float)Math.Round((float)player.SessionVariables["levelup_xp"],1);
                    layout.Level = (int)player.SessionVariables["levelup_level"];
                    layout.XPNeeded = XP.GetXPNeeded(layout.Level);
                    layout.ActivePerks = string.Join("\n", _boosts.Where(b => b.HasBoost.ContainsKey(player.NetId)).Select(b => b.GetColouredName()));
                    layout.LastPerk = LastBoost.TryGetValue(player, out var value) ? value : "None";
                } catch (System.Exception e)
                {
                    Log.Error("Error in LevelUpHandler HUD Update: "+e);
                }
            }
            
        }
        
        //On player spawn
        public void OnSpawned(SpawnedEventArgs ev)
        {
            //if(ev.Player.Role.Team == Team.SCPs) return; //Patches Zombie Coke
            if(ev.Player.Role.Team == Team.Dead) return; //Patches visual status effects as spectator
            if(ev.Player.SessionVariables.ContainsKey("levelup_xp") == false)
                ev.Player.SessionVariables.Add("levelup_xp", 0f);
            if(ev.Player.SessionVariables.ContainsKey("levelup_level") == false)
                ev.Player.SessionVariables.Add("levelup_level", (int)1);
            if(!ev.Player.SessionVariables.ContainsKey("levelup_boosts"))
                ev.Player.SessionVariables["levelup_boosts"] = ListPool<string>.Pool.Get();
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
            Timing.CallDelayed(2f, () =>
            {
                foreach(XP xp in _xp.Where(x=>x is ISpawnedEvent))
                    ((ISpawnedEvent) xp).OnSpawned(ev);
            });

        }

        public void OnRevive(ChangingRoleEventArgs ev)
        {
            if(ev.Reason != SpawnReason.Revived) return;
            if(ev.NewRole != RoleTypeId.Scp0492) return;
            foreach(XP xp in _xp.Where(x=>x is IResurrectEvent))
                ((IResurrectEvent) xp).OnResurrect(ev);
                
        }

        public void OnRoundStart()
        {
            TickHandle = MEC.Timing.RunCoroutine(Tick());
            /*foreach (Player player in Player.List)
            {
                player.SessionVariables.Add("levelup_xp", 0f);
                player.SessionVariables.Add("levelup_level", 1);
                player.SessionVariables.Add("levelup_boosts",ListPool<string>.Pool.Get());
            }*/
        }
        
        public void OnPlayerJoin(JoinedEventArgs ev)
        {
            if(ev.Player.SessionVariables.ContainsKey("levelup_xp") == false)
                ev.Player.SessionVariables.Add("levelup_xp", 0f);
            if(ev.Player.SessionVariables.ContainsKey("levelup_level") == false)
                ev.Player.SessionVariables.Add("levelup_level", 1);
            if(!ev.Player.SessionVariables.ContainsKey("levelup_boosts"))
                ev.Player.SessionVariables["levelup_boosts"] = ListPool<string>.Pool.Get();
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
            /*PlayerXP.Clear();
            PlayerLevel.Clear();*/
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
            Exiled.Events.Handlers.Scp914.UpgradingPickup += OnScp914UpgradingPickup;
            Exiled.Events.Handlers.Player.ChangingRole += OnRevive;
            Exiled.Events.Handlers.Player.TogglingNoClip += OnPressKeybind;
            RoomTrigger.EnterRoom += OnEnterRoom;
            RoomTrigger.ExitRoom += OnExitRoom;
            
            //SCP
            Exiled.Events.Handlers.Player.EnteringPocketDimension += OnEnteringPocketDimension;
            Exiled.Events.Handlers.Scp173.BlinkingRequest += OnBlinkRequest;
            Exiled.Events.Handlers.Scp0492.ConsumedCorpse += OnConsumedCorpse;
            Exiled.Events.Handlers.Scp096.Enraging += OnEnraging;
            Exiled.Events.Handlers.Scp939.SavingVoice += OnSavingVoice;
            Exiled.Events.Handlers.Scp939.PlayingSound += OnPlayingSound;
            Exiled.Events.Handlers.Scp939.PlayingVoice += OnPlayingVoice;
            
            Setup();
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
            Exiled.Events.Handlers.Scp914.UpgradingPickup -= OnScp914UpgradingPickup;
            Exiled.Events.Handlers.Player.ChangingRole -= OnRevive;
            Exiled.Events.Handlers.Player.TogglingNoClip -= OnPressKeybind;
            MEC.Timing.KillCoroutines(TickHandle);
            /*Traverse.Create(Exiled.Events.Handlers.Player.Escaping).Field("InnerEvent").GetValue<CustomEventHandler>()
                .GetInvocationList();*/
            RoomTrigger.EnterRoom -= OnEnterRoom;
            RoomTrigger.ExitRoom -= OnExitRoom;
            
            
            //SCP
            Exiled.Events.Handlers.Player.EnteringPocketDimension -= OnEnteringPocketDimension;
            Exiled.Events.Handlers.Scp173.BlinkingRequest -= OnBlinkRequest;
            Exiled.Events.Handlers.Scp0492.ConsumedCorpse -= OnConsumedCorpse;
            Exiled.Events.Handlers.Scp096.Enraging -= OnEnraging;
            Exiled.Events.Handlers.Scp939.SavingVoice -= OnSavingVoice;
            Exiled.Events.Handlers.Scp939.PlayingSound -= OnPlayingSound;
            Exiled.Events.Handlers.Scp939.PlayingVoice -= OnPlayingVoice;
            
            foreach(XP xp in _xp)
                xp.Reset();
            foreach(Boost boost in _boosts)
                boost.Reset();
            
            Shutdown();
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "LevelUp",
            FormattedName = "<color=#FFD700>Level Up</color>",
            Aliases = new []{"lvlup"},
            Description = "Players can level up and gain boosts.",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false,
            Balance = 0,
            Category = Category.Overhaul | Category.HUD | Category.Effect
        };
        
        public static LevelUpConfig Config => RoundModifiers.Instance.Config.LevelUp;
        
        public static float BaseXP => Config.BaseXP;
        public static float XPPerLevel => Config.XPPerLevel;
    }
}