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
}