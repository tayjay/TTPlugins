using TTCore.Events.EventArgs;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces;

public interface IRoomEvent
{
    void OnEnterRoom(EnterRoomEventArgs ev);
    
    void OnExitRoom(ExitRoomEventArgs ev);
}