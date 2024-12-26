using System.Collections.Generic;
using System.Net;
using MapGeneration;
using TTAdmin.WebNew.DataTypes;
using UnityEngine;

namespace TTAdmin.WebNew.Handlers;

/*public class GetAllMapsRequest : RequestHandler
{
    public override string Path { get; } = "/facility/map/all";
    public override MethodType Method { get; } = MethodType.GET;
    public override bool RequiresAuth { get; } = true;
    public override void ProcessRequest(HttpListenerContext context)
    {
        Dictionary<string, List<byte[]>> mapData = DictionaryPool<string, List<byte[]>>.Pool.Get();
        foreach (ImageGenerator generator in ImageGenerator.ZoneGenerators)
        {
            if(!mapData.ContainsKey(generator.alias)) mapData.Add(generator.alias, new List<byte[]>());
            foreach(Texture2D map in generator.maps)
            {
                mapData[generator.alias].Add(map.EncodeToPNG());
            }
        }
        JsonResponse<Dictionary<string, List<byte[]>>>.Send(context.Response, mapData);
    }
}*/