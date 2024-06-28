using System.Net;
using TTAdmin.Data;
using TTAdmin.WebNew.DataTypes;

namespace TTAdmin.WebNew.Handlers.Scp914;

public class GetScp914DataRequest : RequestHandler
{
    public override string Path => "/scp914";
    public override MethodType Method => MethodType.GET;
    public override bool RequiresAuth => true;
    public override void ProcessRequest(HttpListenerContext context)
    {
        JsonResponse<Scp914Data>.Send(context.Response, new Scp914Data());
    }
}