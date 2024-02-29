using System.ComponentModel;
using Exiled.API.Interfaces;

namespace TTCore
{
    public class TTConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        [Description("Enable pathfinding for NPCs. Requires generating a NavMesh, takes awhile to load.")]
        public bool EnablePathFinding = false;
    }
}