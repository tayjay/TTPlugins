using System.ComponentModel;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using Respawning;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers
{
    public class Insurrection : Modifier
    {
        
        public void OnSpawned(SpawnedEventArgs ev)
        {
            
            if (ev.Player.Role == RoleTypeId.FacilityGuard)
            {
                Log.Debug("Setting " + ev.Player.Nickname + " to Chaos.");
                ev.Player.RoleManager.ServerSetRole(RoleTypeId.ChaosConscript, RoleChangeReason.RoundStart, RoleSpawnFlags.None);
                Respawn.GrantTickets(SpawnableTeamType.ChaosInsurgency, 10);
            }
            
        }
        
        public void OnRoundStart()
        {
            MEC.Timing.CallDelayed(1f, () =>
            {
                Cassie.Message(
                    "Insurgency Threat detected in .G6 entrance zone .g1",
                    false, true, true);
            });
        }
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "Insurrection",
            FormattedName = "<color=green>Insurrection</color>",
            Aliases = new []{"ins"},
            Description = "Facility Guards spawn as Chaos Insurgents.",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false,
            Balance = 1,
            Category = Category.HumanRole
        };

        public Config InsurrectionConfig => Config.InsurrectionConfig;
        public float Tickets => InsurrectionConfig.Tickets;
        
        public class Config : ModConfig
        {
            public static Config InsurrectionConfig => RoundModifiers.Instance.Config.Insurrection;
            [Description("The amount of tickets to grant to the Chaos Insurgency when a guard is replaced. Default is 10.")]
            public float Tickets { get; set; } = 10;
        }
    }
}