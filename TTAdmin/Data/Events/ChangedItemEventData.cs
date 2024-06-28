using Exiled.Events.EventArgs.Player;

namespace TTAdmin.Data.Events;

public class ChangedItemEventData : EventData
{
    public override string EventName => "changed_item";
    public PlayerData Player { get; set; }
    public string Item { get; set; }
    public string OldItem { get; set; }


    public ChangedItemEventData(ChangedItemEventArgs ev)
    {
        Player = new PlayerData(ev.Player);
        Item = ev.Item?.Type.ToString();
        OldItem = ev.OldItem?.Type.ToString();
    }
}