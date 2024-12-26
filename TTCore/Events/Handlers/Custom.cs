using Exiled.Events.Features;
using TTCore.Components;
using TTCore.Events.EventArgs;

namespace TTCore.Events.Handlers;

public static class Custom
{
    public static Event CustomEvent { get; set; } = new Event();
    
    public static Event<AdminToyCollisionEventArgs> AdminToyCollision { get; set; } = new Event<AdminToyCollisionEventArgs>();
    
    public static Event<Scp018BounceEventArgs> Scp018Bounce { get; set; } = new Event<Scp018BounceEventArgs>();
    
    public static Event<SetupNpcBrainEventArgs> SetupNpcBrain { get; set; } = new Event<SetupNpcBrainEventArgs>();
    
    public static Event<InspectFirearmEventArgs> InspectFirearm { get; set; } = new Event<InspectFirearmEventArgs>();
    
    //public static Event<AccessFirearmBaseStatsEventArgs> AccessFirearmBaseStats { get; set; } = new Event<AccessFirearmBaseStatsEventArgs>();
    //public static Event<ZombieAttackEventArgs> ZombieAttack { get; set; } = new Event<ZombieAttackEventArgs>();
   
    public static Event<ChooseScpSpawnQueueEventArgs> ChooseScpSpawnQueue { get; set; } = new Event<ChooseScpSpawnQueueEventArgs>();
   
    public static Event<PlayerEffectsAwakeArgs> PlayerEffectsAwake { get; set; } = new Event<PlayerEffectsAwakeArgs>();
    
    public static Event<PickupCollisionEventArgs> PickupCollision { get; set; } = new Event<PickupCollisionEventArgs>();
    
    public static Event PreGenerateMap { get; set; } = new Event();
    
    //public static Event<PreGenerateZoneEventArgs> PreGenerateZone { get; set; } = new Event<PreGenerateZoneEventArgs>();
    
    public static Event Scp079BeginOvercharge { get; set; } = new Event();
    
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
    
    /*public static void OnAccessFirearmBaseStats(AccessFirearmBaseStatsEventArgs ev)
    {
        AccessFirearmBaseStats.InvokeSafely(ev);
    }
    */
    
    /*public static void OnZombieAttack(ZombieAttackEventArgs ev)
    {
        ZombieAttack.InvokeSafely(ev);
    }*/
    
    public static void OnChooseScpSpawnQueue(ChooseScpSpawnQueueEventArgs ev)
    {
        ChooseScpSpawnQueue.InvokeSafely(ev);
    }
    
    public static void OnPlayerEffectsAwake(PlayerEffectsAwakeArgs ev)
    {
        PlayerEffectsAwake.InvokeSafely(ev);
    }
    
    public static void OnPickupCollision(PickupCollisionEventArgs ev)
    {
        PickupCollision.InvokeSafely(ev);
    }
    
    public static void OnScp079BeginOvercharge()
    {
        Scp079BeginOvercharge.InvokeSafely();
    }
    
    public static void OnPreGenerateMap()
    {
        PreGenerateMap.InvokeSafely();
    }
    
    /*public static void OnPreGenerateZone(PreGenerateZoneEventArgs ev)
    {
        PreGenerateZone.InvokeSafely(ev);
    }*/
}