namespace TTAdmin.Data;

public class Scp914Data
{
    
    public string ConfigMode { get; set; }
    public string KnobStatus { get; set; }
    public bool IsWorking { get; set; }
    
    public Scp914Data()
    {
        ConfigMode = Exiled.API.Features.Scp914.ConfigMode.ToString();
        KnobStatus = Exiled.API.Features.Scp914.KnobStatus.ToString();
        IsWorking = Exiled.API.Features.Scp914.IsWorking;
    }
}