using System.Net;
using TTAdmin.Data;
using TTAdmin.WebNew.DataTypes;
using UnityEngine;

namespace TTAdmin.WebNew.Handlers.Player;

public class GetPlayerInfoRequest : RequestHandler
{
    public override string Path => "/player";
    public override MethodType Method => MethodType.GET;
    public override bool RequiresAuth => true;
    public override void ProcessRequest(HttpListenerContext context)
    {
        PlayerInfoRequest playerInfoRequest = new PlayerInfoRequest()
        {
            Id = context.Request.QueryString.Get("id") != null ? int.Parse(context.Request.QueryString.Get("id")) : -1,
            Name = context.Request.QueryString.Get("name") != null ? context.Request.QueryString.Get("name") : ""
        };
        
        if(playerInfoRequest.Id == -1 && playerInfoRequest.Name == "")
        {
            new ErrorResponse(HttpStatusCode.BadRequest, "Invalid player info format").Send(context.Response);
            
            return;
        }

        Exiled.API.Features.Player player = null;
        if (playerInfoRequest.Id != -1)
        {
            player = Exiled.API.Features.Player.Get(playerInfoRequest.Id);
        }
        else
        {
            player = Exiled.API.Features.Player.Get(playerInfoRequest.Name);
        }
        if (player != null)
        {
            var playerInfo = new PlayerData(player);
            JsonResponse<PlayerData>.Send(context.Response, playerInfo);
            /*byte[] buffer = JsonSerializer.Serialize();
            context.Response.ContentType = "application/json";
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);*/
        }
        else
        {
            new Response()
            {
                ContentType = "text/plain",
                StatusCode = HttpStatusCode.NotFound,
                Message = "Player not found"
            }.Send(context.Response);
            /*context.Response.StatusCode = (int)HttpStatusCode.NotFound;
            byte[] buffer = Encoding.UTF8.GetBytes("Player not found");
            context.Response.ContentLength64 = buffer.Length;
            context.Response.OutputStream.Write(buffer, 0, buffer.Length);*/
        }
        
        //context.Response.OutputStream.Close();
    }
    
    public class PlayerInfoRequest
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = "";
    }

    public class PlayerInfoResponse
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Ip { get; set; }
        public string SteamId { get; set; }
        public string Role { get; set; }
        public Vector3 Position { get; set; }
        public float Health { get; set; }
        public float MaxHealth { get; set; }
        //public string CurrentItem { get; set; }
        public float Hume { get; set; }
        public float MaxHume { get; set; }
    }
}