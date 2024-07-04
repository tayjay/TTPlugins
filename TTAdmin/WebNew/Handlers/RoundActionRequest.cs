using System.IO;
using System.Net;
using Exiled.API.Features;
using TTAdmin.Data;
using TTAdmin.WebNew.DataTypes;
using Utf8Json;

namespace TTAdmin.WebNew.Handlers;

public class RoundActionRequest : RequestHandler
{
    public override string Path => "/round";
    public override MethodType Method => MethodType.POST;
    public override bool RequiresAuth => true;
    public override void ProcessRequest(HttpListenerContext context)
    {
        try
        {
            using var reader = new StreamReader(context.Request.InputStream);
            var roundAction = JsonSerializer.Deserialize<RoundAction>(reader.BaseStream);
            switch (roundAction.Action)
            {
                case "start":
                    Round.Start();
                    break;
                case "end":
                    Round.EndRound(true);
                    break;
                case "restart":
                    Round.Restart();
                    break;
                case "lock":
                    Round.IsLocked = true;
                    break;
                case "unlock":
                    Round.IsLocked = false;
                    break;
                default:
                    ErrorResponse.SendError(context.Response, HttpStatusCode.BadRequest, "Invalid action");
                    /*new Response()
                    {
                        StatusCode = HttpStatusCode.BadRequest,
                        Message = "Invalid action",
                        ContentType = "text/plain"
                    }.Send(context.Response);*/
                    return;
            }
            /*new Response()
            {
                StatusCode = HttpStatusCode.OK,
                Message = "Action completed successfully",
                ContentType = ""
            }.Send(context.Response);*/
            JsonResponse<RoundData>.Send(context.Response, new RoundData());
        }
        catch (System.Exception e)
        {
            ErrorResponse.SendError(context.Response, HttpStatusCode.InternalServerError, "An error occurred while processing the request:" + e.Message);
            /*new Response()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "An error occurred while processing the request:" + e.Message,
                ContentType = "text/plain"
            }.Send(context.Response);*/
        }
    }
    
    public class RoundAction
    {
        public string Action { get; set; }
    }
    
}