using Exiled.Events.EventArgs.Warhead;

namespace TTAdmin.Data.Events;

public class WarheadStartingEventData : EventData
{
    public override string EventName => "warhead_starting";
    
    public PlayerData Player { get; set; }
    public WarheadData Warhead { get; set; }
    public bool IsAuto { get; set; }

    public WarheadStartingEventData(StartingEventArgs ev)
    {
        if(ev.Player != null)
            Player = new PlayerData(ev.Player);
        Warhead = new WarheadData();
        IsAuto = ev.IsAuto;
    }
}