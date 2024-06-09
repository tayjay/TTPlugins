using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using CommandSystem;
using Exiled.Events.Features;
using Exiled.Permissions.Extensions;
using HarmonyLib;
using MEC;

namespace TTCore.Commands.RemoteAdmin;

public class DiagnosticsCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        /*if (!sender.CheckPermission("diagnostics"))
        {
            response = "You do not have permission to run this command.";
            return false;
        }

        try
        {
            Timing.RunCoroutine(RunDiagnostics(sender));
            response = "Running diagnostics...";
            return true;
        }
        catch (Exception e)
        {
            response = $"An error occurred while running diagnostics: {e.Message}";
            return false;
        }*/
        throw new NotImplementedException();
    }
    
    public IEnumerator<float> RunDiagnostics(ICommandSender sender)
    {
        string message = "Diagnostics:\n";
        
        
        sender.Respond(message, true);
        yield return Timing.WaitForOneFrame;
    }

    public string Command { get; } = "diagnostics";
    public string[] Aliases { get; } = { "diag" };
    public string Description { get; }
    public bool SanitizeResponse => true;
}