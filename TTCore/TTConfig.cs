using Exiled.API.Interfaces;

namespace TTCore
{
    public class TTConfig : IConfig
    {
        public bool IsEnabled { get; set; }
        public bool Debug { get; set; }
    }
}