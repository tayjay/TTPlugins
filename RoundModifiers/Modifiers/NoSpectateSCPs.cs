using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.API;

namespace RoundModifiers.Modifiers;

public class NoSpectateSCPs : Modifier
{

    public void OnSpawned(SpawnedEventArgs ev)
    {
        /*if (ev.Player.Role.Side == Side.Scp)
        {
            ev.Player.ReferenceHub.authManager.NetworkSyncedUserId = null;
        }
        else
        {
            ev.Player.ReferenceHub.authManager.NetworkSyncedUserId = ev.Player.ReferenceHub.authManager._privUserId;
            
        }*/
    }
    
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.Spawned += OnSpawned;
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
        /*foreach(Player player in Player.List)
        {
            if (player.ReferenceHub.authManager.NetworkSyncedUserId == null)
            {
                player.ReferenceHub.authManager.NetworkSyncedUserId = player.ReferenceHub.authManager._privUserId;
            }
        }*/
    }

    public override ModInfo ModInfo { get; } = new()
    {
        Name = "NoSpectateSCPs",
        Aliases = new[] { "nospectate" },
        Description = "Prevents players from spectating SCPs.",
        Category = Category.ScpRole,
        Balance = -3,
        Impact = ImpactLevel.MajorGameplay,
        FormattedName = "No Spectate SCPs",
    };
}