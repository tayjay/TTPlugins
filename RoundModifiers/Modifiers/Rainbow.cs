using System.Collections.Generic;
using System.ComponentModel;
using MEC;
using RoundModifiers.API;
using UnityEngine;

namespace RoundModifiers.Modifiers
{
    public class Rainbow : Modifier
    {
        
        CoroutineHandle Coroutine;
        private Dictionary<RoomLightController, Color> RoomColors;
        private float LastChange;

        public float Speed => RainbowConfig.Speed;
        public float ColorChangeTime => RainbowConfig.ColorChangeTime;
        public float MinRGB => RainbowConfig.MinRGB;
        public float MaxRGB => RainbowConfig.MaxRGB;
        
        public void OnRoundStart()
        {
            RoomColors = new Dictionary<RoomLightController, Color>();
            foreach (RoomLightController instance in RoomLightController.Instances)
            {
                //Get a random color
                Color color = new Color(UnityEngine.Random.Range(MinRGB, MaxRGB), UnityEngine.Random.Range(MinRGB, MaxRGB),
                    UnityEngine.Random.Range(MinRGB, MaxRGB));
                instance.NetworkOverrideColor = color;
                RoomColors[instance] = color;
            }
            LastChange = Time.time;
            Coroutine = Timing.RunCoroutine(RainbowTick());
        }
        
        
        public IEnumerator<float> RainbowTick()
        {
            while (true)
            {
                yield return Timing.WaitForOneFrame;
                foreach (RoomLightController instance in RoomLightController.Instances)
                {
                    if(Time.time - LastChange > ColorChangeTime)
                    {
                        //Get a random color
                        RoomColors[instance] = new Color(UnityEngine.Random.Range(MinRGB, MaxRGB), UnityEngine.Random.Range(MinRGB, MaxRGB),
                            UnityEngine.Random.Range(MinRGB, MaxRGB));
                    }
                    Color currentColor = instance.NetworkOverrideColor;
                    Color targetColor = RoomColors[instance];
                    
                    //Transition to the target color slowly
                    //Create a new color that is a percentage of the way between the current color and the target color
                    Color newColor = new Color(
                        Mathf.Lerp(currentColor.r, targetColor.r, Speed),
                        Mathf.Lerp(currentColor.g, targetColor.g, Speed),
                        Mathf.Lerp(currentColor.b, targetColor.b, Speed)
                    );
                    instance.NetworkOverrideColor = newColor;
                }
                if(Time.time - LastChange > ColorChangeTime)
                {
                    LastChange = Time.time;
                }
            }
        }
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        }
        
        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "Rainbow",
            FormattedName = "<color=red>R</color><color=orange>a</color><color=yellow>i</color><color=green>n</color><color=blue>b</color><color=purple>o</color><color=red>w</color>",
            Aliases = new []{"colour"},
            Description = "The lights are changing colors!",
            Impact = ImpactLevel.Graphics,
            MustPreload = false,
            Balance = 0,
            Category = Category.Visual | Category.Lights
        };

        public static Config RainbowConfig => RoundModifiers.Instance.Config.Rainbow;

        public class Config : ModConfig
        {
            [Description("The speed of the rainbow effect. Default is 0.01f")]
            public float Speed { get; set; } = 0.01f;
            [Description("How long a color will stay before changing. Default is 3f.")]
            public float ColorChangeTime { get; set; } = 3f;
            [Description("The minimum and maximum RGB values for the rainbow effect. Default is 0.1f and 2f")]
            public float MinRGB { get; set; } = 0.1f;
            public float MaxRGB { get; set; } = 2f;
        }
    }
}