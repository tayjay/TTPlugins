using Exiled.Events.EventArgs.Player;
using MoonSharp.Interpreter;

namespace SCriPt.Handlers
{
    public class Events
    {
        public void OnRoundStart()
        {
            
        }

        public void OnWaitingForPlayers()
        {
            foreach(Script script in SCriPt.Instance.LoadedScripts)
            {
                
            }
        }

        public void OnSpawn(SpawnedEventArgs ev)
        {
            
        }
        
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Spawned += OnSpawn;
        }
        
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Player.Spawned -= OnSpawn;
        }
    }
}