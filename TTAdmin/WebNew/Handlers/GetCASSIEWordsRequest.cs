using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Text.RegularExpressions;
using Exiled.API.Features;
using TTAdmin.WebNew.DataTypes;
using Utf8Json;

namespace TTAdmin.WebNew.Handlers;

public class GetCASSIEWordsRequest : RequestHandler
{
    public override string Path => "/cassie/words";
    public override MethodType Method => MethodType.GET;
    public override bool RequiresAuth => false;
    public override void ProcessRequest(HttpListenerContext context)
    {

        List<string> voiceLines = new List<string>();
        foreach (var kvp in NineTailedFoxAnnouncer.singleton.voiceLines)
        {
            voiceLines.Add(kvp.apiName);
        }
        /*byte[] buffer = JsonSerializer.Serialize(new CASSIEWordsResponse
        {
            Words = string.Join(", ", voiceLines)
        });
        context.Response.ContentType = "application/json";
        context.Response.ContentLength64 = buffer.Length;
        context.Response.OutputStream.Write(buffer, 0, buffer.Length);
        context.Response.OutputStream.Close();*/
        JsonResponse<CASSIEWordsResponse>.Send(context.Response, new CASSIEWordsResponse
        {
            Words = string.Join(", ", voiceLines)
        });
    }
    
    public class CASSIEWordsResponse
    {
        public string Words { get; set; }
    }
}