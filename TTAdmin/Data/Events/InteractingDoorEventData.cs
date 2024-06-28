using Exiled.Events.EventArgs.Player;

namespace TTAdmin.Data.Events;

public class InteractingDoorEventData : EventData
{
    public override string EventName => "interacting_door";
    public PlayerData Player { get; set; }
    public DoorData Door { get; set; }
    
    public InteractingDoorEventData(InteractingDoorEventArgs ev)
    {
        Player = new PlayerData(ev.Player);
        Door = new DoorData(ev.Door);
    }

    
}