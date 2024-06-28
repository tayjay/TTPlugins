using Exiled.Events.EventArgs.Warhead;

namespace TTAdmin.Data.Events;

public class WarheadStoppingEventData : EventData
{
    public override string EventName => "warhead_stopping";
    
    public PlayerData Player { get; set; }
    public WarheadData Warhead { get; set; }
    
    public WarheadStoppingEventData(StoppingEventArgs ev)
    {
        if(ev.Player != null)
            Player = new PlayerData(ev.Player);
        Warhead = new WarheadData();
    }
}