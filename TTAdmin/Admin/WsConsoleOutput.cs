using System;
using System.Collections.Generic;
using System.Linq;
using MEC;
using TTAdmin.WebNew;
using TTCore.API;
using UnityEngine;
using Utf8Json;

namespace TTAdmin.Admin;

public class WsConsoleOutput : IOutput
{
    public static string[] BlacklistedStrings = new[]
    {
        "[TTAdmin]",
    };
    
    
    public void Print(string text)
    {
        if (BlacklistedStrings.Any(text.Contains))
            return;
        
        Timing.CallDelayed(Timing.WaitForOneFrame, () =>
        {
            string response = JsonSerializer.ToJsonString(new ConsoleOutputData()
            {
                Text = text
            });
            TTAdmin.Instance.SubscriptionHandler.GetClientsWithSubscription("console").ForEach(client =>
            {
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
    }
    
    public class ConsoleOutputData
    {
        public string Text { get; set; }
    }
}