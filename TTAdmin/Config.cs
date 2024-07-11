using System;
using System.ComponentModel;
using Exiled.API.Interfaces;

namespace TTAdmin
{
    public class Config : IConfig
    {
        public bool IsEnabled { get; set; } = true;
        public bool Debug { get; set; } = false;

        //public string Host { get; set; } = "127.0.0.1";
        
        [Description("Whether to use SSL for the server.")]
        public bool UseSsl { get; set; } = false;
        
        [Description("Path to the certificate file.")]
        public string CertificatePath { get; set; } = "";
        
        [Description("Password for the certificate file.")]
        public string CertificatePassword { get; set; } = "";
        
        [Description("Whether to allow any command to be ran using POST /command.")]
        public bool AllowCommands { get; set; } = true;
        
        [Description("Whether to enable the REST server.")]
        public bool RestEnabled { get; set; } = false;
        
        [Description("Port for the REST server.")]
        public int RestPort { get; set; } = 8080;
        
        [Description("Whether to enable the WebSocket server.")]
        public bool WebSocketEnabled { get; set; } = false;
        
        [Description("Port for the WebSocket server.")]
        public int WebSocketPort { get; set; } = 8081;
        
        [Description("Size of the WebSocket buffer. Default is 1024.")]
        public int WsBufferSize { get; set; } = 1024;
        
        //[Description("API key for the WebSocket server. Delete the whole row to generate a new key."),Obsolete]
        //public string ApiKey { get; set; } = Guid.NewGuid().ToString();
        
        //public string ApiKeyPath { get; set; } = "/TTCore/TTAdmin/APIKey.json";
        
        [Description("Whether local admin console output should be subscribable via WebSocket. WIP")]
        public bool EnableConsoleOutput { get; set; } = false;
        
        [Description("How many months before the API key is considered old, and recommend regenerating a new one. Default is 6.")]
        public int ApiKeyWarningAge { get; set; } = 6;
    }
}