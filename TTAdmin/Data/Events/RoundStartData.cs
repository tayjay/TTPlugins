using System;

namespace TTAdmin.Data.Events;

public class RoundStartData : EventData
{
    public override string EventName => "round_start";
}