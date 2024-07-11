using System.Collections.Generic;
using System.Linq;
using System.Net;
using Exiled.API.Features;
using TTAdmin.Data;
using TTAdmin.WebNew.DataTypes;

namespace TTAdmin.WebNew.Handlers;

public class GetRoomInfoRequest : RequestHandler
{
    public override string Path => "/facility/room";
    public override MethodType Method => MethodType.GET;
    public override bool RequiresAuth => true;
    public override void ProcessRequest(HttpListenerContext context)
    {
        try
        {
            RoomInfoRequest request = new RoomInfoRequest()
            {
                RoomName = context.Request.QueryString.Get("name")
            };
            if(request.RoomName == null)
            {
                
                ErrorResponse.SendError(context.Response, HttpStatusCode.BadRequest, "Room name is required");
                
                /*
                new Response()
                {
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Room name is required",
                    ContentType = "text/plain"
                }.Send(context.Response);*/
                return;
            }

            Room room = Room.Get(r => r.Name == request.RoomName).First();
            if (room == null)
            {
                
                ErrorResponse.SendError(context.Response, HttpStatusCode.NotFound, "Room not found");
                /*new Response()
                {
                    StatusCode = HttpStatusCode.NotFound,
                    Message = "Room not found",
                    ContentType = "text/plain"
                }.Send(context.Response);*/
                return;
            }
            
            JsonResponse<RoomData>.Send(context.Response, new RoomData(room));
        }
        catch (System.Exception e)
        {
            
            ErrorResponse.SendError(context.Response, HttpStatusCode.InternalServerError, "An error occurred while processing the request:" + e.Message);
            /*new Response()
            {
                StatusCode = HttpStatusCode.InternalServerError,
                Message = "An error occurred while processing the request:" + e.Message,
                ContentType = "text/plain"
            }.Send(context.Response);*/
        }
    }


    public class RoomInfoRequest
    {
        public string RoomName { get; set; }
    }
}