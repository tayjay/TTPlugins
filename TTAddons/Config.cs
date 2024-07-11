using System.ComponentModel;
using Exiled.API.Interfaces;

namespace TTAddons
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        
        [Description ("Allow SCP players to use the .unstuck command. Default is true.")]
        public bool AllowUnstuckForScps { get; set; } = true;
        
        [Description ("The time in seconds that SCPs can use the .unstuck command. Default is 30 seconds.")]
        public int UnstuckTime { get; set; } = 30;

        [Description("Chance of a player spawning as SCP-3114 (0-1)f. Default is 0.05.")]
        public float Scp3114Chance { get; set; } = 0.05f;
        [Description("Minimum players required to spawn SCP-3114. Default is 10.")]
        public int Scp3114MinPlayers { get; set; } = 10;
        [Description("True: Choose an SCP to replace with 3114, False: Choose a ClassD to replace with 3114. Default is false.")]
        public bool Scp3114ReplaceScp { get; set; } = false;
        
        [Description("Enable spectator lobby game. Default is true.")]
        public bool EnableSpectatorGame { get; set; } = false;
        
        [Description("Enable weapon stats. Default is true.")]
        public bool EnableWeaponStats { get; set; } = false;
        
        [Description("How long do Zombies get spawn protection for? Allows resurrecting players in Tesla gates. Default is 3f seconds.")]
        public float Scp0492SpawnProtection { get; set; } = 3f;
        
        [Description("Enable SCP-049 resurrection logic replacement? Allows changing maximum zombie resurrections, how long they have to revive, etc. Default is false.")]
        public bool ReplaceScp049ResurrectLogic { get; set; } = false;
        
        [Description("How many times can Scp049 resurrect a player? Default is 2.")]
        public int Scp049MaxResurrections { get; set; } = 2;
        
        [Description("How long does Scp049 have to resurrect a player that was a target of 'Doctor's good sense'? Default is 18f seconds.")]
        public float Scp049TargetCorpseDuration { get; set; } = 18f;
        
        [Description("How long does Scp049 have to resurrect a normal player? Default is 12f seconds.")]
        public float Scp049HumanCorpseDuration { get; set; } = 12f;
        
        [Description("How much Hume Shield does Scp049 get for resurrecting a player? Default is 200f.")]
        public float Scp049ResurrectTargetReward { get; set; } = 200f;
        
        [Description("Should Scp079's experience scale with player count? Default is false")]
        public bool EnableScp079XpScaling { get; set; } = false;
        
        [Description("Base player count for Scp079 experience scaling. Smaller numbers make the SCP weaker at higher player counts, bigger numbers make the SCP stronger at lower player counts. Default is 15.")]
        public int Scp079BasePlayerCount { get; set; } = 15;
        
        [Description("Disable auto-banning by the server. Only do so if you trust your players. Default is false.")]
        public bool NoAutoBan { get; set; } = false;
        [Description("Disable auto-kicking by the server. Only do so if you trust your players. Default is false.")]
        public bool NoAutoKick { get; set; } = false;
    }
}