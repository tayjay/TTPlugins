using Exiled.API.Features;
using UnityEngine;

namespace TTAdmin.Data;

public class LiftData
{
    public string Name { get; set; }
    public string Status { get; set; }
    public string Type { get; set; }
    public Vector3 Position { get; set; }
    public Quaternion Rotation { get; set; }
    
    public LiftData(Lift lift)
    {
        Name = lift.Name;
        Status = lift.Status.ToString();
        Type = lift.Type.ToString();
        Position = lift.Position;
        Rotation = lift.Rotation;
    }
}