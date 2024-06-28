using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Server;

namespace TTAdmin.Data.Events;

public class RoundEndedEventData : EventData
{
    public override string EventName => "round_ended";
    public string LeadingTeam { get; set; }
    public int ClassD { get; set; }
    public int Scientists { get; set; }
    public int ChaosInsurgents { get; set; }
    public int Foundation { get; set; }
    public int SCPs { get; set; }
    public int Zombies { get; set; }
    public int WarheadKills { get; set; }
    public List<PlayerData> AlivePlayers { get; set; }
    
    public RoundEndedEventData(RoundEndedEventArgs ev)
    {
        LeadingTeam = ev.LeadingTeam.ToString();
        ClassD = ev.ClassList.class_ds;
        Scientists = ev.ClassList.scientists;
        ChaosInsurgents = ev.ClassList.chaos_insurgents;
        Foundation = ev.ClassList.mtf_and_guards;
        SCPs = ev.ClassList.scps_except_zombies;
        Zombies = ev.ClassList.zombies;
        WarheadKills = ev.ClassList.warhead_kills;
        AlivePlayers = PlayerData.ConvertList(Player.Get(p => p.IsAlive));
    }
}