using Exiled.Events.EventArgs.Player;

namespace TTAdmin.Data.Events;

public class KickedEventData : EventData
{
    public override string EventName => "kicked";
    public PlayerData PlayerData { get; set; }
    public string Reason { get; set; }
    
    public KickedEventData(KickedEventArgs ev)
    {
        PlayerData = new PlayerData(ev.Player);
        Reason = ev.Reason;
    }
}