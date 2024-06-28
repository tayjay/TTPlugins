using System;
using System.Collections.Generic;
using System.Net;
using System.Text;
using Exiled.API.Features;
using TTAdmin.WebNew.DataTypes;

namespace TTAdmin.WebNew.Handlers;

public abstract class RequestHandler : IRequestHandler
{
    
    public abstract string Path { get; }
    public abstract MethodType Method { get; }
    
    public abstract bool RequiresAuth { get; }
    
    public virtual bool CanHandle(HttpListenerRequest request)
    {
        return request.HttpMethod == Method.ToString() && request.Url.AbsolutePath == Path;
    }

    public void Handle(HttpListenerContext context)
    {
        if(!IsValidApiKey(context.Request))
        {
            Log.Debug("Invalid API key");
            foreach(var header in context.Request.Headers.AllKeys)
            {
                Log.Debug($"{header}: {context.Request.Headers[header]}");
            }
            InvalidApiKeyResponse.SendError(context.Response);
            return;
        }
        ProcessRequest(context);
    }
    
    public abstract void ProcessRequest(HttpListenerContext context);
    
    [Flags]
    public enum MethodType
    {
        GET,
        POST
    }

    protected bool IsValidApiKey(HttpListenerRequest request)
    {
        if(!RequiresAuth) return true;
        string apiKey = request.Headers["API-Key"];
        return apiKey == TTAdmin.Instance.Config.ApiKey;
    }
}