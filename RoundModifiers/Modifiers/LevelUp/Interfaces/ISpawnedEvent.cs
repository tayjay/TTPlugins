using Exiled.Events.EventArgs.Player;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces;

public interface ISpawnedEvent
{
    void OnSpawned(SpawnedEventArgs ev);
}