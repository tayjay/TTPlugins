using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp939;
using MEC;
using PlayerRoles;
using RoundModifiers.API;
using TTCore.Extensions;

namespace RoundModifiers.Modifiers
{
    public class Puppies : Modifier
    {
        
        public void OnChangingRole(ChangingRoleEventArgs ev)
        {
            
            if (ev.NewRole == RoleTypeId.Scp049 || (ev.NewRole == RoleTypeId.Scp079 && AffectScp079) || ev.NewRole == RoleTypeId.Scp096 ||
                ev.NewRole == RoleTypeId.Scp106 || ev.NewRole == RoleTypeId.Scp173 || (ev.NewRole == RoleTypeId.Scp3114 && AffectScp3114))
            {
                ev.NewRole = RoleTypeId.Scp939;
            }

            if (ev.NewRole == RoleTypeId.Scp939)
            {
                Timing.CallDelayed(1f, () =>
                {
                    Log.Info("Setting " + ev.Player.Nickname + " size to "+Size);
                    ev.Player.MaxHealth = HealthStart;
                    ev.Player.Health = ev.Player.MaxHealth;
                    //ev.Player.HumeShieldStat.CurValue = HumeStart;
                    ev.Player.HumeShield = HumeStart;
                    ev.Player.ChangeSize(Size);
                });
            }
            
        }

        public void OnSavingVoice(SavingVoiceEventArgs ev)
        {
            if(!ShareVoices) return;
            if(!ev.IsAllowed) return;

            if (!AlreadySavedVoices.ContainsKey(ev.Stolen.Id))
            {
                AlreadySavedVoices.Add(ev.Stolen.Id, new List<Player>());
            }
            if(AlreadySavedVoices[ev.Stolen.Id].Contains(ev.Player))
            {
                ev.IsAllowed = false;
                return;
            }
            AlreadySavedVoices[ev.Stolen.Id].Add(ev.Player);
            
            // Try giving the voice to other puppies
            foreach (Player scp939Player in Player.List.Where(p => p.Role == RoleTypeId.Scp939 && p.Id != ev.Player.Id))
            {
                Scp939Role scp939 = scp939Player.Role as Scp939Role;
                if (scp939 == null) continue;
                if (scp939.MimicryRecorder.SavedVoices
                    .Any(r => r.Owner.PlayerId == ev.Stolen.Footprint.PlayerId))
                    return;
                scp939.MimicryRecorder.SaveRecording(ev.Stolen.ReferenceHub);
            }
        }
        
        public Dictionary<int,List<Player>> AlreadySavedVoices { get; set; }
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Player.ChangingRole += OnChangingRole;
            Exiled.Events.Handlers.Scp939.SavingVoice += OnSavingVoice;

            AlreadySavedVoices = DictionaryPool<int, List<Player>>.Pool.Get();
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Player.ChangingRole -= OnChangingRole;
            Exiled.Events.Handlers.Scp939.SavingVoice -= OnSavingVoice;
            
            DictionaryPool<int, List<Player>>.Pool.Return(AlreadySavedVoices);
        }

        public override ModInfo ModInfo { get; } = new ModInfo
        {
            Name = "Puppies",
            FormattedName = "<color=red>Puppies</color>",
            Aliases = new []{"pup"},
            Description = "All SCPs are mini 939s",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false,
            Balance = -2,
            Category = Category.ScpRole | Category.Scale | Category.Health
        };
        
        public static Config PuppiesConfig => RoundModifiers.Instance.Config.Puppies;
        public static float Size => PuppiesConfig.SCPScale;
        public static float HealthStart => PuppiesConfig.Scp939HealthStart;
        public static float HumeStart => PuppiesConfig.Scp939HumeStart;
        public static bool AffectScp079 => PuppiesConfig.AffectScp079;
        public static bool AffectScp3114 => PuppiesConfig.AffectScp3114;
        public static bool ShareVoices => PuppiesConfig.ShareVoices;
        
        public class Config
        {
            [Description("The scale SCPs should spawn as during the Puppies modifier. Default is 0.5f.")]
            public float SCPScale { get; set; } = 0.65f;
            [Description("How much HP SCP939 should start with during the Puppies modifier. Default is 750f.")]
            public float Scp939HealthStart { get; set; } = 750;
            [Description("How much Hume SCP939 should start with during the Puppies modifier. Default is 100f.")]
            public float Scp939HumeStart { get; set; } = 500f;
            [Description("Whether SCP079 should be affected by the Puppies modifier. Default is true.")]
            public bool AffectScp079 { get; set; } = true;
            [Description("Whether SCP3114 should be affected by the Puppies modifier. Default is true.")]
            public bool AffectScp3114 { get; set; } = true;
            [Description("Whether SCPs should share voices during the Puppies modifier. Default is true.")]
            public bool ShareVoices { get; set; } = false;
        }
    }
}