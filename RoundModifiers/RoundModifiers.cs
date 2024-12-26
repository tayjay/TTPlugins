using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.CustomRoles.API.Features;
using Exiled.Events.Commands.PluginManager;
using Exiled.Events.Commands.Reload;
using Exiled.Loader;
using RoundModifiers.API;
using RoundModifiers.Handlers;
using RoundModifiers.Modifiers;
using RoundModifiers.Modifiers.GunGame;
using RoundModifiers.Modifiers.Scp1507;
using RoundModifiers.Modifiers.LevelUp;
using RoundModifiers.Modifiers.Nicknames;
using RoundModifiers.Modifiers.RogueAI;
using RoundModifiers.Modifiers.Scp507;
using RoundModifiers.Modifiers.WeaponStats;
using TTCore.Reflection;
using TTCore.Handlers;

namespace RoundModifiers
{
    public class RoundModifiers : Plugin<Config>
    {
        private static readonly RoundModifiers Singleton = new RoundModifiers();
        
        public static RoundModifiers Instance => Singleton;
        
        public Dictionary<ModInfo,Modifier> Modifiers { get; }
        
        public RoundManager RoundManager { get; private set; }

        private RoundModifiers()
        {
            Modifiers = new Dictionary<ModInfo, Modifier>();
            RoundManager = new RoundManager();
        }

        public override PluginPriority Priority { get; } = PluginPriority.Medium;
        
        public override void OnEnabled()
        {
            base.OnEnabled();
            SetupModifiers();
            RoundManager = new RoundManager();
            RoundManager.Register();
            CustomRole.RegisterRoles(false, null);
            Log.Debug(Nicknames.NicknameData);
            //CustomEffects.RegisterEffect<WeaponStatsEffect>();
            OptionalSCriPt optionalSCriPt = new OptionalSCriPt();
            if(optionalSCriPt.IsPresent)
            {
                optionalSCriPt.AddGlobal(typeof(LuaModifiers), "RoundModifiers");
            }
            
            /*if(OptionalPlugin.GetPlugin("SCriPt", out OptionalPlugin SCriPtConnection))
            {
                object connector = SCriPtConnection.CallMethod("SetupConnector", this);
                TTReflection.CallVoidMethod(connector, "AddGlobal", typeof(LuaModifiers), "RoundModifiers");
            }*/
        }
        
        
        
        public override void OnDisabled()
        {
            base.OnDisabled();
            ResetModifiers();
            RoundManager.Unregister();
            RoundManager = null;
            CustomRole.UnregisterRoles();
        }
        
        public override void OnReloaded()
        {
            base.OnReloaded();
            ResetModifiers();
            RoundManager.Unregister();
            RoundManager = new RoundManager();
            SetupModifiers();
            RoundManager.Register();
        }

        private void SetupModifiers()
        {
            Modifiers.Clear();
            AddModifier(new Blackout());
            AddModifier(new Rainbow());
            AddModifier(new Imposter());
            AddModifier(new RogueAI());
            AddModifier(new LevelUp());
            AddModifier(new Insurrection());
            AddModifier(new MultiBall());
            AddModifier(new NoDecontamination());
            AddModifier(new Puppies());
            AddModifier(new RandomSpawnSize());
            AddModifier(new Scp249());
            //AddModifier(new MicroHIV());
            AddModifier(new NoScp914());
            AddModifier(new NoKOS());
            AddModifier(new RadioSilent());
            AddModifier(new FriendlyFire());
            //AddModifier(new Medic());
            AddModifier(new BoneZone());
            AddModifier(new NoCassie());
            AddModifier(new MysteryBox());
            AddModifier(new HealthBars());
            AddModifier(new Pills());
            AddModifier(new Paper());
            AddModifier(new Keyless());
            AddModifier(new ExplosiveRagdolls());
            AddModifier(new WeaponStats2());
            AddModifier<Scp507>();
            AddModifier<ScpShuffle>();
            AddModifier<Peanuts>();
            AddModifier<DoNotLook>();
            AddModifier<ExtraLife>();
            //AddModifier<GunGame>();
            //AddModifier<Flamingos>();
            AddModifier<Nicknames>();
            //AddModifier<GhostHunting>();
            AddModifier<ScpChat>();
            //AddModifier<LowPower>();
            //AddModifier<ZombieSurvival>();
            //AddModifier<ScpBackup>();
            //AddModifier<ZombieSurvival>();
            //AddModifier<Debug>();
            AddModifier<PayToWin>();
            //AddModifier<BigWorld>();
            //AddModifier<BehindYou>();
            
            //AddModifier<ComboImposter>();
            
            BlacklistModifiers();
        }
        
        public void AddModifier<T>() where T : Modifier, new()
        {
            AddModifier(new T());
        }
        
        public void AddModifier(Modifier mod)
        {
            Modifiers.Add(mod.ModInfo, mod);
        }
        
        private void ResetModifiers()
        {
            foreach(Modifier mod in Modifiers.Values)
                mod.Unregister();
        }
        
        
        
        /**
         * Get a modifier by name
         * @param name The name of the modifier
         * @param modifier The modifier
         * @param exact If the name must be exact
         * @return If the modifier was found
         */
        public bool TryGetModifier(string name, out Modifier modifier, bool exact = false)
        {
            List<ModInfo> mods = ListPool<ModInfo>.Pool.Get();
            foreach (ModInfo mod in Modifiers.Keys)
            {
                if (mod.Name.ToLower() == name.ToLower() || mod.Aliases.Contains(name.ToLower()))
                {
                    modifier = Modifiers[mod];
                    return true;
                } else if (!exact && mod.Name.ToLower().StartsWith(name.ToLower()))
                {
                    mods.Add(mod);
                }
            }
            if (mods.Count == 1)
            {
                modifier = Modifiers[mods.First()];
                return true;
            }
            modifier = null;
            return false;
        }
        
        public T GetModifier<T>() where T : Modifier
        {
            foreach (Modifier mod in Modifiers.Values)
            {
                if (mod is T t)
                    return t;
            }
            return null;
        }
        
        public bool IsModifierActive(ModInfo mod)
        {
            return Modifiers[mod].IsEnabled;
        }
        
        public bool IsModifierActive(string name)
        {
            return TryGetModifier(name, out Modifier mod, exact:true) && mod.IsEnabled;
        }
        
        public bool IsModifierActive(Modifier mod)
        {
            return mod.IsEnabled;
        }

        public void BlacklistModifiers()
        {
            string[] blacklist = Config.BlacklistedModifiers;
            foreach (string mod in blacklist)
            {
                if (TryGetModifier(mod, out Modifier modifier, true))
                {
                    modifier.Unregister();
                    Modifiers.Remove(modifier.ModInfo);
                    Log.Info("Blacklisted modifier: "+modifier.ModInfo.Name);
                }
                else
                {
                    Log.Info("Could not find modifier to blacklist: "+mod);
                }
            }
        }

        public override string Author { get; } = "TayTay";
        public override string Name { get; } = "RoundModifiers";
        public override System.Version Version { get; } = new System.Version(0, 8, 0);
    }
}