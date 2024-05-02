using System;
using Exiled.Events.EventArgs.Scp173;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs.Scp;

public class Scp173XP : ScpXP, IBlinkEvent
{
    public void OnBlinkingRequest(BlinkingRequestEventArgs ev)
    {
        GiveXP(ev.Player, Math.Min(10*ev.Targets.Count, 30));
    }
}