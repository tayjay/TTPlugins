﻿using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.API.Features.Toys;
using Exiled.Events.EventArgs.Player;
using InventorySystem;
using MapGeneration;
using MEC;
using PlayerRoles;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers
{
    public class Blackout : Modifier
    {
        
        
        private CoroutineHandle _blackoutTick;
        
        public void OnRoundStart()
        {
            
            Timing.CallDelayed(2f, () =>
            {
                Overcharge(FacilityZone.LightContainment, float.MaxValue);
                Overcharge(FacilityZone.HeavyContainment, float.MaxValue);
                Overcharge(FacilityZone.Entrance, float.MaxValue);
                Overcharge(FacilityZone.Surface, float.MaxValue);
            });
            
            _blackoutTick = Timing.RunCoroutine(BlackoutTick());
            
        }
        
        public void OnTriggeringTesla(TriggeringTeslaEventArgs ev)
        {
            if(ev.Tesla.Room.AreLightsOff)
                ev.IsAllowed = false;
        }

        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role.Team != Team.SCPs && ev.Player.Role.Team != Team.Dead && ev.Reason != SpawnReason.Escaped)
            {
                Timing.CallDelayed(2f, () =>
                {
                    //ev.Player.ShowHint("The lights are out! You have been given a light.");
                    if (ev.Player.Role.Type == RoleTypeId.ClassD)
                    {
                        Log.Debug("Giving " + ev.Player.Nickname + " a lantern.");
                        ev.Player.CurrentItem = Exiled.API.Features.Items.Item.Create(ItemType.Lantern);
                    }
                    else
                    {
                        Inventory inv = ev.Player.Inventory;
                        Log.Debug("Giving " + ev.Player.Nickname + " a flashlight.");
                        ev.Player.CurrentItem = Exiled.API.Features.Items.Item.Create(ItemType.Flashlight); //Forces Flashlight into hands
                    }
                });
            }
            
        }
        
        //Pick a random room and turn the lights on for 10 seconds, then turn them back off.
        public IEnumerator<float> BlackoutTick()
        {
            while (true)
            {
                yield return Timing.WaitForSeconds(1f);
                Room room = Room.Random();
                if (room == null) continue;
                OverchargeRoom(room, 1f);
                yield return Timing.WaitForSeconds(LightRollDuration);
                OverchargeRoom(room, float.MaxValue);
            }
        }

        public void Overcharge(FacilityZone zoneToAffect, float duration)
        {
            bool flag = zoneToAffect != 0;
            foreach (RoomLightController instance in RoomLightController.Instances)
            {
                if (!flag || instance.Room.Zone == zoneToAffect)
                    instance.ServerFlickerLights(duration);
            }
        }

        public void OverchargeRoom(Room room, float duration)
        {
            if(room == null) return;
            RoomLightController lightController = room.RoomLightController;
            if (lightController == null)
                return;
            lightController.ServerFlickerLights(duration);
        }

        public bool HasLightItem(Player player)
        {
            //Check held items
            foreach (Item item in player.Items)
            {
                if (item.IsLightEmitter)
                    return true;
                if (item is Firearm firearm)
                {
                    if(firearm.FlashlightEnabled)
                        return true;
                }
            }

            return false;
        }
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Player.TriggeringTesla += OnTriggeringTesla;
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Player.TriggeringTesla -= OnTriggeringTesla;
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
            
            Timing.KillCoroutines(_blackoutTick);
        }

        public override ModInfo ModInfo { get; } = new ModInfo()
        {
            Name = "Blackout",
            FormattedName = "<color=#444444>Blackout</color>",
            Description = "The lights are out!",
            Aliases = new [] {"bo"},
            Impact = ImpactLevel.MinorGameplay,
            MustPreload = false,
            Balance = -2,
            Category = Category.Lights
        };
        
        public Config BlackoutConfig => RoundModifiers.Instance.Config.Blackout;
        
        public float LightRollDuration => BlackoutConfig.LightRollDuration;

        public class Config
        {
            [Description("The duration of each light roll. Default is 10.")]
            public float LightRollDuration { get; set; } = 10f;
        }
    }
}