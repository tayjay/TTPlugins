using System.Collections.Generic;
using Exiled.Events.EventArgs.Server;

namespace TTAdmin.Data.Events;

public class RespawningTeamEventData : EventData
{
    public override string EventName => "respawning_team";
    
    public List<PlayerData> Players { get; set; }
    public string Team { get; set; }
    public List<string> SpawnQueue { get; set; }

    public RespawningTeamEventData(RespawningTeamEventArgs ev)
    {
        Players = PlayerData.ConvertList(ev.Players);
        Team = ev.NextKnownTeam.ToString();
        SpawnQueue = new List<string>();
        foreach (var spawn in ev.SpawnQueue)
        {
            SpawnQueue.Add(spawn.ToString());
        }
    }
}