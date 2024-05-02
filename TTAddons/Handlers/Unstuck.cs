using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.Events.EventArgs.Player;
using MEC;
using PlayerRoles;
using TTAddons.Commands.Client;
using TTCore.API;
using UnityEngine;
using Utils.NonAllocLINQ;

namespace TTAddons.Handlers
{
    public class Unstuck : IRegistered
    {

        public List<Player> playerCanUnstuck { get; private set; }
        
        public Unstuck()
        {
            
        }

        public bool DoUnstuck(Player player, out string response)
        {
            Log.Info("Attempting to Unstuck.");
            if (player == null)
            {
                response = "Player not found.";
                return false;
            }
            
            if (!player.IsScp)
            {
                response = "You must be an SCP to use this command.";
                return false;
            }
            
            if (!playerCanUnstuck.Contains(player))
            {
                response = "Unstuck has expired.";
                return false;
            }

            //Do logic to check how far into a round we are.
            
            Room spawnRoom = null;
            if (player.Role == RoleTypeId.Scp049)
            {
                spawnRoom = Room.Get(RoomType.Hcz049);
            } else if (player.Role == RoleTypeId.Scp096)
            {
                spawnRoom = Room.Get(RoomType.Hcz096);
            } else if (player.Role == RoleTypeId.Scp106)
            {
                spawnRoom = Room.Get(RoomType.Hcz106);
            } else if (player.Role == RoleTypeId.Scp173)
            {
                spawnRoom = Room.Get(RoomType.Hcz049);
            } else if (player.Role == RoleTypeId.Scp939)
            {
                spawnRoom = Room.Get(RoomType.Hcz939);
            } else if (player.Role == RoleTypeId.Scp3114)
            {
                spawnRoom = Room.Get(RoomType.Lcz173);
            }
            
            if (spawnRoom != null)
            {
                player.Position = spawnRoom.Position + Vector3.up;
                if (player.Role == RoleTypeId.Scp3114)
                    player.Position += Vector3.up * 12;//Change this if it's too high
                if (player.Role == RoleTypeId.Scp096)
                {
                    Vector3 offset = (spawnRoom.Rotation) * (new Vector3(-5f, 0f, 0f));
                    player.Position += offset;
                }

                playerCanUnstuck.Remove(player);
                response = "You have been unstuck.";
                return true;
            }

            response = "Could not find a spawn room.";
            return false;

        }
        
        
        public void OnSpawned(SpawnedEventArgs ev)
        {
            if (ev.Player.Role.Team == Team.SCPs && TTAddons.Instance.Config.AllowUnstuckForScps)
            {
                
                playerCanUnstuck.AddIfNotContains(ev.Player);
                Timing.CallDelayed(5f, () =>
                {
                    ev.Player.ShowHint("If you are stuck, press ~ and type .unstuck to teleport to your spawn room.",
                        30f);
                    Timing.CallDelayed(30f, () =>
                    {
                        playerCanUnstuck.Remove(ev.Player);
                    });
                });
            }
        }
        
        public void Register()
        {
            Exiled.Events.Handlers.Player.Spawned += OnSpawned;
            playerCanUnstuck = ListPool<Player>.Pool.Get();
        }
        
        public void Unregister()
        {
            Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
            
            ListPool<Player>.Pool.Return(playerCanUnstuck);
        }
    }
}