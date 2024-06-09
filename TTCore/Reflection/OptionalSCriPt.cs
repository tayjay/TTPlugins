using Exiled.API.Interfaces;
using Exiled.Loader;

namespace TTCore.Reflection;

public class OptionalSCriPt : OptionalPlugin
{
    public OptionalSCriPt() : base(Loader.GetPlugin("SCriPt"))
    {
        
    }

    public OptionalReference SetupConnector(IPlugin<IConfig> plugin)
    {
        if(Plugin == null) return null;
        if (Create(CallMethod("SetupConnector", plugin), out OptionalReference connector))
        {
            return connector;
        }

        return null;

    }
}