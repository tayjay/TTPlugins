using Exiled.API.Enums;
using Exiled.API.Features;
using UnityEngine;

namespace RoundModifiers.Modifiers.RogueAI.Abilities
{
    public class ActivateNukeAbility : Ability
    {
        public ActivateNukeAbility(Side helpingSide, int lifetime = 30) : base("Activate Warhead", "", helpingSide, 10, lifetime)
        {
        }

        public override bool Setup()
        {
            if(Warhead.IsInProgress)
            {
                return true;
            }
            if(Random.Range(0,100)<10)
                return true;
            return false;
        }

        public override void Start()
        {
            base.Start();
            if(Warhead.IsInProgress)
            {
                Warhead.Stop();
            }
            else
            {
                Warhead.Start();
            }
            
        }

        public override void End()
        {
            base.End();
            Warhead.Stop();
        }

        public override bool Update()
        {
            return true;
        }
    }
}