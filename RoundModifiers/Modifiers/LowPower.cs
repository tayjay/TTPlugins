using RoundModifiers.API;
using UnityEngine;

namespace RoundModifiers.Modifiers;

public class LowPower : Modifier
{

    public void OnRoundStart()
    {
        foreach (RoomLightController instance in RoomLightController.Instances)
        {
            instance.NetworkOverrideColor =
                new Color(0.1f, 0.1f, 0.1f, 0.1f);
        }
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
    }

    public override ModInfo ModInfo => new ModInfo()
    {
        Name = "LowPower",
        FormattedName = "Low Power",
        Aliases = new[] {"LP"},
        Description = "Lights are dimmed",
        Impact = ImpactLevel.MinorGameplay,
        MustPreload = false,
        Balance = -1,
        Category = Category.Lights | Category.Visual
    };
}