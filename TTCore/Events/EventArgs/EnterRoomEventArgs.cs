using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;

namespace TTCore.Events.EventArgs;

public class EnterRoomEventArgs : IExiledEvent
{
    public Room Room { get; set; }
    public Player Player { get; set; }
    
    public EnterRoomEventArgs(Room room, ReferenceHub player)
    {
        Room = room;
        Player = Player.Get(player);
    }
}