using System.Collections.Generic;
using System.IO;
using System.Net;
using System.Text;
using System.Threading;
using Exiled.API.Features;
using TTAdmin.WebNew.DataTypes;
using TTAdmin.WebNew.Handlers;
using TTAdmin.WebNew.Handlers.Player;
using Utf8Json;
using Server = TTAdmin.WebSocketServer.Server;

namespace TTAdmin.WebNew;

public class Program
{
    private static int playerCount = 10; // Example player count
    private static bool running = true;
    private static List<IRequestHandler> handlers;
    
    private static Server WebSocketServer { get; set; }

    public static void Main()
    {
        handlers = new List<IRequestHandler>
        {
            new GetPlayerCountHandler(),
            new ExecuteCommandHandler(),
            new GetTpsHandler(),
            new GetCASSIEWordsRequest(),
            new GetPlayerInfoRequest(),
            new GetAllPlayerInfoRequest(),
            new RoundActionRequest(),
            new GetFacilityRequest(),
            new GetRoomInfoRequest(),
            new GetRoundInfoRequest(),
            new LobbyActionRequest()
        };
        
        var httpThread = new Thread(StartHttpServer);
        httpThread.Start();

        Log.Info("HTTP server running...");
        //Console.ReadLine();
        //running = false;
        //httpThread.Join();
        
        /*notificationServer = new WebSocketNotificationServer("ws://localhost:8081/");
        cts = new CancellationTokenSource();
        _ = notificationServer.StartAsync(cts.Token);*/

        WebSocketServer = new Server(new IPEndPoint(IPAddress.Parse("127.0.0.1"), 8081));
        WebSocketServer.OnClientConnected += (sender, client) =>
        {
            Log.Info($"Client connected: {client.GetClient().GetGuid()}");
            WebSocketServer.SendMessage(client.GetClient(), JsonSerializer.ToJsonString(new Dictionary<string, object>()
            {
                {"response", "Oh Hello, client!"}
            }));
        };
        
        WebSocketServer.OnClientDisconnected += (sender, client) =>
        {
            Log.Info($"Client disconnected: {client.GetClient().GetGuid()}");
            TTAdmin.Instance.SubscriptionHandler.ClearSubscriptions(client.GetClient());
        };
        
        WebSocketServer.OnMessageReceived += (sender, message) =>
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
        
        WebSocketServer.OnSendMessage += (sender, message) =>
        {
            Log.Info($"Message sent: {message.GetMessage()}");
        };

    }

    private static void StartHttpServer()
    {
        HttpListener listener = new HttpListener();
        listener.Prefixes.Add($"http://+:{TTAdmin.Instance.Config.RestPort}/");
        listener.Start();
        Log.Info("HTTP server started on http://localhost:8080/");

        while (running)
        {
            var context = listener.GetContext();
            ThreadPool.QueueUserWorkItem(o => HandleRequest(context));
        }

        listener.Stop();
        Log.Info("HTTP server stopped.");
    }

    private static void HandleRequest(HttpListenerContext context)
    {
        foreach (var handler in handlers)
        {
            if (handler.CanHandle(context.Request))
            {
                handler.Handle(context);
                return;
            }
        }

        // Default response for unhandled requests
        new Response()
        {
            StatusCode = HttpStatusCode.BadRequest,
            ContentType = "text/plain",
            Message = "Invalid request"
        }.Send(context.Response);
        /*context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
        byte[] buffer = Encoding.UTF8.GetBytes("Invalid request");
        context.Response.ContentType = "text/plain";
        context.Response.ContentLength64 = buffer.Length;
        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
        context.Response.OutputStream.Close();*/
    }
}