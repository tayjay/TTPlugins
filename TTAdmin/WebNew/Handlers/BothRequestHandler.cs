using System.Net;
using TTAdmin.WebNew.DataTypes;

namespace TTAdmin.WebNew.Handlers;

public abstract class BothRequestHandler : RequestHandler
{
    public override MethodType Method => MethodType.GET | MethodType.POST;
    public override bool RequiresAuth => true;
    public override void ProcessRequest(HttpListenerContext context)
    {
        if (!IsValidApiKey(context.Request))
        {
            new ErrorResponse(HttpStatusCode.Unauthorized, "Invalid API key").Send(context.Response);
            return;
        }
        if(context.Request.HttpMethod == MethodType.GET.ToString())
        {
            ProcessGetRequest(context);
            return;
        }
        if(context.Request.HttpMethod == MethodType.POST.ToString())
        {
            ProcessPostRequest(context);
            return;
        }
    }
    
    public abstract void ProcessGetRequest(HttpListenerContext context);
    public abstract void ProcessPostRequest(HttpListenerContext context);
    
    public override bool CanHandle(HttpListenerRequest request)
    {
        return request.HttpMethod == MethodType.GET.ToString() && request.Url.AbsolutePath == Path || request.HttpMethod == MethodType.POST.ToString() && request.Url.AbsolutePath == Path;
    }
}