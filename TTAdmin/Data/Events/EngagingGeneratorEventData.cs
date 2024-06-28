using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;

namespace TTAdmin.Data.Events;

public class EngagingGeneratorEventData : EventData
{
    public override string EventName => "engaging_generator";
    
    public GeneratorData Generator { get; set; }
    
    public EngagingGeneratorEventData(GeneratorActivatingEventArgs ev)
    {
        Generator = new GeneratorData(ev.Generator);
    }
}