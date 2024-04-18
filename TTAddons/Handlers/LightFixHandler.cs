using System.Linq;
using Exiled.API.Enums;
using Exiled.API.Features;
using TTCore.API;
using UnityEngine;

namespace TTAddons.Handlers
{
    public class LightFixHandler : IRegistered
    {

        public void OnWaitingForPlayers()
        {
            Room room = Room.Get(RoomType.LczToilets);
            foreach (Component c in room.gameObject.GetComponentsInChildren<Component>())
            {
                if(c==null || c.name == null) continue;
                Log.Info(c.gameObject.name);
                if (c.name.ToLower().Contains("light"))
                {
                    Log.Info("Destroying: " + c.name);
                    Object.Destroy(c);
                }
            }
        }
        
        public void Register()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers += OnWaitingForPlayers;
        }

        public void Unregister()
        {
            Exiled.Events.Handlers.Server.WaitingForPlayers -= OnWaitingForPlayers;
        }
    }
}