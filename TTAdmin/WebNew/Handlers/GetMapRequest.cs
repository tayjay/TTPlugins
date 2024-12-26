using System.Collections.Generic;
using System.Net;
using MapGeneration;
using TTAdmin.WebNew.DataTypes;
using UnityEngine;

namespace TTAdmin.WebNew.Handlers;

/*public class GetMapRequest : RequestHandler
{
    public override string Path { get; } = "/facility/map";
    public override MethodType Method { get; } = MethodType.GET;
    public override bool RequiresAuth { get; } = true;
    public override void ProcessRequest(HttpListenerContext context)
    {
        Dictionary<string, byte[]> mapData = DictionaryPool<string, byte[]>.Pool.Get();
        foreach (ImageGenerator generator in ImageGenerator.ZoneGenerators)
        {
            mapData.Add(generator.alias,generator.map.EncodeToPNG());
        }
        JsonResponse<Dictionary<string, byte[]>>.Send(context.Response, mapData);
    }
}*/