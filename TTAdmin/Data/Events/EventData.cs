using System;
using Utf8Json;

namespace TTAdmin.Data.Events;

public abstract class EventData
{
    public abstract string EventName { get; }
    public DateTime TimeStamp => DateTime.Now;
    
   
}