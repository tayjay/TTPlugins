using System.Net;
using LightContainmentZoneDecontamination;
using TTAdmin.Data;
using TTAdmin.WebNew.DataTypes;

namespace TTAdmin.WebNew.Handlers;

public class DecontaminationRequestHandler : AllRequestHandler
{
    public override string Path { get; }
    public override void ProcessGetRequest(HttpListenerContext context)
    {
        JsonResponse<LczDecontaminationData>.Send(context.Response, new LczDecontaminationData());
    }

    public override void ProcessPostRequest(HttpListenerContext context)
    {
        using var reader = new System.IO.StreamReader(context.Request.InputStream);
        string body = reader.ReadToEnd();
        PostDecontaminationRequest postDecontaminationRequest = Utf8Json.JsonSerializer.Deserialize<PostDecontaminationRequest>(body);

        if (postDecontaminationRequest.Action == "start")
        {
            DecontaminationController.Singleton.Start();
        }
        else if (postDecontaminationRequest.Action == "disable")
        {
            DecontaminationController.Singleton.NetworkDecontaminationOverride = DecontaminationController.DecontaminationStatus.Disabled;
        }
        else if (postDecontaminationRequest.Action == "enable")
        {
            DecontaminationController.Singleton.NetworkDecontaminationOverride = DecontaminationController.DecontaminationStatus.None;
        }
        else if (postDecontaminationRequest.Action == "force")
        {
            DecontaminationController.Singleton.NetworkDecontaminationOverride = DecontaminationController.DecontaminationStatus.Forced;
        }
        else
        {
            ErrorResponse.SendError(context.Response, HttpStatusCode.BadRequest, "Invalid action");
            return;
        }

        JsonResponse<LczDecontaminationData>.Send(context.Response, new LczDecontaminationData());
    }
    
    public class PostDecontaminationRequest
    {
        public string Action { get; set; } = "";
    }
}