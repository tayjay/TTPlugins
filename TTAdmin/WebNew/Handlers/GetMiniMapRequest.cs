using System.Collections.Generic;
using System.Net;
using Exiled.API.Features;
using MapGeneration;
using TTAdmin.WebNew.DataTypes;
using UnityEngine;

namespace TTAdmin.WebNew.Handlers;

/*public class GetMiniMapRequest : RequestHandler
{
    public override string Path { get; } = "/facility/minimap";
    public override MethodType Method { get; } = MethodType.GET;
    public override bool RequiresAuth { get; } = true;
    public override void ProcessRequest(HttpListenerContext context)
    {
        List<byte[]> mapData = new List<byte[]>();
        foreach (ImageGenerator generator in ImageGenerator.ZoneGenerators)
        {
            Log.Debug($"Found {generator.legend.Length} minimaps for {generator.alias}");
            generator.legend.ForEach(minimap =>
            {
                if(minimap.icon == null) return;
                if(!minimap.icon.isReadable) return;
                Log.Debug($"Loading image {minimap.label}");
                mapData.Add(((Texture2D)minimap.icon).EncodeToPNG());
            });
        }
        JsonResponse<List<byte[]>>.Send(context.Response, mapData);
    }
}*/