using Exiled.Events.EventArgs.Player;

namespace TTAdmin.Data.Events;

public class DiedEventData : EventData
{
    public override string EventName => "died";
    public PlayerData Killer { get; set; } = null;
    public PlayerData Player { get; set; } = null;
    public string OldRole { get; set; }
    public bool IsSuicide { get; set; }
    public bool IsFriendlyFire { get; set; }
    public string DamageType { get; set; }
    


    public DiedEventData(DiedEventArgs ev)
    {
        if(ev.Attacker != null)
            Killer = new PlayerData(ev.Attacker);
        if(ev.Player != null)
            Player = new PlayerData(ev.Player);
        OldRole = ev.Player?.Role.Type.ToString();
        IsSuicide = ev.DamageHandler?.IsSuicide ?? false;
        IsFriendlyFire = ev.DamageHandler?.IsFriendlyFire ?? false;
        DamageType = ev.DamageHandler?.Type.ToString();
    }
}