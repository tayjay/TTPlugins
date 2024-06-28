using Exiled.Events.EventArgs.Player;

namespace TTAdmin.Data.Events;

public class JoinedEventData : EventData
{
    public override string EventName => "joined";
    public PlayerData PlayerData { get; set; }
    
    public JoinedEventData(JoinedEventArgs ev)
    {
        PlayerData = new PlayerData(ev.Player);
    }
}