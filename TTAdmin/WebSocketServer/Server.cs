// Copyright © 2017 - MazyModz. Created by Dennis Andersson. All rights reserved.

using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using Exiled.API.Features;

namespace TTAdmin.WebSocketServer;

/// <summary>
    /// Handler for when a message was received
    /// </summary>
    public class OnMessageReceivedHandler : EventArgs
    {
        /// <summary>The client that send the message</summary>
        private Client _client;

        /// <summary>The message the client sent</summary>
        private string _message;

        /// <summary>Create a new message received event handler</summary>
        /// <param name="Client">The client that sent the message</param>
        /// <param name="Message">The message the client sent</param>
        public OnMessageReceivedHandler(Client Client, string Message)
        {
            this._client = Client;
            this._message = Message;
        }

        /// <summary>Get the client that sent the received message</summary>
        /// <returns>The client that sent the message</returns>
        public Client GetClient()
        {
            return _client;
        }

        /// <summary>The message that was received from the client</summary>
        /// <returns>The received message</returns>
        public string GetMessage()
        {
            return _message;
        }

    }

    /// <summary>
    /// Handler for when a message was send to a client
    /// </summary>
    public class OnSendMessageHandler : EventArgs
    {
        /// <summary>The client the message was sent to</summary>
        private Client _client;

        /// <summary>The message that was sent to the client</summary>
        private string _message;

        /// <summary>Create a new handler for when a message was sent</summary>
        /// <param name="Client">The client the message was sent to</param>
        /// <param name="Message">The message that was sent to the client</param>
        public OnSendMessageHandler(Client Client, string Message)
        {
            this._client = Client;
            this._message = Message;
        }

        /// <summary>The client the message was sent to</summary>
        /// <returns>The client receiver</returns>
        public Client GetClient()
        {
            return _client;
        }

        /// <summary>The message that was send to the client</summary>
        /// <returns>The sent message</returns>
        public string GetMessage()
        {
            return _message;
        }
    }

    /// <summary>
    /// Handler for when a client connected
    /// </summary>
    public class OnClientConnectedHandler : EventArgs
    {
        /// <summary>The client that connected to the server</summary>
        private Client _client;

        /// <summary>Create a new event handler for when a client connected</summary>
        /// <param name="Client">The client that connected</param>
        public OnClientConnectedHandler(Client Client)
        {
            this._client = Client;
        }

        /// <summary>Get the client that was connected</summary>
        /// <returns>The client that connected </returns>
        public Client GetClient()
        {
            return _client;
        }
    }

    /// <summary>
    /// Handler for when a client disconnects
    /// </summary>
    public class OnClientDisconnectedHandler : EventArgs
    {
        /// <summary>The client that diconnected</summary>
        private Client _client;

        /// <summary>Create a new handler for when a client disconnects</summary>
        /// <param name="Client">The disconnected client</param>
        public OnClientDisconnectedHandler(Client Client)
        {
            this._client = Client;
        }

        /// <summary>Gets the client that disconnected</summary>
        /// <returns>The disconnected client</returns>
        public Client GetClient()
        {
            return _client;
        }
    }

    ///<summary>
    /// Object for all listen servers
    ///</summary>
    public partial class Server
    {
        #region Fields

        /// <summary>The listen socket (server socket)</summary>
        private Socket _socket;

        /// <summary>The listen ip end point of the server</summary>
        private IPEndPoint _endPoint;

        /// <summary>The connected clients to the server </summary>
        private List<Client> _clients = new List<Client>();

        #endregion

        #region Class Events

        /// <summary>Create and start a new listen socket server</summary>
        /// <param name="EndPoint">The listen endpoint of the server</param>
        public Server(IPEndPoint EndPoint)
        {
            // Set the endpoint if the input is valid
            if (EndPoint == null) return;
            this._endPoint = EndPoint;

            // Create a new listen socket
            this._socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);

            Console.WriteLine("Copyright © 2017 - MazyModz. Created by Dennis Andersson. All rights reserved.\n\n");
            Console.WriteLine("WebSocket Server Started\nListening on {0}:{1}\n", GetEndPoint().Address.ToString(), GetEndPoint().Port);

            // Start the server
            start();
        }

        #endregion

        #region Field Getters

        /// <summary>Gets the listen socket</summary>
        /// <returns>The listen socket</returns>
        public Socket GetSocket()
        {
            return _socket;
        }

        /// <summary>Get the listen socket endpoint</summary>
        /// <returns>The listen socket endpoint</returns>
        public IPEndPoint GetEndPoint()
        {
            return _endPoint;
        }

        /// <summary>Gets a connected client at the given index</summary>
        /// <param name="Index">The connected client array index</param>
        /// <returns>The connected client at the index, returns null if the index is out of bounds</returns>
        public Client GetConnectedClient(int Index)
        {
            if (Index < 0 || Index >= _clients.Count) return null;
            return _clients[Index];
        }

        /// <summary>Gets a connected client with the given guid</summary>
        /// <param name="Guid">The Guid of the client to get</param>
        /// <returns>The client with the given id, return null if no client with the guid could be found</returns>
        public Client GetConnectedClient(string Guid)
        {
            foreach(Client client in _clients)
            {
                if (client.GetGuid() == Guid) return client;
            }
            return null;
        }

        /// <summary>Gets a connected client with the given socket</summary>
        /// <param name="Socket">The socket of the client </param>
        /// <returns>The connected client with the given socket, returns null if no client with the socket was found</returns>
        public Client GetConnectedClient(Socket Socket)
        {
            foreach(Client client in _clients)
            {
                if (client.GetSocket() == Socket) return client;
            }
            return null;
        }

        /// <summary>Get the number of clients that are connected to the server</summary>
        /// <returns>The number of connected clients</returns>
        public int GetConnectedClientCount()
        {
            return _clients.Count;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Starts the listen server when a server object is created
        /// </summary>
        private void start()
        {
            // Bind the socket and start listending
            GetSocket().Bind(GetEndPoint());
            GetSocket().Listen(0);

            // Start to accept clients and accept incomming connections 
            GetSocket().BeginAccept(connectionCallback, null);
        }

        /// <summary>
        /// Stops the listen server 
        /// </summary>
        public void Stop()
        {
            GetSocket().Close();
            GetSocket().Dispose();
        }

        /// <summary>Called when the socket is trying to accept an incomming connection</summary>
        /// <param name="AsyncResult">The async operation state</param>
        private void connectionCallback(IAsyncResult AsyncResult)
        {
            try
            {
                // Gets the client thats trying to connect to the server
                Socket clientSocket = GetSocket().EndAccept(AsyncResult);

                // Read the handshake updgrade request
                byte[] handshakeBuffer = new byte[TTAdmin.Instance.Config.WsBufferSize];
                int handshakeReceived = clientSocket.Receive(handshakeBuffer);

                // Get the hanshake request key and get the hanshake response
                string requestKey = Helpers.GetHandshakeRequestKey(Encoding.Default.GetString(handshakeBuffer));
                string hanshakeResponse = Helpers.GetHandshakeResponse(Helpers.HashKey(requestKey));
                
                string handshakeRequest = Encoding.Default.GetString(handshakeBuffer);
                //Log.Debug("Handshake request: " + handshakeRequest);
                
                // Extract the request path from the first line of the request
                string requestLine = handshakeRequest.Split(new[] { "\r\n" }, StringSplitOptions.None)[0];
                string requestPath = requestLine.Split(' ')[1]; // Example: "GET /path HTTP/1.1"
                Log.Debug("Request path: " + requestPath);
                
                // Parse the headers from the request
                Dictionary<string, string> headers = ParseHeaders(handshakeRequest);
                
                /*if(headers.ContainsKey("Method"))
                    Log.Info("Method: " + headers["Method"]);*/
                
                // Validate the API key
                if (headers.ContainsKey("API-Key"))
                {
                    //Get the API key from the header, this isn't always possible
                    if (!ValidateApiKey(headers["API-Key"]))
                    {
                        Log.Error("Invalid API key");
                        clientSocket.Send(Encoding.Default.GetBytes("HTTP/1.1 401 Unauthorized\r\n\r\n"));
                        clientSocket.Close();
                        GetSocket().BeginAccept(connectionCallback, null);
                        return;
                    }
                }
                else
                {
                    string[] pathquery = requestPath.Split('?');
                    if (pathquery.Length > 1)
                    {
                        string[] queryParts = pathquery[1].Split('&');
                        Dictionary<string, string> query = new Dictionary<string, string>();
                        foreach (string queryPart in queryParts)
                        {
                            string[] queryPartParts = queryPart.Split('=');
                            query[queryPartParts[0]] = queryPartParts[1];
                        }

                        if (!query.ContainsKey("API-Key"))
                        {
                            Log.Error("No API key provided");
                            clientSocket.Send(Encoding.Default.GetBytes("HTTP/1.1 401 Unauthorized\r\n\r\n"));
                            clientSocket.Close();
                            GetSocket().BeginAccept(connectionCallback, null);
                            return;
                        }
                        if (!ValidateApiKey(query["API-Key"]))
                        {
                            Log.Error("Invalid API key");
                            clientSocket.Send(Encoding.Default.GetBytes("HTTP/1.1 401 Unauthorized\r\n\r\n"));
                            clientSocket.Close();
                            GetSocket().BeginAccept(connectionCallback, null);
                            return;
                        }
                    }
                    else
                    {
                        Log.Error("No API key provided");
                        clientSocket.Send(Encoding.Default.GetBytes("HTTP/1.1 401 Unauthorized\r\n\r\n"));
                        clientSocket.Close();
                        GetSocket().BeginAccept(connectionCallback, null);
                    }
                }
                
                

                // Send the handshake updgrade response to the connecting client 
                clientSocket.Send(Encoding.Default.GetBytes(hanshakeResponse));

                // Create a new client object and add 
                // it to the list of connected clients
                Client client = new Client(this, clientSocket);
                _clients.Add(client);

                // Call the event when a client has connected to the listen server 
                if (OnClientConnected == null) throw new Exception("Server error: event OnClientConnected is not bound!");
                OnClientConnected(this, new OnClientConnectedHandler(client));

                // Start to accept incomming connections again 
                GetSocket().BeginAccept(connectionCallback, null);

            }
            catch (Exception Exception)
            {
                Log.Error($"An error has occured while trying to accept a connecting client.\n\n{Exception.Message}");
            }
        }

        /// <summary>Called when a message was recived, calls the OnMessageReceived event</summary>
        /// <param name="Client">The client that sent the message</param>
        /// <param name="Message">The message that the client sent</param>
        public void ReceiveMessage(Client Client, string Message)
        {
            if (OnMessageReceived == null) throw new Exception("Server error: event OnMessageReceived is not bound!");
            OnMessageReceived(this, new OnMessageReceivedHandler(Client, Message));
        }

        /// <summary>Called when a client disconnectes, calls event OnClientDisconnected</summary>
        /// <param name="Client">The client that disconnected</param>
        public void ClientDisconnect(Client Client)
        {
            // Remove the client from the connected clients list
            _clients.Remove(Client);

            // Call the OnClientDisconnected event
            if (OnClientDisconnected == null) throw new Exception("Server error: OnClientDisconnected is not bound!");
            OnClientDisconnected(this, new OnClientDisconnectedHandler(Client));
        }

        #endregion

        #region Server Events

        /// <summary>Send a message to a connected client</summary>
        /// <param name="Client">The client to send the data to</param>
        /// <param name="Data">The data to send the client</param>
        public void SendMessage(Client Client, string Data)
        {
            // Create a websocket frame around the data to send
            byte[] frameMessage = Helpers.GetFrameFromString(Data);

            // Send the framed message to the in client
            Client.GetSocket().Send(frameMessage);

            // Call the on send message callback event 
            if (OnSendMessage == null) throw new Exception("Server error: event OnSendMessage is not bound!");
            OnSendMessage(this, new OnSendMessageHandler(Client, Data));
        }
        
        public void BroadcastMessage(string Data)
        {
            foreach(Client client in _clients)
            {
                SendMessage(client, Data);
            }
        }

        /// <summary>Called after a message was sent</summary>
        public event EventHandler<OnSendMessageHandler> OnSendMessage;

        /// <summary>Called when a client was connected to the server (after handshake)</summary>
        public event EventHandler<OnClientConnectedHandler> OnClientConnected;

        /// <summary>Called when a message was received from a connected client</summary>
        public event EventHandler<OnMessageReceivedHandler> OnMessageReceived;

        /// <summary>Called when a client disconnected</summary>
        public event EventHandler<OnClientDisconnectedHandler> OnClientDisconnected;

        #endregion
        
        
        
        # region TayTay
        
        /// <summary>
        /// Parses HTTP headers from a request string.
        /// </summary>
        private Dictionary<string, string> ParseHeaders(string request)
        {
            var headers = new Dictionary<string, string>();
            var lines = request.Split(new[] { "\r\n" }, StringSplitOptions.None);
            foreach (var line in lines)
            {
                //Log.Debug(line);
                int separatorIndex = line.IndexOf(':');
                if (separatorIndex > 0)
                {
                    string name = line.Substring(0, separatorIndex).Trim();
                    string value = line.Substring(separatorIndex + 1).Trim();
                    headers[name] = value;
                }
                /*else
                {
                    string value = line.Trim();
                    headers["Method"] = value;
                }*/
                
            }

            return headers;
        }

        /// <summary>
        /// Validates the API key.
        /// </summary>
        private bool ValidateApiKey(string apiKey)
        {
            return true;
            // Implement your API key validation logic here
            return apiKey == TTAdmin.Instance.ApiKey.KeyString; // Example validation
        }
        
        # endregion
    }