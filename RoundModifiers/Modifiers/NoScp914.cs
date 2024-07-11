using System;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Scp914;
using MapGeneration;
using Mirror;
using RoundModifiers.API;
using UnityEngine;
using Object = UnityEngine.Object;

namespace RoundModifiers.Modifiers
{
    public class NoScp914 : Modifier
    {
        public void OnActivatingScp914(ActivatingEventArgs ev)
        {
            ev.Player.ShowHint("SCP-914 is out of order.");
            ev.IsAllowed = false;
        }

        public void OnRoundStart()
        {
            Room.Get(RoomType.Lcz914).RoomLightController.NetworkOverrideColor = Color.red;
        }

        public void OnWaitingForPlayers()
        {
            //DestroyRoom(Room.Get(RoomType.Lcz914));
        }

        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Scp914.Activating += OnActivatingScp914;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
            
            //SeedSynchronizer.OnMapGenerated += OnMapGenerated;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Scp914.Activating -= OnActivatingScp914;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
            
            //SeedSynchronizer.OnMapGenerated -= OnMapGenerated;
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "NoScp914",
            FormattedName = "<color=purple>No Scp-914</color>",
            Aliases = new []{"no914"},
            Description = "SCP-914 is out of order.",
            Impact = ImpactLevel.MinorGameplay,
            MustPreload = false,
            Balance = -2,
            Category = Category.Scp914
        };
    }
}