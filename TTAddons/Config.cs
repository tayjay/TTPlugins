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
    }
}