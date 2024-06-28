using System.Diagnostics;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Server;
using Exiled.Events.EventArgs.Warhead;
using MEC;
using PluginAPI.Core.Attributes;
using PluginAPI.Enums;
using PluginAPI.Events;
using TTAdmin.Data.Events;
using TTAdmin.WebNew;
using TTCore.API;
using Utf8Json;

namespace TTAdmin.Handlers;

public class EventsHandler : IRegistered
{
    
    protected void BroadcastEvent<T>(T data) where T : EventData
    {
        Timing.CallDelayed(Timing.WaitForOneFrame, () =>
        {
            Log.Debug($"Broadcasting event: {data.EventName}");
            Stopwatch sw = new Stopwatch();
            sw.Start();
            string response = JsonSerializer.ToJsonString(data);
            TTAdmin.Instance.SubscriptionHandler.GetClientsWithSubscription(data).ForEach(client =>
            {
                WebNew.WsServer.Server.SendMessage(client, response);
            });
            sw.Stop();
            Log.Debug($"Broadcasted event: {data.EventName} in {sw.ElapsedMilliseconds}ms");
        });
    }
    
    public void OnRoundStart()
    {
        Log.Info("Round started!");
        RoundStartData data = new RoundStartData();
        BroadcastEvent(data);
    }

    public void OnSpawned(SpawnedEventArgs ev)
    {
        SpawnedEventData data = new SpawnedEventData(ev);
        BroadcastEvent(data);
    }

    public void OnKicked(KickedEventArgs ev)
    {
        KickedEventData data = new KickedEventData(ev);
        BroadcastEvent(data);
    }
    
    public void OnBanned(BannedEventArgs ev)
    {
        BannedEventData data = new BannedEventData(ev);
        BroadcastEvent(data);
    }
    
    public void OnJoined(JoinedEventArgs ev)
    {
        JoinedEventData data = new JoinedEventData(ev);
        BroadcastEvent(data);
    }
    
    public void OnLeft(LeftEventArgs ev)
    {
        LeftEventData data = new LeftEventData(ev);
        BroadcastEvent(data);
    }
    
    public void OnHurt(HurtEventArgs ev)
    {
        HurtEventData data = new HurtEventData(ev);
        BroadcastEvent(data);
    }
    
    public void OnDied(DiedEventArgs ev)
    {
        DiedEventData data = new DiedEventData(ev);
        BroadcastEvent(data);
    }
    
    public void OnChangedItem(ChangedItemEventArgs ev)
    {
        ChangedItemEventData data = new ChangedItemEventData(ev);
        BroadcastEvent(data);
    }
    
    public void OnInteractingDoor(InteractingDoorEventArgs ev)
    {
        InteractingDoorEventData data = new InteractingDoorEventData(ev);
        BroadcastEvent(data);
    }
    
    public void OnInteractingElevator(InteractingElevatorEventArgs ev)
    {
        InteractingElevatorEventData data = new InteractingElevatorEventData(ev);
        BroadcastEvent(data);
    }
    
    //Server
    public void OnWaitingForPlayers()
    {
        BasicEventData data = new BasicEventData("waiting_for_players");
        BroadcastEvent(data);
    }
    
    
    public void OnRoundEnded(RoundEndedEventArgs ev)
    {
        RoundEndedEventData data = new RoundEndedEventData(ev);
        BroadcastEvent(data);
    }
    
    public void OnRespawningTeam(RespawningTeamEventArgs ev)
    {
        RespawningTeamEventData data = new RespawningTeamEventData(ev);
        BroadcastEvent(data);
    }
    
    public void OnRoundRestarting()
    {
        
        BasicEventData data = new BasicEventData("round_restart");
        BroadcastEvent(data);
    }

    public void OnGenerated()
    {
        GeneratedEventData data = new GeneratedEventData();
        BroadcastEvent(data);
    }
    
    //Warhead

    public void OnStarting(StartingEventArgs ev)
    {
        WarheadStartingEventData data = new WarheadStartingEventData(ev);
        BroadcastEvent(data);
    }
    
    public void OnStopping(StoppingEventArgs ev)
    {
        WarheadStoppingEventData data = new WarheadStoppingEventData(ev);
        BroadcastEvent(data);
    }
    
    public void OnDetonated()
    {
        DetonatedEventData data = new DetonatedEventData();
        BroadcastEvent(data);
    }
    
