using System.Collections.Generic;

namespace TTAdmin.Web
{
    public class ServerUpdateData : IJsonSerializable
    {
        public bool Online { get; set; }
        public int PlayerCount { get; set; }
        public int MaxPlayers { get; set; }
        public string Ip { get; set; }
        public List<string> PlayerNames { get; set; }
        public List<int> PlayerIds { get; set; }
        public string AdminId { get; set; }
    }
}