using System.ComponentModel;
using Exiled.API.Interfaces;

namespace TTAddons
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        
        [Description ("Allow SCP players to use the .unstuck command.")]
        public bool AllowUnstuckForScps { get; set; } = true;
        
        [Description ("The time in seconds that SCPs can use the .unstuck command.")]
        public int UnstuckTime { get; set; } = 30;

        [Description("Chance of a player spawning as SCP-3114 (0-1)f")]
        public float Scp3114Chance { get; set; } = 0.05f;
    }
}