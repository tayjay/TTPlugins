using System;
using System.Collections.Generic;
using System.Threading;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using TTAdmin.Handlers;
using TTAdmin.WebNew;
using Utf8Json;

namespace TTAdmin
{
    public class TTAdmin : Plugin<Config>
    {
        
        
        private static readonly TTAdmin Singleton = new TTAdmin();
        
        public static TTAdmin Instance => Singleton;

        
        public override string Name { get; } = "TTAdmin";
        public override string Author { get; } = "TayTay";
        public override Version Version { get; } = new Version(0, 1, 0);
        
        
        public SubscriptionHandler SubscriptionHandler;
        public EventsHandler EventsHandler;
        
        
        public void OnWaitingForPlayers()
        {
            var payload = new { ev = "waiting_for_players", timestamp = DateTime.Now };
            Timing.RunCoroutine(WSTick());
        }

        public IEnumerator<float> WSTick()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(1f);
                var payload = new { data="tps", value=Server.Tps };
                WebNew.WsServer.Server.BroadcastMessage(JsonSerializer.ToJsonString(payload));
            }
        }


        private TTAdmin()
        {
            Log.Info("TTAdmin has been constructed!");
        }

        public override PluginPriority Priority { get; } = PluginPriority.Higher;

        public override void OnEnabled()
        {
            base.OnEnabled();
            Setup();
            Log.Info("TTAdmin has been enabled!");
        }
        
        public override void OnDisabled()
        {
            base.OnDisabled();
            Shutdown();
        }
        
        public override void OnReloaded()
        {
            base.OnReloaded();
        }

        public void Setup()
        {
            /*if (Config.ApiKey == "")
            {
                //Need to generate a new API key
                Config.ApiKey = Guid.NewGuid().ToString();
                ServerConfigSynchronizer.RefreshAllConfigs();
                Log.Info("New API Key is: " + Config.ApiKey);
            }*/
            
            SubscriptionHandler = new SubscriptionHandler();
            EventsHandler = new EventsHandler();
            EventsHandler.Register();
            
            RestServer.Start();
            WebNew.WsServer.Start();
        }
        
        public void Shutdown()
        {
            RestServer.Stop();
            WebNew.WsServer.Stop();
        }
        
        
        
    }
}