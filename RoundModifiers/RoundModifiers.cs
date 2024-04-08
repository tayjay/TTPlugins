using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using RoundModifiers.API;
using RoundModifiers.Handlers;
using RoundModifiers.Modifiers;
using RoundModifiers.Modifiers.LevelUp;
using RoundModifiers.Modifiers.RogueAI;

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
        }
        
        public override void OnDisabled()
        {
            base.OnDisabled();
            ResetModifiers();
            RoundManager.Unregister();
            RoundManager = null;
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
            AddModifier(new MicroHIV());
            AddModifier(new NoScp914());
            AddModifier(new NoKOS());
            AddModifier(new RadioSilent());
            AddModifier(new FriendlyFire());
            //AddModifier(new Medic());
            AddModifier(new BoneZone());
            AddModifier(new NoCassie());
            //AddModifier(new Fun());
            //AddModifier(new BodyBlock());
            //AddModifier(new Alive());
            AddModifier(new MysteryBox());
            AddModifier(new HealthBars());
            //AddModifier(new MoonGravity());
            AddModifier(new WeaponStats());
            AddModifier(new AntiCamping());
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
            List<ModInfo> mods = new List<ModInfo>();
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

        public override string Author { get; } = "TayTay";
        public override string Name { get; } = "RoundModifiers";
        public override System.Version Version { get; } = new System.Version(0, 2, 0);
    }
}