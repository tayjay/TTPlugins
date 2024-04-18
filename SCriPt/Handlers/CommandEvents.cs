using System;
using MoonSharp.Interpreter;
using MoonSharp.Interpreter.Interop;
using PluginAPI.Core;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;

namespace SCriPt.Handlers
{
    [MoonSharpUserData]
    public class CommandEvents : IEventHandler
    {
        // Triggers when a Remote Admin command is recieved
        [MoonSharpVisible(true)]
        public event EventHandler<RemoteAdminCommandEvent> RemoteAdminCommand;
        
        [PluginEvent(ServerEventType.RemoteAdminCommand), MoonSharpHidden]
        public void OnRemoteAdminCommandEvent(RemoteAdminCommandEvent ev)
        {
            Log.Info(ev.Command);
            RemoteAdminCommand?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<RemoteAdminCommandExecutedEvent> RemoteAdminCommandExecuted;
        
        [PluginEvent(ServerEventType.RemoteAdminCommandExecuted), MoonSharpHidden]
        public void OnRemoteAdminCommandExecutedEvent(RemoteAdminCommandExecutedEvent ev)
        {
            Log.Info(ev.Command);
            RemoteAdminCommandExecuted?.Invoke(this, ev);
        }
        
        // Triggers when a local admin server command is issued
        [MoonSharpVisible(true)]
        public event EventHandler<ConsoleCommandEvent> ConsoleCommand;
        
        [PluginEvent(ServerEventType.ConsoleCommand), MoonSharpHidden]
        public void OnConsoleCommandEvent(ConsoleCommandEvent ev)
        {
            Log.Info(ev.Command);
            ConsoleCommand?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<ConsoleCommandExecutedEvent> ConsoleCommandExecuted;
        
        [PluginEvent(ServerEventType.ConsoleCommandExecuted), MoonSharpHidden]
        public void OnConsoleCommandExecutedEvent(ConsoleCommandExecutedEvent ev)
        {
            Log.Info(ev.Command);
            ConsoleCommandExecuted?.Invoke(this, ev);
        }
        
        // Triggers when a player sends through the ~ console
        [MoonSharpVisible(true)]
        public event EventHandler<PlayerGameConsoleCommandEvent> PlayerGameConsoleCommand;
        
        [PluginEvent(ServerEventType.PlayerGameConsoleCommand), MoonSharpHidden]
        public void OnPlayerGameConsoleCommandEvent(PlayerGameConsoleCommandEvent ev)
        {
            Log.Info(ev.Command);
            PlayerGameConsoleCommand?.Invoke(this, ev);
        }
        
        [MoonSharpVisible(true)]
        public event EventHandler<PlayerGameConsoleCommandExecutedEvent> PlayerGameConsoleCommandExecuted;
        
        [PluginEvent(ServerEventType.PlayerGameConsoleCommandExecuted), MoonSharpHidden]
        public void OnPlayerGameConsoleCommandExecutedEvent(PlayerGameConsoleCommandExecutedEvent ev)
        {
            Log.Info(ev.Command);
            PlayerGameConsoleCommandExecuted?.Invoke(this, ev);
        }
        
        public void RegisterEvents()
        {
            PluginAPI.Events.EventManager.RegisterEvents(this);
            Exiled.API.Features.Log.Info("CommandEvents registered!");
        }

        public void RegisterEventTypes()
        {
            UserData.RegisterType<ConsoleCommandEvent>();
        }

        public void UnregisterEvents()
        {
            PluginAPI.Events.EventManager.UnregisterEvents(this);
        }
    }
}