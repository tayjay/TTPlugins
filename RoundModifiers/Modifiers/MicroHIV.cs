using System.ComponentModel;
using Exiled.Events.EventArgs.Player;
using InventorySystem.Items.MicroHID;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers
{
    public class MicroHIV : Modifier
    {
        public void OnUsingHID(UsingMicroHIDEnergyEventArgs ev)
        {
            
            //Get state
            HidState state = ev.CurrentState;
            float healthDrain = 0;
            if (state == HidState.PoweringUp || state == HidState.Primed)
            {
                healthDrain = ev.Drain*ChargeDrain;
                ev.Player.ShowHint("You feel drained...", 3f);
            }

            if (state == HidState.Firing)
            {
                healthDrain = ev.Drain*FireDrain;
            }

            ev.Drain = 0;
            ev.Player.Hurt(healthDrain, "Sucked dry by the MicroHID");
            
        }

        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Player.UsingMicroHIDEnergy += OnUsingHID;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Player.UsingMicroHIDEnergy -= OnUsingHID;
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "MicroHIV",
            FormattedName = "<color=red>MicroH-IV</color>",
            Aliases = new []{"microhid", "hid"},
            Description = "Using the MicroHID drains your health.",
            Impact = ImpactLevel.MinorGameplay,
            MustPreload = false,
            Balance = 2,
            Category = Category.Utility
        };
        
        public Config MicroHIVConfig => RoundModifiers.Instance.Config.MicroHIV;
        public float ChargeDrain => MicroHIVConfig.ChargeDrain;
        public float FireDrain => MicroHIVConfig.FireDrain;
        
        public class Config : ModConfig
        {
            [Description("The amount of health drained when Charging the MicroHID. Default is 0.75f.")]
            public float ChargeDrain { get; set; } = 0.75f;
            [Description("The amount of health drained when Firing the MicroHID. Default is 3.5f.")]
            public float FireDrain { get; set; } = 3.5f;
        }
    }
}