using System.IO;
using System.Net;
using Scp914;
using TTAdmin.Data;
using TTAdmin.WebNew.DataTypes;
using Utf8Json;

namespace TTAdmin.WebNew.Handlers.Scp914;

public class Scp914Request : AllRequestHandler
{
    public override string Path => "/scp914";
    public override void ProcessGetRequest(HttpListenerContext context)
    {
        JsonResponse<Scp914Data>.Send(context.Response, new Scp914Data());
    }

    public override void ProcessPostRequest(HttpListenerContext context)
    {
        try
        {
            using var reader = new StreamReader(context.Request.InputStream);
            var scp914Request = JsonSerializer.Deserialize<Scp914RequestData>(reader.BaseStream);

            switch (scp914Request.Action)
            {
                case "knob_status":
                    Exiled.API.Features.Scp914.KnobStatus = Scp914KnobSetting.TryParse<Scp914KnobSetting>(scp914Request.Data.ToString(), out var knobStatus) ? knobStatus : Exiled.API.Features.Scp914.KnobStatus;
                    break;
                case "start":
                    Exiled.API.Features.Scp914.Start();
                    break;
                default:
                    new ErrorResponse(HttpStatusCode.BadRequest,"Invalid action").Send(context.Response);
                    return;
            }
            JsonResponse<Scp914Data>.Send(context.Response, new Scp914Data());
        }   
        catch (System.Exception e)
        {
            new ErrorResponse(HttpStatusCode.InternalServerError,"An error occurred while processing the request:" + e.Message).Send(context.Response);
        }
    }
    
    public class Scp914RequestData
    {
        public string Action { get; set; }
        public object Data { get; set; } = null;
    }
}