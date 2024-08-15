using AdminToys;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Interfaces;
using UnityEngine;

namespace TTCore.Events.EventArgs;

public class AdminToyCollisionEventArgs : IExiledEvent
{
    public AdminToyBase AdminToy { get; set; }
    public Player Player { get; set; }
    public GameObject GameObject { get; set; }
    
    public AdminToyCollisionEventArgs(AdminToyBase adminToy, GameObject gameObject)
    {
        AdminToy = adminToy;
        GameObject = gameObject;
        Player = Player.Get(gameObject);
        
    }
}