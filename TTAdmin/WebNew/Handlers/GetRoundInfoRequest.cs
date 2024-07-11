using System.Net;
using TTAdmin.Data;
using TTAdmin.WebNew.DataTypes;

namespace TTAdmin.WebNew.Handlers;

public class GetRoundInfoRequest : RequestHandler
{
    public override string Path => "/round";
    public override MethodType Method => MethodType.GET;
    public override bool RequiresAuth => true;
    public override void ProcessRequest(HttpListenerContext context)
    {
        try
        {
            RoundData roundData = new RoundData();
            
            JsonResponse<RoundData>.Send(context.Response, roundData);
        } catch (System.Exception e)
        {
            /*new Response()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "An error occurred while processing the request: "+e.Message,
                ContentType = "text/plain"
            }.Send(context.Response);*/
            ErrorResponse.SendError(context.Response, HttpStatusCode.InternalServerError, "An error occurred while processing the request:" + e.Message);
        }
    }
}