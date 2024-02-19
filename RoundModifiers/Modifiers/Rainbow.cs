using System.Collections.Generic;
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

        public float Speed => RoundModifiers.Instance.Config.Rainbow_Speed;
        public float ColorChangeTime => RoundModifiers.Instance.Config.Rainbow_ColorChangeTime;
        public float MinRGB => RoundModifiers.Instance.Config.Rainbow_MinRGB;
        public float MaxRGB => RoundModifiers.Instance.Config.Rainbow_MaxRGB;
        
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
            Description = "The lights are changing colors!",
            Impact = ImpactLevel.Graphics,
            MustPreload = false
        };

        /*public override string Name { get; } = "Rainbow";
        public override string Description { get; } = "The lights are changing colors!";
        public override string[] Aliases { get; } = {"rb"};
        public override ImpactLevel Impact { get; } = ImpactLevel.Graphics;*/
    }
}