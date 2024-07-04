using System;
using System.Net;
using TTAdmin.Data;
using TTAdmin.WebNew.DataTypes;
using TTCore.Npcs.AI.Core.Management;

namespace TTAdmin.WebNew.Handlers;

public class GetAIDataHandler : RequestHandler
{
    public override string Path => "/aidata";
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
            if (player.TryGetAI(out var ai))
            {
                try
                {
                    JsonResponse<AIData>.Send(context.Response, new AIData(ai));
                } catch (Exception e)
                {
                    new ErrorResponse(HttpStatusCode.InternalServerError, e.Message).Send(context.Response);
                }
                
            }
            else
            {
                new ErrorResponse(HttpStatusCode.BadRequest, "Player does not have AI").Send(context.Response);
            }
        }
        else
        {
            new ErrorResponse(HttpStatusCode.NotFound, "Player not found").Send(context.Response);
        }
    }
    
    public class PlayerInfoRequest
    {
        public int Id { get; set; } = -1;
        public string Name { get; set; } = "";
    }
}