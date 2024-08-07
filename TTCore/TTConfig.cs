﻿using System.ComponentModel;
using Exiled.API.Interfaces;

namespace TTCore
{
    public class TTConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Enable pathfinding for NPCs. Requires generating a NavMesh")]
        public bool EnablePathFinding = false;
        
        [Description("Enable custom effects to be registered")]
        public bool EnableCustomEffects { get; set; } = true;
        
        [Description("Enable voice modification for players")]
        public bool EnableVoiceModification { get; set; } = true;
        
        [Description("Enable AI NPCs")]
        public bool EnableAi { get; set; } = true;

        [Description("Should console output be written to a file for easier debugging?")]
        public bool WriteConsoleOutputToFile { get; set; } = false;
        
    }
}