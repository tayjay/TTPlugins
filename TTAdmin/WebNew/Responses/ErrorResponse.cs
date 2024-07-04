using System.Net;
using System.Text;
using Utf8Json;

namespace TTAdmin.WebNew.DataTypes;

public class ErrorResponse : Response
{
    public ErrorResponse(HttpStatusCode code, string message)
    {
        StatusCode = HttpStatusCode.BadRequest;
        ContentType = "application/json";
        Message = message;
    }

    public override byte[] GetBuffer()
    {
        return JsonSerializer.Serialize(new ErrorRsp { Error = Message });
    }

    public class ErrorRsp
    {
        public string Error { get; set; }
    }
    
    
    public static void SendError(HttpListenerResponse response, HttpStatusCode code, string message)
    {
        new ErrorResponse(code, message).Send(response);
    }
    
    public static void Forbidden(HttpListenerResponse response, string message = "Forbidden")
    {
        SendError(response, HttpStatusCode.Forbidden, message);
    }
    
    public static void BadRequest(HttpListenerResponse response, string message = "Bad Request")
    {
        SendError(response, HttpStatusCode.BadRequest, message);
    }
    
    public static void Unauthorized(HttpListenerResponse response, string message = "Incorrect API Key")
    {
        SendError(response, HttpStatusCode.Unauthorized, message);
    }
    
    public static void NotFound(HttpListenerResponse response, string message = "Not Found")
    {
        SendError(response, HttpStatusCode.NotFound, message);
    }
}