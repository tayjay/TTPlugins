using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;

namespace TTAdmin.Data;

public class FacilityData
{
    public string Seed { get; set; }
    public List<RoomData> Rooms { get; set; }
    public WarheadData Warhead { get; set; }
    public LczDecontaminationData LczDecontamination { get; set; }
    public Scp914Data Scp914 { get; set; }
    public List<GeneratorData> Generators { get; set; }
    
    public FacilityData()
    {
        Rooms = RoomData.ConvertList(Room.List.ToList());
        Seed = Map.Seed.ToString();
        Warhead = new WarheadData();
        LczDecontamination = new LczDecontaminationData();
        Scp914 = new Scp914Data();
        Generators = new List<GeneratorData>();
        foreach(Generator generator in Generator.List)
        {
            Generators.Add(new GeneratorData(generator));
        }
    }
}