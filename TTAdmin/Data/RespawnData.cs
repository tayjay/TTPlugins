using Exiled.API.Features;

namespace TTAdmin.Data;

//todo: replace respawn data with 14.0 data
public class RespawnData
{
    public bool IsSpawning { get; set; }
    public float ChaosTickets { get; set; }
    public float NtfTickets { get; set; }
    public bool ProtectionEnabled { get; set; }
    public string NextKnownTeam { get; set; }
    public bool ProtectedCanShoot { get; set; }
    public float TimeUntilNextPhase { get; set; }

    public RespawnData()
    {
        IsSpawning = Respawn.IsSpawning;
        //ChaosTickets = Respawn.ChaosTickets;
        //NtfTickets = Respawn.NtfTickets;
        ProtectionEnabled = Respawn.ProtectionEnabled;
        //NextKnownTeam = Respawn.NextKnownTeam.ToString();
        ProtectedCanShoot = Respawn.ProtectedCanShoot;
        //TimeUntilNextPhase = Respawn.TimeUntilNextPhase;
    }
}