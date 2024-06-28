using Exiled.Events.EventArgs.Player;

namespace TTAdmin.Data.Events;

public class SpawnedEventData : EventData
{
    public override string EventName => "spawned";
    public PlayerData PlayerData { get; set; }
    public string Reason { get; set; }
    public string OldRole { get; set; }
    public string SpawnFlags { get; set; }

    public SpawnedEventData(SpawnedEventArgs ev)
    {
        PlayerData = new PlayerData(ev.Player);
        Reason = ev.Reason.ToString();
        OldRole = ev.OldRole.Type.ToString();
        SpawnFlags = ev.SpawnFlags.ToString();
    }
}