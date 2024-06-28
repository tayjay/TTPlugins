using Exiled.API.Features.Pickups;
using UnityEngine;

namespace TTAdmin.Data;

public class PickupData
{
    
    public int Serial { get; set; }
    public string Type { get; set; }
    public Vector3 Position { get; set; }
    
    public PickupData(Pickup pickup)
    {
        Serial = pickup.Serial;
        Type = pickup.Type.ToString();
        Position = pickup.Position;
    }
}