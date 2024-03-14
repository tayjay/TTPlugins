using System;
using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using MEC;
using TTAdmin.Scripting.Custom;
using TTAdmin.Web;

namespace TTAdmin
{
    public class TTAdmin : Plugin<Config>
    {
        
        
        private static readonly TTAdmin Singleton = new TTAdmin();
        
        public static TTAdmin Instance => Singleton;

        
        public override string Name { get; } = "TTAdmin";
        public override string Author { get; } = "TayTay";
        public override Version Version { get; } = new Version(0, 1, 0);
        
        //private CoroutineHandle AdminPanelCoroutine;
        //GameServerClient GameServerClient;
        //ScriptLoader ScriptLoader;
        
       

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
            //AdminPanelCoroutine = Timing.RunCoroutine(AdminPanel());
            try
            {
                //ScriptLoader = new ScriptLoader();
                //ScriptLoader.Initialize();
            } catch (Exception e)
            {
                Log.Error(e);
            }
            Log.Info("TTAdmin has been setup!");
        }
        
        public void Shutdown()
        {
            //Timing.KillCoroutines(AdminPanelCoroutine);
        }
        
        /*private IEnumerator<float> AdminPanel()
        { 
            GameServerClient = new GameServerClient("localhost:8080");
            Timing.CallDelayed(1f, () =>
            {
                GameServerClient.ConnectAsync();
            });
            yield return Timing.WaitForSeconds(1f);
        }*/
        
        
    }
}