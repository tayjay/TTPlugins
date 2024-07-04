using System.Net;
using Exiled.API.Features;
using TTAdmin.Data;
using TTAdmin.WebNew.DataTypes;

namespace TTAdmin.WebNew.Handlers;

public class WarheadRequestHandler : AllRequestHandler
{
    public override string Path => "/warhead";
    public override void ProcessGetRequest(HttpListenerContext context)
    {
        JsonResponse<WarheadData>.Send(context.Response, new WarheadData());
    }

    public override void ProcessPostRequest(HttpListenerContext context)
    {
        using var reader = new System.IO.StreamReader(context.Request.InputStream);
        string body = reader.ReadToEnd();
        PostWarheadRequest postWarheadRequest = Utf8Json.JsonSerializer.Deserialize<PostWarheadRequest>(body);

        if (postWarheadRequest.Action == "start")
        {
            Warhead.Start();
        }
        else if (postWarheadRequest.Action == "stop")
        {
            Warhead.Stop();
        }
        else if (postWarheadRequest.Action == "toggle_lever")
        {
            Warhead.LeverStatus = !Warhead.LeverStatus;
        }
        else if (postWarheadRequest.Action == "lock")
        {
            Warhead.IsLocked = true;
        }
        else if (postWarheadRequest.Action == "unlock")
        {
            Warhead.IsLocked = false;
        }
        else if (postWarheadRequest.Action == "shake")
        {
            Warhead.Shake();
        }
        else
        {
            //ErrorResponse.SendError(context.Response, HttpStatusCode.BadRequest, "Invalid action");
            ErrorResponse.BadRequest(context.Response, "Invalid action");
            return;
        }

        JsonResponse<WarheadData>.Send(context.Response, new WarheadData());
    }
    
    public class PostWarheadRequest
    {
        public string Action { get; set; } = "";
    }
}