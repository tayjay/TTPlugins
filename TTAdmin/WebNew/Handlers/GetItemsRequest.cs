using System;
using System.Collections.Generic;
using System.Net;
using TTAdmin.WebNew.DataTypes;
using Utf8Json;

namespace TTAdmin.WebNew.Handlers;

public class GetItemsRequest : RequestHandler
{
    public override string Path => "/items";
    public override MethodType Method => MethodType.GET;
    public override bool RequiresAuth => false;
    public override void ProcessRequest(HttpListenerContext context)
    {
        List<string> items = new List<string>();
        //Iterate over all ItemType Enums
        foreach (ItemType item in Enum.GetValues(typeof(ItemType)))
        {
            items.Add(item.ToString());
        }
        
        JsonResponse<ItemsResponse>.Send(context.Response, new ItemsResponse()
        {
            Items = items
        });
    }

    public class ItemsResponse
    {
        public List<string> Items { get; set; }
    }
}