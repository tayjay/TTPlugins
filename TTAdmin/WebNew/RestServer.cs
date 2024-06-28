using System.Collections.Generic;
using System.Net;
using System.Threading;
using Exiled.API.Features;
using TTAdmin.WebNew.DataTypes;
using TTAdmin.WebNew.Handlers;
using TTAdmin.WebNew.Handlers.Player;
using TTAdmin.WebNew.Handlers.Scp914;

namespace TTAdmin.WebNew;

public class RestServer
{
    private static List<IRequestHandler> handlers;
    private static bool running = true;
    
    public static void Start()
    {
        if (!TTAdmin.Instance.Config.RestEnabled)
        {
            Log.Error("REST server is disabled by config.");
            return;
        }
        
        
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
            new LobbyActionRequest(),
            new Scp914Request()
        };
        
        var httpThread = new Thread(StartHttpServer);
        httpThread.Start();

        Log.Info("HTTP server running...");
    }
    
    private static void StartHttpServer()
    {
        HttpListener listener = new HttpListener();
        string protocol = TTAdmin.Instance.Config.UseSsl ? "s" : "";
        //{TTAdmin.Instance.Config.Host}
        listener.Prefixes.Add($"http{protocol}://+:{TTAdmin.Instance.Config.RestPort}/");
        
        listener.Start();
        Log.Info($"HTTP server started on http{protocol}://+:{TTAdmin.Instance.Config.RestPort}/");

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
        if (context.Request.HttpMethod == "OPTIONS")
        {
            context.Response.AddHeader("Access-Control-Allow-Headers", "Content-Type, Accept, X-Requested-With, API-Key");
            context.Response.AddHeader("Access-Control-Allow-Methods", "GET, POST");
            context.Response.AddHeader("Access-Control-Max-Age", "1728000");
            context.Response.AddHeader("Access-Control-Allow-Origin", "*");
            context.Response.StatusCode = 200;
            context.Response.OutputStream.Close();
            return;
        }
        context.Response.AddHeader("Access-Control-Allow-Origin", "*");
        
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
        
    }
    
    public static void Stop()
    {
        running = false;
    }
}