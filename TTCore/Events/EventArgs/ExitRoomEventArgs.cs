using Exiled.API.Features;

namespace TTCore.Events.EventArgs;

public class ExitRoomEventArgs : EnterRoomEventArgs
{
    public ExitRoomEventArgs(Room room, ReferenceHub player) : base(room, player)
    {
    }
}