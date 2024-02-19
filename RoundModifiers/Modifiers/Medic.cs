using RoundModifiers.API;

namespace RoundModifiers.Modifiers
{
    /*
     * Allows players to heal other players
     * When using a medkit and looking at a player, the player will be healed
     * If the player is looking at a dead body, the player will be revived if that was their most recent corpse and they are currently dead.
     */
    public class Medic : Modifier
    {
        
        
        
        
        protected override void RegisterModifier()
        {
            throw new System.NotImplementedException();
        }

        protected override void UnregisterModifier()
        {
            throw new System.NotImplementedException();
        }

        public override ModInfo ModInfo { get; }
    }
}