using System.Net;
using Exiled.API.Features;
using TTAdmin.WebNew.DataTypes;

namespace TTAdmin.WebNew.Handlers;

public class CASSIEActionRequest : AllRequestHandler
{
    public override string Path => "/facility/cassie";
    public override void ProcessGetRequest(HttpListenerContext context)
    {
        ErrorResponse.SendError(context.Response, HttpStatusCode.MethodNotAllowed, "Method not allowed");
    }

    public override void ProcessPostRequest(HttpListenerContext context)
    {
        using var reader = new System.IO.StreamReader(context.Request.InputStream);
        string body = reader.ReadToEnd();
        PostCASSIEActionRequest postCASSIEActionRequest = Utf8Json.JsonSerializer.Deserialize<PostCASSIEActionRequest>(body);

        if (postCASSIEActionRequest.Message == null)
        {
            ErrorResponse.SendError(context.Response, HttpStatusCode.BadRequest, "Invalid request body");
            return;
        }

        if (postCASSIEActionRequest.Action == "say" || postCASSIEActionRequest.Action == "")
        {
            if(postCASSIEActionRequest.GlitchChance > 0 || postCASSIEActionRequest.JamChance > 0)
            {
                Cassie.GlitchyMessage(postCASSIEActionRequest.Message, postCASSIEActionRequest.GlitchChance, postCASSIEActionRequest.JamChance);
            }
            else
            {
                Cassie.Message(postCASSIEActionRequest.Message, postCASSIEActionRequest.IsHeld, postCASSIEActionRequest.IsNoisy, postCASSIEActionRequest.IsSubtitles);
            }
            
        } else if (postCASSIEActionRequest.Action == "clear")
        {
            Cassie.Clear();
        } else if (postCASSIEActionRequest.Action == "isValid")
        {
            new Response()
            {
                ContentType = "application/json",
                Message = "{\"isValid\":\"" + Cassie.IsValid(postCASSIEActionRequest.Message) + "\"}",
                StatusCode = HttpStatusCode.OK
            }.Send(context.Response);
            return;
        }

        
        JsonResponse<SuccessResponse>.Send(context.Response, new SuccessResponse());
    }
    
    public class PostCASSIEActionRequest
    {
        public string Message { get; set; } = "";
        public bool IsHeld { get; set; } = false;
        public bool IsNoisy { get; set; } = true;
        public bool IsSubtitles { get; set; } = false;
        public float GlitchChance { get; set; } = 0f;
        public float JamChance { get; set; } = 0f;
        public string Action { get; set; } = "";
    }
    
    public class SuccessResponse
    {
        public string Status { get; set; } = "Success";
    }
}