using Exiled.Events.EventArgs.Player;

namespace TTAdmin.Data.Events;

public class StoppingGeneratorEventData : EventData
{
    public override string EventName => "stopping_generator";
    
    public PlayerData Player { get; set; }
    public GeneratorData Generator { get; set; }
    
    public StoppingGeneratorEventData(StoppingGeneratorEventArgs ev)
    {
        Player = new PlayerData(ev.Player);
        Generator = new GeneratorData(ev.Generator);
    }
}