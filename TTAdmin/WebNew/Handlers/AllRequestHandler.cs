using System.Collections.Generic;
using System.Net;
using TTAdmin.WebNew.DataTypes;

namespace TTAdmin.WebNew.Handlers;

public abstract class AllRequestHandler : RequestHandler
{
    public override MethodType Method => MethodType.GET | MethodType.POST | MethodType.PATCH;
    public override bool RequiresAuth => true;
    public Dictionary<string,object> Data { get; set; }
    public override void ProcessRequest(HttpListenerContext context)
    {
        if (!IsValidApiKey(context.Request))
        {
            new ErrorResponse(HttpStatusCode.Unauthorized, "Invalid API key").Send(context.Response);
            return;
        }
        if(context.Request.HttpMethod == MethodType.GET.ToString())
        {
            Data = GetGETParams(context);
            ProcessGetRequest(context);
            return;
        }
        if(context.Request.HttpMethod == MethodType.POST.ToString())
        {
            Data = GetPOSTBody(context);
            ProcessPostRequest(context);
            return;
        }
        if(context.Request.HttpMethod == MethodType.PATCH.ToString())
        {
            ProcessPatchRequest(context);
            return;
        }
    }
    
    public abstract void ProcessGetRequest(HttpListenerContext context);
    public abstract void ProcessPostRequest(HttpListenerContext context);
    
    public virtual void ProcessPatchRequest(HttpListenerContext context)
    {
        new ErrorResponse(HttpStatusCode.MethodNotAllowed, "PATCH method not allowed").Send(context.Response);
    }
    
    public override bool CanHandle(HttpListenerRequest request)
    {
        return request.HttpMethod == MethodType.GET.ToString() && request.Url.AbsolutePath == Path || request.HttpMethod == MethodType.POST.ToString() && request.Url.AbsolutePath == Path;
    }
    
    protected Dictionary<string,object> GetGETParams(HttpListenerContext context)
    {
        Dictionary<string,object> dict = new Dictionary<string,object>();
        foreach(string key in context.Request.QueryString.AllKeys)
        {
            dict.Add(key, context.Request.QueryString.Get(key));
        }
        return dict;
    }
    
    protected Dictionary<string,object> GetPOSTBody(HttpListenerContext context)
    {
        string body = new System.IO.StreamReader(context.Request.InputStream).ReadToEnd();
        return Utf8Json.JsonSerializer.Deserialize<Dictionary<string,object>>(body);
    }
}