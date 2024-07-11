using System;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Extensions;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Cassie;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using InventorySystem;
using InventorySystem.Items;
using InventorySystem.Items.MarshmallowMan;
using InventorySystem.Items.Pickups;
using InventorySystem.Items.Usables.Scp330;
using Mirror;
using PluginAPI.Core.Items;
using RoundModifiers.API;
using TTCore.HUDs;
using TTCore.Utilities;
using UnityEngine;
using Item = Exiled.API.Features.Items.Item;
using Object = UnityEngine.Object;

namespace RoundModifiers.Modifiers
{
    public class Fun : Modifier
    {
        public string OldUnitName { get; set; } = "";
        public string NewUnitName { get; set; } = "";
        
        public void OnNTFAnnouncement(AnnouncingNtfEntranceEventArgs ev)
        {
            /*OldUnitName = ev.UnitName;
            ev.UnitName = Cassie.VoiceLines.Where(l => l.GetName().StartsWith(ev.UnitName.Substring(0, 1)))
                .GetRandomValue().GetName();
            NewUnitName = ev.UnitName;*/
        }

        public void OnCassieAnnouncement(SendingCassieMessageEventArgs ev)
        {
            /*if(ev.Words.ToLower().Contains(OldUnitName.ToLower()))
                ev.Words = ev.Words.ToLower().Replace(OldUnitName.ToLower(), NewUnitName.ToLower());*/
            //ev.Words = ".g7";
            Log.Info(ev.Words);
        }

        public void OnCreatePickup(DroppedItemEventArgs ev)
        {
            /*foreach (Component c in ev.Pickup.GameObject.GetComponentsInChildren<Component>())
            {
                //Log.Info($"Destroying component: {c.name} {c.tag} {c.GetType().FullName}");
                if (c != null)
                {
                    if (c.GetType().FullName.Contains("Rigidbody"))
                    {
                        Log.Info($"Destroying component: {c.name} {c.tag} {c.GetType().FullName}");
                        NetworkServer.Destroy(c.gameObject);
                        Object.Destroy(c);
                    }
                }
                
            }*/
        }

        public void OnSpawn(SpawnedEventArgs ev)
        {

            //ev.Player.AddItem((ItemType)55);
            //ev.Player.ShowHUDHint($"You have been given a marshmallow! {ev.Player.CurrentItem.Base.name}", 5);
            //Log.Info("Added marshmallow item to player inventory.");
        }
        
        public void OnRoundStart()
        {
            //Log.Info("Round started.");
            RoomUtils.DestroyRoom(Room.Get(RoomType.Lcz330));
            RoomUtils.DestroyRoom(Room.Get(RoomType.HczTestRoom));
        }
        
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance += OnNTFAnnouncement;
            Exiled.Events.Handlers.Cassie.SendingCassieMessage += OnCassieAnnouncement;
            Exiled.Events.Handlers.Player.DroppedItem += OnCreatePickup;
            Exiled.Events.Handlers.Player.Spawned += OnSpawn;
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Map.AnnouncingNtfEntrance -= OnNTFAnnouncement;
            Exiled.Events.Handlers.Cassie.SendingCassieMessage -= OnCassieAnnouncement;
            Exiled.Events.Handlers.Player.DroppedItem -= OnCreatePickup;
            Exiled.Events.Handlers.Player.Spawned -= OnSpawn;
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        }

        public override ModInfo ModInfo { get; } = new ModInfo() {
            Name = "Fun",
            FormattedName = "<color=white>Fun</color>",
            Aliases = new []{"fun"},
            Description = "For testing purposes only.",
            Impact = ImpactLevel.MinorGameplay,
            MustPreload = false
        };
    }
}