using PluginAPI.Core;

namespace TTAdmin.Data;

public class RoundData
{
    public bool IsRoundStarted { get; set; }
    public bool IsRoundEnded { get; set; }
    public bool IsLocked { get; set; }
    public bool IsLobbyLocked { get; set; }
    public double RoundTime { get; set; }
    
    public RoundData()
    {
        IsRoundStarted = Round.IsRoundStarted;
        IsRoundEnded = Round.IsRoundEnded;
        IsLocked = Round.IsLocked;
        IsLobbyLocked = Round.IsLobbyLocked;
        RoundTime = Round.Duration.TotalSeconds;
    }
}