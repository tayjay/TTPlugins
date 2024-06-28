using Exiled.Events.EventArgs.Player;

namespace TTAdmin.Data.Events;

public class HurtEventData : EventData
{
    public override string EventName => "hurt";
    public PlayerData Attacker { get; set; } = null;
    public PlayerData Player { get; set; } = null;
    public float Amount { get; set; }
    public string DamageType { get; set; }
    
    public HurtEventData(HurtEventArgs ev)
    {
        if(ev.Attacker != null)
            Attacker = new PlayerData(ev.Attacker);
        if(ev.Player != null)
            Player = new PlayerData(ev.Player);
        Amount = ev.Amount;
        DamageType = ev.DamageHandler?.Type.ToString();
    }
}