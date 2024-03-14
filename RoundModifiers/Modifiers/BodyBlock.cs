using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Toys;
using Exiled.API.Structs;
using Exiled.Events.EventArgs.Player;
using MEC;
using PluginAPI.Events;
using RoundModifiers.API;
using UnityEngine;

namespace RoundModifiers.Modifiers
{
    public class BodyBlock : Modifier
    {
        public Dictionary<Player, Primitive> Primitives = new Dictionary<Player, Primitive>();

        public void OnRoundStart()
        {
            Timing.RunCoroutine(UpdatePrimitives());
        }
        
        protected IEnumerator<float> UpdatePrimitives()
        {
            while (true)
            {
                foreach (var primitive in Primitives)
                {
                    primitive.Value.Position = primitive.Key.Position;
                }
                yield return Timing.WaitForOneFrame;
            }
        }
        
        public void OnPlayerSpawned(SpawnedEventArgs ev)
        {
            if(Primitives.ContainsKey(ev.Player)) Primitives[ev.Player].Destroy();
            CharacterController controller = ev.Player.GameObject.GetComponent<CharacterController>();
            Primitives[ev.Player] = (Primitive.Create(new PrimitiveSettings(PrimitiveType.Capsule, UnityEngine.Color.blue, ev.Player.Position, UnityEngine.Vector3.zero, new Vector3(controller.radius*3.5f,controller.height/2,controller.radius*3.5f), true)));
        }

        public void OnPlayerDeath(DiedEventArgs ev)
        {
            if (Primitives.ContainsKey(ev.Player))
            {
                Primitives[ev.Player].Destroy();
                Primitives.Remove(ev.Player);
            }
        }
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Player.Spawned += OnPlayerSpawned;
            Exiled.Events.Handlers.Player.Died += OnPlayerDeath;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Player.Spawned -= OnPlayerSpawned;
            Exiled.Events.Handlers.Player.Died -= OnPlayerDeath;
            
            foreach (var primitive in Primitives)
            {
                primitive.Value.Destroy();
            }
            Primitives.Clear();
        }

        public override ModInfo ModInfo { get; } = new ModInfo() 
        {
            Name = "BodyBlock",
            FormattedName = "<color=blue>BodyBlock</color>",
            Aliases = new[] {"bb"},
            Description = "Players have collision",
            Impact = ImpactLevel.MinorGameplay,
            MustPreload = false
        };
    }
}