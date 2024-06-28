namespace TTAdmin.Data.Events;

public class DetonatedEventData : EventData
{
    public override string EventName => "warhead_detonated";
    
    public WarheadData Warhead { get; set; } = new();
}