using System.ComponentModel;
using Exiled.API.Interfaces;

namespace SCriPt
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;
        
        [Description("Should Lua print statements display as Info (true) or Debug (false) level in console, default is false")]
        public bool PrintInfo { get; set; } = false;
    }
}