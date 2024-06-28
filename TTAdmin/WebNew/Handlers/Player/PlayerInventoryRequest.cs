using System;
using System.Collections.Generic;
using System.Net;
using Exiled.API.Enums;
using TTAdmin.Data;
using TTAdmin.WebNew.DataTypes;

namespace TTAdmin.WebNew.Handlers.Player;

public class PlayerInventoryRequest : BothRequestHandler
{
    public override string Path => "/player/inventory";
    public override void ProcessGetRequest(HttpListenerContext context)
    {
        GetPlayerInfoRequest.PlayerInfoRequest playerInfoRequest = new GetPlayerInfoRequest.PlayerInfoRequest()
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
        JsonResponse<InventoryData>.Send(context.Response, new InventoryData(player));
    }

    public override void ProcessPostRequest(HttpListenerContext context)
    {
        using var reader = new System.IO.StreamReader(context.Request.InputStream);
        string body = reader.ReadToEnd();
        PostInventoryRequest postInventoryRequest = Utf8Json.JsonSerializer.Deserialize<PostInventoryRequest>(body);
        
        // Get player
        Exiled.API.Features.Player player = Exiled.API.Features.Player.Get(postInventoryRequest.Id);
        if (player == null)
        {
            new ErrorResponse(HttpStatusCode.NotFound, "Player not found").Send(context.Response);
            return;
        }

        switch (postInventoryRequest.Action)
        {
            case "add_item":
                foreach (var item in postInventoryRequest.Items)
                {
                    if (Enum.TryParse<ItemType>(item.ToString(), out ItemType type))
                    {
                        player.AddItem(type);
                    }
                }
                break;
            case "remove_item":
                foreach (var item in postInventoryRequest.Items)
                {
                    if (Enum.TryParse<ItemType>(item.ToString(), out ItemType type))
                    {
                        player.RemoveItem(i => i.Type == type);
                    }
                }
                break;
            case "clear_items":
                player.ClearItems();
                break;
            case "add_ammo":
                foreach (var ammo in postInventoryRequest.Ammo)
                {
                    if (Enum.TryParse<AmmoType>(ammo.Key, out AmmoType type))
                    {
                        player.AddAmmo(type,(byte)ammo.Value);
                    }
                }
                break;
            /*case "remove_ammo":
                foreach (var ammo in postInventoryRequest.Ammo)
                {
                    if (Enum.TryParse<AmmoType>(ammo.Key, out AmmoType type))
                    {
                        
                    }
                }
                break;*/
            case "clear_ammo":
                player.ClearAmmo();
                break;
            case "clear_all":
                player.ClearInventory();
                break;
            default:
                new ErrorResponse(HttpStatusCode.BadRequest, "Invalid action").Send(context.Response);
                return;
        }
        
        JsonResponse<InventoryData>.Send(context.Response, new InventoryData(player));

    }
    
    public class PostInventoryRequest
    {
        public int Id { get; set; }
        public string Action { get; set; }
        public List<object> Items { get; set; } = null;
        public Dictionary<string, object> Ammo { get; set; } = null;
        
    }
}