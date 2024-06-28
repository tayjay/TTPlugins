using Exiled.Events.EventArgs.Player;

namespace TTAdmin.Data.Events;

public class StartingGeneratorEventData : EventData
{
    public override string EventName => "starting_generator";
    
    public PlayerData Player { get; set; }
    public GeneratorData Generator { get; set; }
    
    
    public StartingGeneratorEventData(ActivatingGeneratorEventArgs ev)
    {
        Player = new PlayerData(ev.Player);
        Generator = new GeneratorData(ev.Generator);
    }
}