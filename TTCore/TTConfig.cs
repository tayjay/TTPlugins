using Exiled.API.Interfaces;

namespace TTCore
{
    public class TTConfig : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
    }
}