using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Interfaces;

namespace TTCore.Events.EventArgs;

public class InspectFirearmEventArgs : IExiledEvent
{
    public Player Player { get; }
    public Firearm Firearm { get; }

    public ushort Serial => Firearm.Serial;
    
    public InspectFirearmEventArgs(Player player, Firearm firearm)
    {
        Player = player;
        Firearm = firearm;
    }
}