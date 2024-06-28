using Exiled.Events.EventArgs.Player;

namespace TTAdmin.Data.Events;

public class KillingPlayerEventData : EventData
{
    public override string EventName => "force_killing_player";
    
    public PlayerData Player { get; set; }
    public string Reason { get; set; }
    
    public KillingPlayerEventData(KillingPlayerEventArgs ev)
    {
        Player = new PlayerData(ev.Player);
        Reason = ev.Handler.ServerLogsText;
    }
}