using System.ComponentModel;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.API;
using UnityEngine;

namespace RoundModifiers.Modifiers;

public class BigWorld : Modifier
{

    public static Config BigWorldConfig => RoundModifiers.Instance.Config.BigWorld;

    public void OnSpawned(SpawnedEventArgs ev)
    {
        if (ev.Player.Role.IsAlive)
        {
            ev.Player.Scale = Vector3.one * BigWorldConfig.Scale;
        }
    }
    
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.Spawned += OnSpawned;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "BigWorld",
        Aliases = new []{"miniplayers"},
        Description = "All players are tiny!",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false,
        Balance = 1,
        Category = Category.Scale,
        FormattedName = "<color=green>Big World</color>"
    };

    public class Config
    {
        [Description("The scale of the players. Default is 0.4.")]
        public float Scale { get; set; } = 0.4f;
    }
}