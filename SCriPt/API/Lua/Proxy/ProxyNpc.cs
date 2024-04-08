using Exiled.API.Features;

namespace SCriPt.API.Lua.Proxy
{
    public class ProxyNpc : ProxyPlayer
    {
        private Npc Npc { get; }
        
        public ProxyNpc(Npc player) : base(player)
        {
            Npc = player;
        }
        
        public void LookAtPlayer(Player player)
        {
            //Npc.LookAtPlayer(player);
        }
    }
}