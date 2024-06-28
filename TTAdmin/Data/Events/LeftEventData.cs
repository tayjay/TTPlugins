using Exiled.Events.EventArgs.Player;

namespace TTAdmin.Data.Events;

public class LeftEventData : EventData
{
    public override string EventName => "left";
    
    public PlayerData PlayerData { get; set; }
    
    public LeftEventData(LeftEventArgs ev)
    {
        PlayerData = new PlayerData(ev.Player);
    }
}