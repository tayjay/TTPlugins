using Exiled.Events.EventArgs.Scp0492;
using RoundModifiers.Modifiers.LevelUp.Interfaces;

namespace RoundModifiers.Modifiers.LevelUp.XPs.Scp;

public class Scp0492XP : ScpXP, IConsumeEvent
{
    public void OnConsumedCorpse(ConsumedCorpseEventArgs ev)
    {
        GiveXP(ev.Player,ev.ConsumeHeal);
    }
}