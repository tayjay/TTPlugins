using System;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class ServerEvents
    {
        
        [MoonSharpVisible(true)]
        public event EventHandler WaitingForPlayers; 
        
        [MoonSharpHidden]
        public void OnWaitingForPlayers()
        {
            //EventManager.CallEvent("WaitingForPlayers");
            WaitingForPlayers?.Invoke(this, EventArgs.Empty);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler RoundStart; 
        
        [MoonSharpHidden]
        public void OnRoundStart()
        {
            //EventManager.CallEvent("RoundStart");
            RoundStart?.Invoke(null, EventArgs.Empty);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<EndingRoundEventArgs> EndingRound;
        
        [MoonSharpHidden]
        public void OnRoundEnding(EndingRoundEventArgs ev)
        {
            //EventManager.CallEvent("RoundEnding", ev);
            EndingRound?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<RoundEndedEventArgs> RoundEnded;
        
        [MoonSharpHidden]
        public void OnRoundEnded(RoundEndedEventArgs ev)
        {
            //EventManager.CallEvent("RoundEnded", ev);
            RoundEnded?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler RestartingRound;
        
        [MoonSharpHidden]
        public void OnRestartingRound()
        {
            RestartingRound?.Invoke(null, EventArgs.Empty);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ReportingCheaterEventArgs> ReportingCheater;
        
        [MoonSharpHidden]
        public void OnReportingCheater(ReportingCheaterEventArgs ev)
        {
            ReportingCheater?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<RespawningTeamEventArgs> RespawningTeam;
        
        [MoonSharpHidden]
        public void OnRespawningTeam(RespawningTeamEventArgs ev)
        {
            RespawningTeam?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<AddingUnitNameEventArgs> AddingUnitName;
        
        [MoonSharpHidden]
        public void OnAddingUnitName(AddingUnitNameEventArgs ev)
        {
            AddingUnitName?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<LocalReportingEventArgs> LocalReporting;
        
        [MoonSharpHidden]
        public void OnLocalReporting(LocalReportingEventArgs ev)
        {
            LocalReporting?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ChoosingStartTeamQueueEventArgs> ChoosingStartTeam;
        
        [MoonSharpHidden]
        public void OnChoosingStartTeam(ChoosingStartTeamQueueEventArgs ev)
        {
            ChoosingStartTeam?.Invoke(null, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler ReloadedConfigs;
        
        [MoonSharpHidden]
        public void OnReloadedConfigs()
        {
            ReloadedConfigs?.Invoke(null, EventArgs.Empty);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler ReloadedTranslations;
        
        [MoonSharpHidden]
        public void OnReloadedTranslations()
        {
            ReloadedTranslations?.Invoke(null, EventArgs.Empty);
        }

        [MoonSharpVisible(true)]
        public event EventHandler ReloadedGameplay;
        
        [MoonSharpHidden]
        public void OnReloadedGameplay()
        {
            ReloadedGameplay?.Invoke(null, EventArgs.Empty);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler ReloadedRA;
        
        [MoonSharpHidden]
        public void OnRealoadedRA()
        {
            ReloadedRA?.Invoke(null, EventArgs.Empty);
        }

        [MoonSharpVisible(true)]
        public event EventHandler ReloadedPlugins;
        
        [MoonSharpHidden]
        public void OnReloadedPlugins()
        {
            ReloadedPlugins?.Invoke(null, EventArgs.Empty);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler ReloadedPermission;
        
        [MoonSharpHidden]
        public void OnReloadedPermission()
        {
            ReloadedPermission?.Invoke(null, EventArgs.Empty);
        }
        
        [MoonSharpHidden]
        public void RegisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Server.EndingRound += OnRoundEnding;
            Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound += OnRestartingRound;
            Exiled.Events.Handlers.Server.ReportingCheater += OnReportingCheater;
            Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
            Exiled.Events.Handlers.Server.AddingUnitName += OnAddingUnitName;
            Exiled.Events.Handlers.Server.LocalReporting += OnLocalReporting;
            Exiled.Events.Handlers.Server.ChoosingStartTeamQueue += OnChoosingStartTeam;
            Exiled.Events.Handlers.Server.ReloadedConfigs += OnReloadedConfigs;
            Exiled.Events.Handlers.Server.ReloadedTranslations += OnReloadedTranslations;
            Exiled.Events.Handlers.Server.ReloadedGameplay += OnReloadedGameplay;
            Exiled.Events.Handlers.Server.ReloadedRA += OnRealoadedRA;
            Exiled.Events.Handlers.Server.ReloadedPlugins += OnReloadedPlugins;
            Exiled.Events.Handlers.Server.ReloadedPermissions += OnReloadedPermission;
        }

        [MoonSharpHidden]
        public void RegisterEventTypes()
        {
            UserData.RegisterType<EndingRoundEventArgs>();
            UserData.RegisterType<RoundEndedEventArgs>();
            UserData.RegisterType<RespawningTeamEventArgs>();
            UserData.RegisterType<AddingUnitNameEventArgs>();
            UserData.RegisterType<LocalReportingEventArgs>();
            UserData.RegisterType<ChoosingStartTeamQueueEventArgs>();
        }

        [MoonSharpHidden]
        public void UnregisterEvents()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Server.EndingRound -= OnRoundEnding;
            Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
            Exiled.Events.Handlers.Server.RestartingRound -= OnRestartingRound;
            Exiled.Events.Handlers.Server.ReportingCheater -= OnReportingCheater;
            Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
            Exiled.Events.Handlers.Server.AddingUnitName -= OnAddingUnitName;
            Exiled.Events.Handlers.Server.LocalReporting -= OnLocalReporting;
            Exiled.Events.Handlers.Server.ChoosingStartTeamQueue -= OnChoosingStartTeam;
            Exiled.Events.Handlers.Server.ReloadedConfigs -= OnReloadedConfigs;
            Exiled.Events.Handlers.Server.ReloadedTranslations -= OnReloadedTranslations;
            Exiled.Events.Handlers.Server.ReloadedGameplay -= OnReloadedGameplay;
            Exiled.Events.Handlers.Server.ReloadedRA -= OnRealoadedRA;
            Exiled.Events.Handlers.Server.ReloadedPlugins -= OnReloadedPlugins;
            Exiled.Events.Handlers.Server.ReloadedPermissions -= OnReloadedPermission;
        }
        
    }
}