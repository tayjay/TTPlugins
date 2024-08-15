using Exiled.API.Features;

namespace TTCore.Events.EventArgs;

public class StayRoomEventArgs : EnterRoomEventArgs
{
    public StayRoomEventArgs(Room room, ReferenceHub player) : base(room, player)
    {
    }
}