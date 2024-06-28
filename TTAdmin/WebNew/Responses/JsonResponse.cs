using System.Net;
using Utf8Json;

namespace TTAdmin.WebNew.DataTypes;

public class JsonResponse<T> : Response where T : class
{
    public T Data { get; set; }
    
    
    public JsonResponse(T data)
    {
        Data = data;
        StatusCode = HttpStatusCode.OK;
        ContentType = "application/json";
    }
    
    
    public override byte[] GetBuffer()
    {
        return JsonSerializer.Serialize(Data);
    }
    
    public static void Send(HttpListenerResponse response, T data)
    {
        new JsonResponse<T>(data).Send(response);
    }
    
}