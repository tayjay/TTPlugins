using Exiled.API.Features;
using UnityEngine;

namespace TTAdmin.Data;

public class GeneratorData
{
    
    public string Room { get; set; }
    public Vector3 Position { get; set; }
    public bool IsEngaged { get; set; }
    public string State { get; set; }
    
    public GeneratorData(Generator generator)
    {
        Room = generator.Room.Name;
        Position = generator.Position;
        IsEngaged = generator.IsEngaged;
        State = generator.State.ToString();
    }
}