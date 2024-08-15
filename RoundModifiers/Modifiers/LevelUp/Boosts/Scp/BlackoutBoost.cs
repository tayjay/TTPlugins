using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.LevelUp.Interfaces;
using TTCore.Events.EventArgs;

namespace RoundModifiers.Modifiers.LevelUp.Boosts.Scp;

public class BlackoutBoost : Boost, IKeybindAbility
{
    public BlackoutBoost() : base(Boosts.Tier.Rare)
    {
    }

    public override bool AssignBoost(Player player)
    {
        if(!player.IsScp) return false;
        if(HasBoost.ContainsKey(player.NetId)) return false;
        if(Round.ElapsedTime.TotalMinutes < 5) return false;
        return ApplyBoost(player);
    }

    public override bool ApplyBoost(Player player)
    {
        HasBoost.Add(player.NetId, true);
        return true;
    }

    public override string GetName()
    {
        return "<color=black>Blackout</color>";
    }

    public override string GetDescription()
    {
        return "Light seems to fade in your presence.";
    }

    public void OnPressKeybind(TogglingNoClipEventArgs ev)
    {
        if(!HasBoost.ContainsKey(ev.Player.NetId)) return;
        if (!ev.Player.IsScp)
        {
            HasBoost.Remove(ev.Player.NetId);
            return;
        }
        if(ev.Player.CurrentRoom.AreLightsOff) return;
        ev.Player.CurrentRoom.TurnOffLights(60);
    }
}