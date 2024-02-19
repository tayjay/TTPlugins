using System.Collections.Generic;
using Exiled.API.Enums;
using MEC;

namespace RoundModifiers.Modifiers.RogueAI.Abilities
{
    public abstract class Ability
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public Side HelpingSide { get; set; } 
        
        public int AggressionLevel { get; set; }
        public bool IsEnabled { get; set; }
        public int Lifetime { get; set; }

        private CoroutineHandle _abilityCoroutine;
        
        
        public Ability(string name, string description, Side helpingSide, int aggressionLevel, int lifetime = 10)
        {
            Name = name;
            Description = description;
            HelpingSide = helpingSide;
            AggressionLevel = aggressionLevel;
            Lifetime = lifetime;
        }

        public abstract bool Setup();
        
        public virtual void Start()
        {
            //Log.Info("Starting ability " + Name);
            IsEnabled = true;
            _abilityCoroutine = Timing.RunCoroutine(AbilityCoroutine());
        }

        /**
         * Returns true if the ability is still active.
         */
        public abstract bool Update();
        
        public virtual void End()
        {
            IsEnabled = false;
            Timing.KillCoroutines(_abilityCoroutine);
        }

        public IEnumerator<float> AbilityCoroutine()
        {
            int ticks = Lifetime;
            while (Update() && ticks-- > 0 && IsEnabled)
            {
                yield return Timing.WaitForSeconds(1f);
            }
            End();
        }
    }
}