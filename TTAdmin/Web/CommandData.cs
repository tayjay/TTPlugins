using System.Collections.Generic;

namespace TTAdmin.Web
{
    public class CommandData : IJsonSerializable
    {
        public string UserId { get; set; }
        public List<string> Commands { get; set; }
    }
}