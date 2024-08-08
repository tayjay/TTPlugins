using System.Collections.Generic;
using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles.RoleAssign;
using TTCore.API;

namespace TTAddons.Handlers
{
    // Handing that allows a player to choose whether if they want to be an SCP or Human this round
    public class RoleSelectorHandler : IRegistered
    {
        private Dictionary<string,int> _tickets = new Dictionary<string, int>();

        // Reset player points to what they were before
        public void OnRoundStart()
        {
            using (ScpTicketsLoader loader = new ScpTicketsLoader())
            {
                foreach(string playerId in _tickets.Keys)
                {
                    Player player = Player.Get(playerId);
                    if(player == null) continue;
                    if (player.Role.Side == Side.Scp) continue; // Don't reset SCPs
                    loader.ModifyTickets(player.ReferenceHub, _tickets[playerId]+2);
                }
            }
            _tickets.Clear();
        }

        public void OnLeft(LeftEventArgs ev)
        {
            using (ScpTicketsLoader loader = new ScpTicketsLoader())
            {
                if (_tickets.ContainsKey(ev.Player.UserId))
                {
                    loader.ModifyTickets(ev.Player.ReferenceHub, _tickets[ev.Player.UserId]);
                }
            }
        }
        
        // Save player points
        public bool SetSCP(Player player)
        {
            using (ScpTicketsLoader loader = new ScpTicketsLoader())
            {
                
                if (_tickets.TryGetValue(player.UserId, out var ticket))
                {
                    if (ticket <= 5)
                    {
                        return false;
                    }
                    loader.ModifyTickets(player.ReferenceHub, ticket);
                } else if (loader.GetTickets(player.ReferenceHub, 10) <= 4)
                {
                    // Was an SCP too recently, every round a player isn't an SCP they get 2 tickets
                    return false;
                }
                _tickets[player.UserId] = loader.GetTickets(player.ReferenceHub,10);
                loader.ModifyTickets(player.ReferenceHub, _tickets[player.UserId] + 10);
                Log.Info($"Player {player.Nickname} now has {loader.GetTickets(player.ReferenceHub,10)} SCP tickets.");
            }
            return true;
        }
        
        public void SetHuman(Player player)
        {
            using (ScpTicketsLoader loader = new ScpTicketsLoader())
            {
                if (_tickets.ContainsKey(player.UserId))
                {
                    loader.ModifyTickets(player.ReferenceHub, _tickets[player.UserId]);
                }
                _tickets[player.UserId] = loader.GetTickets(player.ReferenceHub,10);
                loader.ModifyTickets(player.ReferenceHub, 0);
                Log.Info($"Player {player.Nickname} now has {loader.GetTickets(player.ReferenceHub,10)} SCP tickets.");
            }
        }

        public void Register()
        {
            Exiled.Events.Handlers.Server.RoundStarted += OnRoundStart;
            Exiled.Events.Handlers.Player.Left += OnLeft;
        }

        public void Unregister()
        {
            Exiled.Events.Handlers.Server.RoundStarted -= OnRoundStart;
            Exiled.Events.Handlers.Player.Left -= OnLeft;
        }
    }
}