using System;
using System.Net;
using System.Net.WebSockets;
using System.Threading;
using System.Threading.Tasks;
using Exiled.API.Features;

namespace TTAdmin.WebNew;

public class TestServer
{
    public static async Task Main(string[] args)
    {
        HttpListener httpListener = new HttpListener();
        httpListener.Prefixes.Add("http://127.0.0.1:5000/ws/");
        httpListener.Start();
        Console.WriteLine("Server started at ws://localhost:5000/ws/");
        
        while (true)
        {
            HttpListenerContext httpContext = await httpListener.GetContextAsync();
            Log.Debug("External Check: "+httpContext.Request.IsWebSocketRequest);
            Log.Debug("Internal Check: "+TestRequest(httpContext.Request));
            if (httpContext.Request.IsWebSocketRequest)
            {
                HttpListenerWebSocketContext webSocketContext = await httpContext.AcceptWebSocketAsync(null);
                WebSocket webSocket = webSocketContext.WebSocket;
                Console.WriteLine("WebSocket connection established");

                await HandleWebSocketCommunication(webSocket);
            }
            else
            {
                Log.Error("Invalid request: "+httpContext.Request.Headers);
                httpContext.Response.StatusCode = 400;
                httpContext.Response.Close();
            }
        }
    }

    private static async Task HandleWebSocketCommunication(WebSocket webSocket)
    {
        byte[] buffer = new byte[1024];
        while (webSocket.State == WebSocketState.Open)
        {
            WebSocketReceiveResult result = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            if (result.MessageType == WebSocketMessageType.Text)
            {
                string message = System.Text.Encoding.UTF8.GetString(buffer, 0, result.Count);
                Console.WriteLine("Received: " + message);
                // Echo the message back
                await webSocket.SendAsync(new ArraySegment<byte>(buffer, 0, result.Count), WebSocketMessageType.Text, result.EndOfMessage, CancellationToken.None);
            }
            else if (result.MessageType == WebSocketMessageType.Close)
            {
                await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
            }
        }
    }

    public static bool TestRequest(HttpListenerRequest request)
    {
        /*if (!WebSocketProtocolComponent.IsSupported)
            return false;*/
        bool flag = false;
        if (string.IsNullOrEmpty(request.Headers["Connection"]) || string.IsNullOrEmpty(request.Headers["Upgrade"]))
            return false;
        foreach (string strA in request.Headers.GetValues("Connection"))
        {
            if (string.Compare(strA, "Upgrade", StringComparison.OrdinalIgnoreCase) == 0)
            {
                flag = true;
                break;
            }
        }
        if (!flag)
            return false;
        foreach (string strA in request.Headers.GetValues("Upgrade"))
        {
            if (string.Compare(strA, "websocket", StringComparison.OrdinalIgnoreCase) == 0)
                return true;
        }
        return false;
    }
}