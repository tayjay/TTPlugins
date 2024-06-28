using System.Net;
using TTAdmin.Data;
using TTAdmin.WebNew.DataTypes;

namespace TTAdmin.WebNew.Handlers;

public class GetFacilityRequest : RequestHandler
{
    public override string Path => "/facility";
    public override MethodType Method => MethodType.GET;
    public override bool RequiresAuth => true;
    public override void ProcessRequest(HttpListenerContext context)
    {
        try
        {
            FacilityData facilityData = new FacilityData();
        
            JsonResponse<FacilityData>.Send(context.Response, facilityData);
        } catch (System.Exception e)
        {
            new Response()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "An error occurred while processing the request: "+e.Message,
                ContentType = "text/plain"
            }.Send(context.Response);
        }
        
    }
}