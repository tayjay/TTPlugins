using LightContainmentZoneDecontamination;

namespace TTAdmin.Data;

public class LczDecontaminationData
{
    
    public bool IsDecontaminating { get; set; }
    public string DecontaminationOverride { get; set; }
    public string DecontaminationStatus { get; set; }

    public LczDecontaminationData()
    {
        IsDecontaminating = DecontaminationController.Singleton.IsDecontaminating;
        DecontaminationOverride = DecontaminationController.Singleton.DecontaminationOverride.ToString();
        DecontaminationStatus = DecontaminationController.Singleton.NetworkDecontaminationOverride.ToString();
    }
}