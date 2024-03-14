using System;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Exiled.API.Features;

namespace TTAdmin.Web
{
    public class GameServerClient
    {
        private ClientWebSocket _client;
        private readonly Uri _serverUri;
        
        public GameServerClient(string serverUri)
        {
            _client = new ClientWebSocket();
            _serverUri = new Uri($"ws://{serverUri}");
        }
        
        public async Task ConnectAsync()
        {
            await _client.ConnectAsync(_serverUri, CancellationToken.None);
            await Task.WhenAll(SendAsync(), ReceiveAsync());
        }
        
        private async Task SendAsync()
        {
            // Example: Send server status updates
            while (_client.State == WebSocketState.Open)
            {
                var serverUpdateData = new ServerUpdateData
                {
                    Online = true,
                    PlayerCount = Server.PlayerCount,
                    MaxPlayers = Server.MaxPlayerCount,
                    Ip = Server.IpAddress,
                    PlayerNames = Player.List.ToList().ConvertAll(p => p.Nickname),
                    PlayerIds = Player.List.ToList().ConvertAll(p => p.Id),
                    AdminId = "1234@discord"
                };
                string jsonData = JsonSerialize.ToJson(serverUpdateData);
                byte[] encodedData = Encoding.UTF8.GetBytes(jsonData);
                
                await _client.SendAsync(new ArraySegment<byte>(encodedData), WebSocketMessageType.Text, true, CancellationToken.None);
                await Task.Delay(1000); // Adjust delay as needed
            }
        }

        private async Task ReceiveAsync()
        {
            byte[] buffer = new byte[1024]; 
            while (_client.State == WebSocketState.Open)
            {
                var receiveResult = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (receiveResult.MessageType == WebSocketMessageType.Text)
                {
                    CommandData command = JsonSerialize.FromJson<CommandData>(Encoding.UTF8.GetString(buffer, 0, receiveResult.Count));
                    ProcessCommand(command);
                }
            }
        }

        private void ProcessCommand(CommandData command)
        {
            // Your logic to process incoming commands from the server
        }
    }
}