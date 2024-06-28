using System.Net;

namespace TTAdmin.WebNew.DataTypes;

public class InvalidApiKeyResponse : Response
{
    public InvalidApiKeyResponse()
    {
        StatusCode = HttpStatusCode.Unauthorized;
        ContentType = "text/plain";
        Message = "Invalid API key";
    }
    
    public static void SendError(HttpListenerResponse response)
    {
        new InvalidApiKeyResponse().Send(response);
    }
}