using System.Linq;
using Exiled.API.Features;
using UnityEngine;

namespace TTCore.Utilities;

public class RoomUtils
{
    /// <summary>
    /// Destroys the specified room
    /// </summary>
    /// <remarks>
    /// This method will not destroy the 079 components (cameras, speakers) in room
    /// </remarks>
    /// <param name="room">Room that should be destroyed</param>
    public static void DestroyRoom(Room room)
    {
        foreach (var component in room.GameObject.GetComponentsInChildren<Component>())
        {
            try
            {
                if (component.name.Contains("SCP-079") || component.name.Contains("CCTV"))
                {
                    Log.Debug($"Prevent from destroying: {component.name} {component.tag} {component.GetType().FullName}");
                    continue;
                }

                if (component.GetComponentsInParent<Component>()
                    .Any(c => c.name.Contains("SCP-079") || c.name.Contains("CCTV")))
                {
                    Log.Debug($"Prevent from destroying: {component.name} {component.tag} {component.GetType().FullName}");
                    continue;
                }
                
                Log.Debug($"Destroying component: {component.name} {component.tag} {component.GetType().FullName}");
                
                Object.Destroy(component);
            }
            catch
            {
                // ignored
            }
        }
    }
}