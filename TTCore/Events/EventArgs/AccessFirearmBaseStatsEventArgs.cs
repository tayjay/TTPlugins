using Exiled.Events.EventArgs.Interfaces;
using InventorySystem.Items.Firearms;
using Firearm = Exiled.API.Features.Items.Firearm;

namespace TTCore.Events.EventArgs;

public class AccessFirearmBaseStatsEventArgs : IExiledEvent
{
    public ushort Serial { get; }
    public Firearm Firearm => (Firearm)Firearm.Get(Serial);
    
    public FirearmBaseStats BaseStats { get; set; }
    
    public AccessFirearmBaseStatsEventArgs(ushort serial, FirearmBaseStats baseStats)
    {
        Serial = serial;
        BaseStats = baseStats;
    }
}