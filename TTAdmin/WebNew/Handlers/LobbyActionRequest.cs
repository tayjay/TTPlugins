using System.IO;
using System.Net;
using Exiled.API.Features;
using TTAdmin.WebNew.DataTypes;
using Utf8Json;

namespace TTAdmin.WebNew.Handlers;

public class LobbyActionRequest : RequestHandler
{
    public override string Path => "/lobby";
    public override MethodType Method => MethodType.POST;
    public override bool RequiresAuth => true;
    public override void ProcessRequest(HttpListenerContext context)
    {
        try
        {
            using var reader = new StreamReader(context.Request.InputStream);
            var lobbyAction = JsonSerializer.Deserialize<RoundActionRequest.RoundAction>(reader.BaseStream);
            
            switch (lobbyAction.Action)
            {
                case "lock":
                    Round.IsLobbyLocked = true;
                    break;
                case "unlock":
                    Round.IsLobbyLocked = false;
                    break;
                default:
                    new Response()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Invalid action",
                        ContentType = "text/plain"
                    }.Send(context.Response);
                    return;
            }

            new Response()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Action completed",
                ContentType = "text/plain"
            }.Send(context.Response);
        }
        catch (System.Exception e)
        {
            new Response()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "An error occurred while processing the request:" + e.Message,
                ContentType = "text/plain"
            }.Send(context.Response);
        }
    }
    
    public class LobbyAction
    {
        public string Action { get; set; }
    }
}