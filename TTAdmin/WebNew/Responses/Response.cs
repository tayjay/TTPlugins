using System.Net;
using System.Text;

namespace TTAdmin.WebNew.DataTypes;

public class Response
{
    public HttpStatusCode StatusCode { get; set; }
    public string ContentType { get; set; }
    public string Message { get; set; }
    
    public byte[] Buffer => GetBuffer();
    
    
    
    public virtual byte[] GetBuffer()
    {
        return Encoding.UTF8.GetBytes(Message);
    }
    
    public virtual void Send(HttpListenerResponse response)
    {
        response.StatusCode = (int)StatusCode;
        response.ContentType = ContentType;
        response.ContentLength64 = Buffer.Length;
        response.OutputStream.Write(Buffer, 0, Buffer.Length);
        response.OutputStream.Close();
    }
}