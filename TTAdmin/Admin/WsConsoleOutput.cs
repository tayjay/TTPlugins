using System;
using System.Collections.Generic;
using System.Linq;
using MEC;
using PluginAPI.Core;
using TTAdmin.WebNew;
using TTCore.API;
using UnityEngine;
using Utf8Json;

namespace TTAdmin.Admin;

/*public class WsConsoleOutput : IOutput
{
    public static string[] BlacklistedStrings = new[]
    {
        "[TTAdmin]",
        "ServerOutput.HeartbeatEntry",
    };
    
    
    public void Print(string text)
    {
        if (BlacklistedStrings.Any(text.Contains))
            return;
        if (TTAdmin.Instance.SubscriptionHandler.GetClientsWithLogSubscription().Count == 0) return;
        Timing.CallDelayed(Timing.WaitForOneFrame, () =>
        {
            string response = JsonSerializer.ToJsonString(new ConsoleOutputData()
            {
                Text = text
            });
            TTAdmin.Instance.SubscriptionHandler.GetClientsWithLogSubscription().ForEach(client =>
            {
                //Log.Debug("Sending log to client...");
                WsServer.Server.SendMessage(client, response);
            });
        });
    }

    public void Print(string text, ConsoleColor c)
    {
        Print(text);
    }

    public void Print(string text, ConsoleColor c, Color rgbColor)
    {
        Print(text);
    }

    public void Register()
    {
        Setup();
    }

    public void Unregister()
    {
        Timing.KillCoroutines(_coroutineHandle);
        if(ServerConsole.ConsoleOutputs !=null && ServerConsole.ConsoleOutputs.Contains(this))
            ServerConsole.ConsoleOutputs?.Remove(this);
    }
    
    CoroutineHandle _coroutineHandle;
    public void Setup()
    {
        _coroutineHandle = Timing.RunCoroutine(AttemptConnectionToServerConsole());
    }
    
    public IEnumerator<float> AttemptConnectionToServerConsole()
    {
        while(ServerConsole.ConsoleOutputs == null)
        {
            yield return Timing.WaitForOneFrame;
        }
        ServerConsole.ConsoleOutputs.Add(this);
        Log.Debug("Connected logs to websocket server!");
    }
    
    public class ConsoleOutputData
    {
        public string Text { get; set; }
    }
}*/