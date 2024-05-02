using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using VoiceChat;

namespace RoundModifiers.Modifiers.LevelUp.Boosts.Scp;

public class AppearHumanBoost : Boost
{
    public AppearHumanBoost() : base(Tier.Legendary)
    {
    }

    public override bool AssignBoost(Player player)
    {
        if(!player.IsScp) return false;
        if(Round.ElapsedTime.TotalMinutes < 15) return false;
        return ApplyBoost(player);
    }

    public override bool ApplyBoost(Player player)
    {
        player.ChangeAppearance(RoleTypeId.ClassD);
        player.VoiceChannel = VoiceChatChannel.Proximity; //todo: Review if this works or breaks balance. Stops SCP from talking to allys.
        return true;
    }

    public override string GetName()
    {
        return "Appear Human";
    }

    public override string GetDescription()
    {
        return "Appear as a human when you level up.";
    }
}