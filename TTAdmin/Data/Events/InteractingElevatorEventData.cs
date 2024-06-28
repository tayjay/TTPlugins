using Exiled.Events.EventArgs.Player;

namespace TTAdmin.Data.Events;

public class InteractingElevatorEventData : EventData
{
    public override string EventName => "interacting_elevator";
    public PlayerData Player { get; set; }
    public LiftData Elevator { get; set; }
    
    public InteractingElevatorEventData(InteractingElevatorEventArgs ev)
    {
        Player = new PlayerData(ev.Player);
        Elevator = new LiftData(ev.Lift);
    }
}