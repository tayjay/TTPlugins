using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Linq;
using CommandSystem;
using Exiled.API.Extensions;
using Exiled.API.Features.Pools;
using Exiled.Permissions.Extensions;

namespace RoundModifiers.Commands.RemoteAdmin;

public class ModHideCommand : ICommand
{
    public bool Execute(ArraySegment<string> arguments, ICommandSender sender, [UnscopedRef] out string response)
    {
        if (sender.CheckPermission("RoundEvents") || sender.CheckPermission("modifier") || sender.CheckPermission("RoundEvents*"))
        {
            List<string> modifiers = ListPool<string>.Pool.Get();
            
            if (arguments.Count == 0)
            {
                //Modifiy this round
                RoundModifiers.Instance.RoundManager.ModifiersHidden = !RoundModifiers.Instance.RoundManager.ModifiersHidden;
                if(RoundModifiers.Instance.RoundManager.ModifiersHidden)
                    response = "Hiding modifiers for the current round.";
                else
                    response = "Showing modifiers for the current round.";

            } else if (arguments.Count == 1 && arguments.At(0).ToLower() == "next")
            {
                //Modify next round
                RoundModifiers.Instance.RoundManager.NextModifiersHidden = !RoundModifiers.Instance.RoundManager.NextModifiersHidden;
                if(RoundModifiers.Instance.RoundManager.NextModifiersHidden)
                    response = "Hiding modifiers for the next round.";
                else
                    response = "Showing modifiers for the next round.";
                
            } else
            {
                ListPool<string>.Pool.Return(modifiers);
                response = "Usage: mod hide [next]";
                return false;
            }
            ListPool<string>.Pool.Return(modifiers);
            return true;
        }
        response = "You do not have permission to use this random command.";
        return false;
    }

    public string Command { get; } = "hide";
    public string[] Aliases { get; } = new[] {"redact"};
    public string Description { get; } = "Hide or show the modifiers for the current or next round.";
    public bool SanitizeResponse => true;
}