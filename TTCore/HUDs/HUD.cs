using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using MEC;
using UnityEngine;

namespace TTCore.HUDs;

public class HUD
{
    private static Dictionary<Player,HUD> Huds = new Dictionary<Player, HUD>();
        
        private float LastDisplayTime;
        
        private float RefreshRate => Layout.RefreshRate;
        
        public HUDLayout Layout { get; protected set; }
        public HUDHint DisplayingHint { get; private set; }

        private Player localPlayer;

        public Player Owner => localPlayer;
        
        public bool IsDisplayingHint => DisplayingHint != null;

        protected HUD() : base()
        {
            
        }
        
        public static HUD GetHUD(Player player)
        {
            try
            {
                //Log.Info("Getting HUD Start");
                if (player == null) return null;
                if(player.GameObject == null) return null;
                //Log.Info("Getting HUD");
                if (player.IsDead) return null;
                if (player.IsNPC) return null;
                if (Huds.TryGetValue(player, out HUD existingHud))
                {
                    //Log.Info("HUD already exists for player");
                    return existingHud;
                }
                //Log.Info("Creating HUD as it doesn't exist");
                HUD hud = new HUD(player);
                if(hud==null) return null;
                //Log.Info("Hud created successfully, linking to Layout");
                hud.SetLayout(new HUDLayout());
                hud.GetLayout<HUDLayout>().SetHUD(hud);
               //Log.Info("Adding HUD to dictionary");
                //Huds.Add(player,hud);
                Huds[player] = hud;
                //Log.Info("Returning HUD");
                return hud;
                
            }
            catch (Exception e)
            {
                Log.Error("Error getting HUD: "+e);
                return null;
            }
        }
        
        public static bool TryGetHUD(Player player, out HUD hud)
        {
            hud = GetHUD(player);
            return hud != null;
        }
        
        public static HUD SetupHUD(Player player, HUDLayout layout)
        {
            HUD hud = null;
            try
            {
                if (player == null) return null;
                //player.TryGetComponent(out HUD h);
                //Log.Info(h);
                hud = new HUD(player);
                if(hud==null) return null;
                hud.SetLayout(layout);
                Huds[player]= hud;
            } catch(Exception e)
            {
                Log.Error("Error setting up HUD: "+e);
            }
            //Log.Info("Returning HUD "+ gameHUD?.Name);
            return hud;
        }
        
        public static void RemoveHUD(Player player)
        {
            if(Huds.ContainsKey(player))
            {
                Huds.Remove(player);
            }
        }
        
        private HUD(Player player) : base()
        {
            //Name = "HUD";
            localPlayer = player;
            LastDisplayTime = 0;
            Layout = null;
        }
        
        public void SetLayout(HUDLayout layout)
        {
            Layout = layout;
        }
        
        public T GetLayout<T>() where T : HUDLayout
        {
            if(Layout==null) return null;
            return Layout as T;
        }

        protected void BehaviourUpdate()
        {
            //Log.Info("Updating HUD");
            if (DisplayingHint != null)
            {
                if(Time.time - DisplayingHint.StartTime > DisplayingHint.Duration)
                {
                    DisplayingHint = null;
                }
            }
            if (Time.time - LastDisplayTime > RefreshRate)
            {
                UpdateHUD();
            }
        }
        
        public void ShowHint(string text, float duration = 3f)
        {
            DisplayingHint = new HUDHint(text, duration);
            UpdateHUD();
        }
        
        private void UpdateHUD()
        {
            //Log.Info("Displaying HUD for "+Owner.Nickname+" with text: "+BuildHUD());
            LastDisplayTime = Time.time;
            Owner.ShowHint(BuildHUD(), RefreshRate+0.1f);
        }

        private string BuildHUD()
        {
            return Layout.BuildHUD();
        }
        
        //mod set CaptureTheFlag

        public class HUDHint
        {
            public string Text { get; set; }
            public float Duration { get; set; }
            public float StartTime { get; set; }

            public HUDHint(string text, float duration = 3f)
            {
                Text = text;
                Duration = duration;
                StartTime = Time.time;
            }
        }


        public static void ClearHUDs()
        {
            Huds.Clear();
        }

        public static CoroutineHandle HUDCoroutine;
        public static void OnRoundStart()
        {
            HUDCoroutine = Timing.RunCoroutine(HUDLoop());
        }

        public static IEnumerator<float> HUDLoop()
        {
            while (true)
            {
                foreach(HUD hud in Huds.Values)
                {
                    if(!hud.Layout.ShouldDisplay()) continue;
                    hud.BehaviourUpdate();
                }

                yield return Timing.WaitForOneFrame;
            }
        }
        
        public static void OnRoundRestart()
        {
            ClearHUDs();
            Timing.KillCoroutines(HUDCoroutine);
        }

        public static void Register()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestart;
            Huds = DictionaryPool<Player, HUD>.Pool.Get();
        }
        
        public static void Unregister()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
            ClearHUDs();
            //UnregisterObjectType("HUD");
            Timing.KillCoroutines(HUDCoroutine);
            DictionaryPool<Player,HUD>.Pool.Return(Huds);
        }
}

    
    public static class HUDExtensions
    {
        public static void ShowHUDHint(this Player player, string text, float duration = 3f)
        {
            try
            {
                if(HUD.TryGetHUD(player, out HUD hud))
                    hud.ShowHint(text, duration);
                else
                    player.ShowHint(text, duration);
            } catch(Exception e)
            {
                Log.Error("Error showing HUD hint: "+e);
            }
            
        }
        
        public static HUD GetHUD(this Player player)
        {
            return HUD.GetHUD(player);
        }
        
        public static HUD SetHUDLayout(this Player player, HUDLayout layout)
        {
            return HUD.SetupHUD(player, layout);
        }
        
        public static bool TryGetHUD(this Player player, out HUD gameHUD)
        {
            return HUD.TryGetHUD(player, out gameHUD);
        }
        
        public static T GetHUDLayout<T>(this Player player) where T : HUDLayout
        {
            return HUD.GetHUD(player)?.GetLayout<T>();
        }
}
