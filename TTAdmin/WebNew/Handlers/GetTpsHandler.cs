using System.Net;
using System.Text;
using Exiled.API.Features;
using TTAdmin.WebNew.DataTypes;
using Utf8Json;

namespace TTAdmin.WebNew.Handlers;

public class GetTpsHandler : RequestHandler
{
    public override string Path => "/tps";
    public override MethodType Method => MethodType.GET;
    public override bool RequiresAuth => true;

    public override void ProcessRequest(HttpListenerContext context)
    {
        JsonResponse<TpsResponse>.Send(context.Response,new TpsResponse
        {
            Tps = Server.Tps
        });
    }
    
    public class TpsResponse
    {
        public double Tps { get; set; }
    }
}