using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using TTAdmin.Admin;
using TTAdmin.Handlers;
using TTAdmin.Security;
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
        public override Version Version { get; } = new Version(0, 3, 0);
        
        
        public SubscriptionHandler SubscriptionHandler;
        public EventsHandler EventsHandler;
        //public WsConsoleOutput WsConsoleOutput;
        
        protected internal APIKey ApiKey { get; private set; }
        
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
            
            ApiKey = APIKey.FromFile("TTCore/TTAdmin/APIKey.json");
            //Check if ApiKey Created DateTime is more than a month ago
            if (ApiKey.Created.AddMonths(Config.ApiKeyWarningAge) < DateTime.Now)
            {
                Log.Error("===========================================================");
                Log.Error("API Key is old, we recommend generating a new one by deleting the file at ./TTCore/TTAdmin/APIKey.json");
                Log.Error("===========================================================");
            }

            /*if (Config.EnableConsoleOutput)
            {
                WsConsoleOutput = new WsConsoleOutput();
                WsConsoleOutput.Register();
            }*/
        }
        
        public void Shutdown()
        {
            RestServer.Stop();
            WebNew.WsServer.Stop();
            
            EventsHandler.Unregister();
            SubscriptionHandler.Subscriptions.Clear();
            /*if (Config.EnableConsoleOutput)
            {
                WsConsoleOutput.Unregister();
                WsConsoleOutput = null;
            }*/
            EventsHandler = null;
            SubscriptionHandler = null;
            
        }
        
        
        
    }
}