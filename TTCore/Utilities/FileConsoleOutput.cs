using System;
using System.Collections.Generic;
using System.IO;
using Exiled.API.Features;
using MEC;
using TTCore.API;
using UnityEngine;

namespace TTCore.Utilities;

public class FileConsoleOutput : IOutput, IRegistered 
{
    public string CurrentLogFile { get; private set; }
    
    CoroutineHandle _coroutineHandle;

    public void Setup()
    {
        StartNewLog();
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

    public void StartNewLog()
    {
        try
        {
            CurrentLogFile = $"TTCore/ConsoleOutput/ConsoleOutput-{DateTime.Now:s}.txt";
            CurrentLogFile = CurrentLogFile.Replace(':', '-');
            Log.Debug("Current log file: " + CurrentLogFile);
            if(!Directory.Exists("TTCore"))
            {
                Directory.CreateDirectory("TTCore");
            }
            if(!Directory.Exists("TTCore/ConsoleOutput"))
            {
                Directory.CreateDirectory("TTCore/ConsoleOutput");
            }
            Log.Debug("Creating new log file...");
            File.AppendAllText(CurrentLogFile, $"Log file created at {DateTime.Now:u}" + Environment.NewLine);
            Log.Debug("Log file created!");
            /*if (!File.Exists(CurrentLogFile))
            {
                
            }*/
        } catch (Exception e)
        {
            Log.Error("Error creating log file: " + e.Message);
        }
        Log.Debug("Log file started!");
    }
    
    public void OnRoundRestart()
    {
        File.AppendAllText(CurrentLogFile, $"Round Restarted at {DateTime.Now:s}" + Environment.NewLine);
        StartNewLog();
    }

    public void OnServerStart()
    {
        StartNewLog();
    }
    
    public void Print(string text)
    {
        File.AppendAllText(CurrentLogFile, $"[{DateTime.Now:u}] {text}" + Environment.NewLine);
    }

    public void Print(string text, ConsoleColor c)
    {
        Print(text);
    }

    public void Print(string text, ConsoleColor c, Color rgbColor)
    {
        Print(text, c);
    }

    public void Register()
    {
        Exiled.Events.Handlers.Server.RestartingRound += OnRoundRestart;
        
        Setup();
    }

    public void Unregister()
    {
        Exiled.Events.Handlers.Server.RestartingRound -= OnRoundRestart;
        Timing.KillCoroutines(_coroutineHandle);
        if(ServerConsole.ConsoleOutputs!=null)
            ServerConsole.ConsoleOutputs.Remove(this);
    }
}