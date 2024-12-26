using System.ComponentModel;
using Exiled.Events.EventArgs.Map;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.API;
using TTCore.Events.EventArgs;
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
    
    /*public void OnAccessBaseStats(AccessFirearmBaseStatsEventArgs ev)
    {
        float hipInaccuracy = ev.BaseStats.HipInaccuracy;
        float adsInaccuracy = ev.BaseStats.AdsInaccuracy;
        ev.BaseStats = ev.BaseStats with
        {
            HipInaccuracy = hipInaccuracy * BigWorldConfig.Scale,
            AdsInaccuracy = adsInaccuracy * BigWorldConfig.Scale
        };
    }*/
    
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.Spawned += OnSpawned;
        //TTCore.Events.Handlers.Custom.AccessFirearmBaseStats += OnAccessBaseStats;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
        //TTCore.Events.Handlers.Custom.AccessFirearmBaseStats -= OnAccessBaseStats;
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