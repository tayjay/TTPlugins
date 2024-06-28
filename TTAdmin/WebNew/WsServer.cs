using System.Collections.Generic;
using System.Net;
using System.Threading;
using Exiled.API.Features;
using TTAdmin.WebNew.Handlers;
using Utf8Json;
using Server = TTAdmin.WebSocketServer.Server;

namespace TTAdmin.WebNew;

public class WsServer
{
    public static Server Server { get; private set; }

    public static void Start()
    {
        if(TTAdmin.Instance.Config.WebSocketEnabled == false)
        {
            Log.Error("Websocket server is disabled by config.");
            return;
        }
        
        var wsThread = new Thread(StartWebSocketServer);
        wsThread.Start();
        
        Log.Info("Websocket server running...");
    }
    
    public static void StartWebSocketServer()
    {
        //IPAddress.Parse(TTAdmin.Instance.Config.Host)
        Server = new Server(new IPEndPoint(IPAddress.Any, TTAdmin.Instance.Config.WebSocketPort));
        Server.OnClientConnected += (sender, client) =>
        {
            Log.Info($"Client connected: {client.GetClient().GetGuid()}");
            Server.SendMessage(client.GetClient(), JsonSerializer.ToJsonString(new Dictionary<string, object>
            {
                {"message", "Welcome to the TTAdmin websocket server"}
            }));
        };
        
        Server.OnClientDisconnected += (sender, client) =>
        {
            Log.Info($"Client disconnected: {client.GetClient().GetGuid()}");
            TTAdmin.Instance.SubscriptionHandler.ClearSubscriptions(client.GetClient());
        };
        
        Server.OnMessageReceived += (sender, message) =>
        {
            Log.Info($"Message received: {message.GetMessage()}");
            //Parse out the Json message
            if(message.GetMessage()=="")
                return;
            var messageObject = JsonSerializer.Deserialize<Dictionary<string, object>>(message.GetMessage());
            
            if (messageObject.TryGetValue("subscribe", out var subscribe))
            {
                Log.Info(subscribe.ToString());
                if(subscribe is List<object> list)
                    TTAdmin.Instance.SubscriptionHandler.Subscribe(message.GetClient(),list);
            }
            if(messageObject.TryGetValue("unsubscribe", out var unsubscribe))
            {
                Log.Info(unsubscribe.ToString());
                if(unsubscribe is List<object> list)
                    TTAdmin.Instance.SubscriptionHandler.Unsubscribe(message.GetClient(),list);
            }
            
        };
        
        Server.OnSendMessage += (sender, message) =>
        {
            Log.Info($"Message sent: {message.GetMessage()}");
        };
        
    }
    
    public static void Stop()
    {
        Server.Stop();
    }
}