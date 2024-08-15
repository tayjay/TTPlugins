using Exiled.Events.Features;
using TTCore.Events.EventArgs;

namespace TTCore.Events.Handlers;

public class RoomTrigger
{
    public static Event<EnterRoomEventArgs> EnterRoom = new();
    public static Event<ExitRoomEventArgs> ExitRoom = new();
    public static Event<StayRoomEventArgs> StayRoom = new();
    
    public static void OnEnterRoom(EnterRoomEventArgs ev) => EnterRoom.InvokeSafely(ev);
    public static void OnExitRoom(ExitRoomEventArgs ev) => ExitRoom.InvokeSafely(ev);
    public static void OnStayRoom(StayRoomEventArgs ev) => StayRoom.InvokeSafely(ev);
}