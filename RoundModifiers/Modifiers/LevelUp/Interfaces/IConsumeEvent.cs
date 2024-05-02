using Exiled.Events.EventArgs.Scp0492;

namespace RoundModifiers.Modifiers.LevelUp.Interfaces;

public interface IConsumeEvent
{
    void OnConsumedCorpse(ConsumedCorpseEventArgs ev);
}