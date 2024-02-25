using AdminToys;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;

namespace TTCore.Events.EventArgs;

public class AdminToyCollisionEventArgs : IExiledEvent
{
    public AdminToyBase AdminToy { get; set; }
    public Player Player { get; set; }
    
    public AdminToyCollisionEventArgs(AdminToyBase adminToy, Player player)
    {
        AdminToy = adminToy;
        Player = player;
    }
}