    [PluginEvent(ServerEventType.RemoteAdminCommandExecuted)]
    public void OnRemoteAdminCommandExecutedEvent(RemoteAdminCommandExecutedEvent ev)
    {
        RemoteAdminCommandExecutedData data = new RemoteAdminCommandExecutedData(ev);
        BroadcastEvent(data);
    }
    
    [PluginEvent(ServerEventType.ConsoleCommandExecuted)]
    public void OnConsoleCommandExecutedEvent(ConsoleCommandExecutedEvent ev)
    {
        ConsoleEventExecutedData data = new ConsoleEventExecutedData(ev);
        BroadcastEvent(data);
    }
    
    [PluginEvent(ServerEventType.PlayerGameConsoleCommandExecuted)]
    public void OnPlayerGameConsoleCommandExecutedEvent(PlayerGameConsoleCommandExecutedEvent ev)
    {
        PlayerGameConsoleCommandExecutedData data = new PlayerGameConsoleCommandExecutedData(ev);
        BroadcastEvent(data);
    }
    
    //Generator
    
    public void OnEngagingGenerator(GeneratorActivatingEventArgs ev)
    {
        EngagingGeneratorEventData data = new EngagingGeneratorEventData(ev);
        BroadcastEvent(data);
    }
    
    public void OnStartingGenerator(ActivatingGeneratorEventArgs ev)
    {
        StartingGeneratorEventData data = new StartingGeneratorEventData(ev);
        BroadcastEvent(data);
    }
    
    public void OnStoppingGenerator(StoppingGeneratorEventArgs ev)
    {
        StoppingGeneratorEventData data = new StoppingGeneratorEventData(ev);
        BroadcastEvent(data);
    }


    public void Register()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Player.Spawned += OnSpawned;
        Exiled.Events.Handlers.Player.Kicked += OnKicked;
        Exiled.Events.Handlers.Player.Banned += OnBanned;
        Exiled.Events.Handlers.Player.Joined += OnJoined;
        Exiled.Events.Handlers.Player.Left += OnLeft;
        Exiled.Events.Handlers.Player.Hurt += OnHurt;
        Exiled.Events.Handlers.Player.Died += OnDied;
        Exiled.Events.Handlers.Player.ChangedItem += OnChangedItem;
        Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        Exiled.Events.Handlers.Server.RoundEnded += OnRoundEnded;
        Exiled.Events.Handlers.Server.RespawningTeam += OnRespawningTeam;
        Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestarting;
        Exiled.Events.Handlers.Map.Generated += OnGenerated;
        Exiled.Events.Handlers.Warhead.Starting += OnStarting;
        Exiled.Events.Handlers.Warhead.Stopping += OnStopping;
        Exiled.Events.Handlers.Warhead.Detonated += OnDetonated;
        Exiled.Events.Handlers.Map.GeneratorActivating += OnEngagingGenerator;
        Exiled.Events.Handlers.Player.ActivatingGenerator += OnStartingGenerator;
        Exiled.Events.Handlers.Player.StoppingGenerator += OnStoppingGenerator;
        
        PluginAPI.Events.EventManager.RegisterEvents(this);
        
    }

    public void Unregister()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
        Exiled.Events.Handlers.Player.Kicked -= OnKicked;
        Exiled.Events.Handlers.Player.Banned -= OnBanned;
        Exiled.Events.Handlers.Player.Joined -= OnJoined;
        Exiled.Events.Handlers.Player.Left -= OnLeft;
        Exiled.Events.Handlers.Player.Hurt -= OnHurt;
        Exiled.Events.Handlers.Player.Died -= OnDied;
        Exiled.Events.Handlers.Player.ChangedItem -= OnChangedItem;
        Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        Exiled.Events.Handlers.Server.RoundEnded -= OnRoundEnded;
        Exiled.Events.Handlers.Server.RespawningTeam -= OnRespawningTeam;
        Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestarting;
        Exiled.Events.Handlers.Map.Generated -= OnGenerated;
        Exiled.Events.Handlers.Warhead.Starting -= OnStarting;
        Exiled.Events.Handlers.Warhead.Stopping -= OnStopping;
        Exiled.Events.Handlers.Warhead.Detonated -= OnDetonated;
        Exiled.Events.Handlers.Map.GeneratorActivating -= OnEngagingGenerator;
        Exiled.Events.Handlers.Player.ActivatingGenerator -= OnStartingGenerator;
        Exiled.Events.Handlers.Player.StoppingGenerator -= OnStoppingGenerator;
        
        PluginAPI.Events.EventManager.UnregisterEvents(this);
        
    }
}