﻿using Exiled.Events.Features;
using TTCore.Events.EventArgs;

namespace TTCore.Events.Handlers;

public static class Custom
{
    public static Event CustomEvent { get; set; } = new Event();
    
    public static Event<AdminToyCollisionEventArgs> AdminToyCollision { get; set; } = new Event<AdminToyCollisionEventArgs>();
    
    public static Event<Scp018BounceEventArgs> Scp018Bounce { get; set; } = new Event<Scp018BounceEventArgs>();
    
    public static Event<SetupNpcBrainEventArgs> SetupNpcBrain { get; set; } = new Event<SetupNpcBrainEventArgs>();
    
    public static Event<InspectFirearmEventArgs> InspectFirearm { get; set; } = new Event<InspectFirearmEventArgs>();
    
    public static Event<AccessFirearmBaseStatsEventArgs> AccessFirearmBaseStats { get; set; } = new Event<AccessFirearmBaseStatsEventArgs>();
    
    public static void OnAdminToyCollision(AdminToyCollisionEventArgs ev)
    {
        AdminToyCollision.InvokeSafely(ev);
    }
    
    public static void OnScp018Bounce(Scp018BounceEventArgs ev)
    {
        Scp018Bounce.InvokeSafely(ev);
    }
    
    public static void OnSetupNpcBrain(SetupNpcBrainEventArgs ev)
    {
        SetupNpcBrain.InvokeSafely(ev);
    }

    public static void OnInspectFirearm(InspectFirearmEventArgs ev)
    {
        InspectFirearm.InvokeSafely(ev);
    }
    
    public static void OnAccessFirearmBaseStats(AccessFirearmBaseStatsEventArgs ev)
    {
        AccessFirearmBaseStats.InvokeSafely(ev);
    }
}