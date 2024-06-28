using Exiled.API.Features;

namespace TTAdmin.Data;

public class WarheadData
{
    public string Status { get; set; }
    public float Time { get; set; }
    public bool LeverStatus { get; set; }
    

    public WarheadData()
    {
        Status = Warhead.Status.ToString();
        Time = Warhead.DetonationTimer;
        LeverStatus = Warhead.LeverStatus;
    }
}