using System;
using TTCore.API;

namespace RoundModifiers.API
{
    public abstract class Modifier :IRegistered
    {
        
        protected abstract void RegisterModifier();
        
        protected abstract void UnregisterModifier();


        public void Register()
        {
            IsEnabled = true;
            RegisterModifier();
        }

        public void Unregister()
        {
            if(!IsEnabled) return;
            IsEnabled = false;
            UnregisterModifier();
        }
        
        public bool IsEnabled { get; private set; }

        public abstract ModInfo ModInfo { get; }
    }
    public enum ImpactLevel
    {
        None,
        Graphics,
        MinorGameplay,
        MajorGameplay,
        Gamemode,
    }

    [Flags]
    public enum Category
    {
        None, // No category
        Visual, // Changes the visual appearance of something
        ScpRole, // Changes what SCP roles spawn
        ScpItem, // Changes the behaviour of an SCP item
        HumanRole, // Changes what Human roles spawn
        CustomRole, // Adds a custom role to the round
        Gamemode, // Full change of game logic for the round
        Scale, // Changes the scale/size of players/objects
        Health, // Modifies the health of players
        Weapon, // Modifies weapons and how they work
        Facility, // Changes behaviour of something facility wide
        Lights, // Changes the behaviour of lights
        Names, // Changes the names of players
        Voice, // Changes the voice of players
        OnDeath, // Changes what happens when a player dies
        Combat, // Changes combat mechanics
        Utility, // Changes utility mechanics
        HUD, // Adds a HUD to the players screen
        Overhaul, // Massive change to the game, but not a full gamemode
        Scp914, // Changes the behaviour of SCP-914
        Combo, // A combination of multiple modifiers
        Npc, // Spawns NPCs
        Effect, // Adds effects to players
    }
}