using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;

namespace TTAdmin.Data;

public class FacilityData
{
    public List<RoomData> Rooms { get; set; }
    public string Seed { get; set; }
    
    public FacilityData()
    {
        Rooms = RoomData.ConvertList(Room.List.ToList());
        Seed = Map.Seed.ToString();
    }
}