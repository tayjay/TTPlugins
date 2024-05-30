using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using MEC;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers
{
    public class NoDecontamination : Modifier
    {
        CoroutineHandle _coroutine;
        public static bool FakeDecon = false;
        public float DamageMultiplier = 1f;
        
        public void OnRoundStart()
        {
            FakeDecon = false;
            DamageMultiplier = RoundModifiers.Instance.Config.NoDecon_DamageMultiplierMin;
        }

        public void OnRoundRestart()
        {
            FakeDecon = false;
            DamageMultiplier = 1f;
        }

        public void OnDecontamination(DecontaminatingEventArgs ev)
        {
            ev.IsAllowed = false;
            _coroutine = Timing.RunCoroutine(DeconTick());
        }

        public IEnumerator<float> DeconTick()
        {
            int count = 0;
            while (true)
            {
                
                foreach (Player p in Player.List)
                {
                    if (p.Zone == ZoneType.LightContainment)
                    {
                        p.Hurt(DamageMultiplier* (p.MaxHealth/100), DamageType.Decontamination);
                    }
                }
                
                if(DamageMultiplier < RoundModifiers.Instance.Config.NoDecon_DamageMultiplierMax)
                    DamageMultiplier += 0.05f;
                yield return Timing.WaitForSeconds(3f);
            }
        }
        
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Map.Decontaminating += OnDecontamination;
            Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestart;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Map.Decontaminating -= OnDecontamination;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
            Timing.KillCoroutines(_coroutine);
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "NoDecontamination",
            FormattedName = "<color=yellow>No Decontamination</color>",
            Aliases = new []{"nodecon", "decon"},
            Description = "Disables decontamination and replaces it with a slow damaging effect.",
            Impact = ImpactLevel.MinorGameplay,
            MustPreload = false,
            Balance = -1
        };
    }
}