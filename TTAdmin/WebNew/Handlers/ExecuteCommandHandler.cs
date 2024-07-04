using System.IO;
using System.Net;
using System.Text;
using Exiled.API.Features;
using TTAdmin.Admin;
using TTAdmin.WebNew.DataTypes;
using Utf8Json;

namespace TTAdmin.WebNew.Handlers;

public class ExecuteCommandHandler : RequestHandler
{
    public override string Path => "/command";
    public override MethodType Method => MethodType.POST;
    public override bool RequiresAuth => true;
    private TTAdminCommandSender _serverConsoleSender = new TTAdminCommandSender();
    public override void ProcessRequest(HttpListenerContext context)
    {
        if(!TTAdmin.Instance.Config.AllowCommands)
        {
            //new ErrorResponse(HttpStatusCode.Forbidden, "Generic commands are disabled").Send(context.Response);
            ErrorResponse.Forbidden(context.Response, "Generic commands are disabled");
            return;
        }
        using (var reader = new StreamReader(context.Request.InputStream, context.Request.ContentEncoding))
        {
            string json = reader.ReadToEnd();
            var commandRequest = JsonSerializer.Deserialize<CommandRequest>(json);
            
            if (commandRequest != null)
            {
                string command = commandRequest.Command;
                // Logic to execute the command on the server
                Log.Info($"Executing command: {command}");

                string responseText="";
                if (commandRequest.Type == "RemoteAdmin")
                {
                    responseText = RemoteAdmin.CommandProcessor.ProcessQuery(command, _serverConsoleSender);
                } else if (commandRequest.Type == "Console")
                {
                    responseText = Server.ExecuteCommand(commandRequest.Command, _serverConsoleSender);
                }
                else
                {
                    ErrorResponse.BadRequest(context.Response, "Invalid command type");
                    //new ErrorResponse(HttpStatusCode.BadRequest, "Invalid command type").Send(context.Response);
                }
                
                var response = new CommandResponse()
                {
                    Response = responseText
                };
                JsonResponse<CommandResponse>.Send(context.Response, response);
                
                /*
                byte[] buffer = Encoding.UTF8.GetBytes(responseText);
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);*/
            }
            else
            {
                new Response()
                {
                    ContentType = "text/plain",
                    StatusCode = HttpStatusCode.BadRequest,
                    Message = "Invalid command format"
                }.Send(context.Response);
                /*context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                byte[] buffer = Encoding.UTF8.GetBytes("Invalid command format");
                context.Response.ContentLength64 = buffer.Length;
                context.Response.OutputStream.Write(buffer, 0, buffer.Length);*/
            }
            //context.Response.OutputStream.Close();
        }
    }
    
    public class CommandRequest
    {
        public string Command { get; set; }
        public string Type { get; set; }
    }
    
    public class CommandResponse
    {
        public string Response { get; set; }
    }
}