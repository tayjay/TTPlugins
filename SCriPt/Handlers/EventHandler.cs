using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using MoonSharp.Interpreter;


namespace SCriPt.Handlers
{
    public class EventHandler
    {
        public void OnRoundStart()
        {
            
        }

        public void OnWaitingForPlayers()
        {
            foreach(Script script in SCriPt.Instance.LoadedScripts)
            {
                if(script.Globals["onWaitingForPlayers"] is Closure func)
                {
                    Log.Info("Calling OnWaitingForPlayers");
                    func.Call();
                }
                else
                {
                    Log.Info("OnWaitingForPlayers is " + script.Globals["OnWaitingForPlayers"].GetType());
                }
            }
        }

        public void OnSpawn(SpawnedEventArgs ev)
        {
            foreach(Script script in SCriPt.Instance.LoadedScripts)
            {
                if(script.Globals["onPlayerSpawned"] is Closure func)
                {
                    Log.Info("Calling OnPlayerSpawned");
                    func.Call(ev.Player);
                }
            }
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