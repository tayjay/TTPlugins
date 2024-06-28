using Exiled.Events.EventArgs.Player;

namespace TTAdmin.Data.Events;

public class BannedEventData : EventData
{
    public override string EventName => "banned";
    public PlayerData PlayerData { get; set; }
    public string Details { get; set; }
    public string Type { get; set; }
    
    public BannedEventData(BannedEventArgs ev)
    {
        PlayerData = new PlayerData(ev.Player);
        Details = ev.Details.ToString();
        Type = ev.Type.ToString();
    }
}