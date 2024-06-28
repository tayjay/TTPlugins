using System.Collections.Generic;
using System.Linq;
using System.Net;
using TTAdmin.Data;
using TTAdmin.WebNew.DataTypes;
using Player = Exiled.API.Features.Player;

namespace TTAdmin.WebNew.Handlers.Player;

public class GetAllPlayerInfoRequest : RequestHandler
{
    public override string Path => "/player/all";
    public override MethodType Method => MethodType.GET;
    public override bool RequiresAuth => true;
    public override void ProcessRequest(HttpListenerContext context)
    {
        var list = new PlayerInfoListResponse
        {
            //Convert all players to PlayerInfo
            Players = PlayerData.ConvertList(Exiled.API.Features.Player.List.ToList())
        };
        JsonResponse<PlayerInfoListResponse>.Send(context.Response, list);
            
    }
    
    public class PlayerInfoListResponse
    {
        public List<PlayerData> Players { get; set; }
    }
}