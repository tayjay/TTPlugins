namespace TTAdmin.Data.Events;

public class BasicEventData : EventData
{
    public override string EventName { get; }
    
    public BasicEventData(string eventName)
    {
        EventName = eventName;
    }
}