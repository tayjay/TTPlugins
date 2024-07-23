using System.Net;
using System.Text;
using Exiled.API.Features;
using Utf8Json;

namespace TTAdmin.WebNew.Handlers;

public class GetPlayerCountHandler : RequestHandler
{
    private static int playerCount => Server.PlayerCount; // Example player count
    
    public override string Path => "/player/count";
    public override MethodType Method => MethodType.GET;
    public override bool RequiresAuth => false;

    public override void ProcessRequest(HttpListenerContext context)
    {
        
        byte[] buffer = JsonSerializer.Serialize(new PlayerCountResponse
        {
            PlayerCount = playerCount,
            Query = context.Request.QueryString.Get("id")
        });
        
        context.Response.ContentType = "application/json";
        context.Response.ContentLength64 = buffer.Length;
        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
        context.Response.OutputStream.Close();
    }
    
    public class PlayerCountResponse
    {
        public int PlayerCount { get; set; }
        public string Query { get; set; }
    }
}

