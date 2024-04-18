using System.Collections.Generic;
using Exiled.Events.EventArgs.Interfaces;
using PlayerRoles;

namespace TTCore.Events.EventArgs;

public class ChooseScpSpawnQueueEventArgs : IExiledEvent
{
    
    public List<RoleTypeId> ScpQueue { get; }
    public List<ReferenceHub> SelectedPlayers { get; }
    
    public ChooseScpSpawnQueueEventArgs(List<RoleTypeId> scpQueue, List<ReferenceHub> selectedPlayers)
    {
        ScpQueue = scpQueue;
        SelectedPlayers = selectedPlayers;
    }
}