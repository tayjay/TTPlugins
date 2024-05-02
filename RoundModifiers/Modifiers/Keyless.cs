using System.Collections.Generic;
using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Doors;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp079;
using PlayerRoles;
using PlayerRoles.RoleAssign;
using PluginAPI.Events;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers;

public class Keyless : Modifier
{
    public void OnPlayerSpawned(SpawnedEventArgs ev)
    {
        ev.Player.IsBypassModeEnabled = true;
    }

    public void OnRoundStart()
    {
        IEnumerable<Player> computers = Player.Get(RoleTypeId.Scp079);
        if (computers.Count() == 0) return;
        foreach (Player computer in computers)
        {
            computer.RoleManager.ServerSetRole(ScpSpawner.NextScp,RoleChangeReason.RoundStart);
        }
    }

    public void OnInteractingDoor(InteractingDoorEventArgs ev)
    {
        //Prevent people from entering 079 prematurely
        if (ev.Door is Gate gate)
        {
            if (gate.Room.Type == RoomType.Hcz079)
            {
                if (Generator.Get(GeneratorState.Engaged).Count() != 3)
                {
                    ev.IsAllowed = false;
                }
            }
                
        }
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.Spawned += OnPlayerSpawned;
        //Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
        Exiled.Events.Handlers.Player.InteractingDoor += OnInteractingDoor;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.Spawned -= OnPlayerSpawned;
        //Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
        Exiled.Events.Handlers.Player.InteractingDoor -= OnInteractingDoor;
    }

    public override ModInfo ModInfo { get; } = new ModInfo()
    {
        Name = "Keyless",
        Aliases = new[] { "nokey" },
        Description = "Everything is unlocked.",
        FormattedName = "<color=yellow>Keyless</color>",
        Impact = ImpactLevel.MajorGameplay,
        MustPreload = false
    };
}