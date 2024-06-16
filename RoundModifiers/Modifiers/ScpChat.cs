using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using RoundModifiers.API;
using UnityEngine;
using VoiceChat;
using VoiceChat.Networking;

namespace RoundModifiers.Modifiers;

public class ScpChat : Modifier
{
    //false means they are talking like default in scp chat, true means they are talking in proximity chat.
    public Dictionary<Player, bool> ProximityChatting { get; set; }

    public bool CanChangeState => RoundModifiers.Instance.Config.ScpChat_CanChangeState;

    public void OnVoiceChatting(VoiceChattingEventArgs ev)
    {
        if(ev.VoiceMessage.Channel!=VoiceChatChannel.ScpChat) return;
        if(!ProximityChatting[ev.Player]) return;
        VoiceMessage message = ev.VoiceMessage with
        {
            Channel = VoiceChatChannel.Proximity
        };
        foreach (Player player in Player.List.Where(
                     player => Vector3.Distance(player.Position, ev.Player.Position) < 30))
        {
            if(player.Role.Team == Team.SCPs) continue;
            player.Connection.Send(message);
        }
    }

    public void OnSpawned(SpawnedEventArgs ev)
    {
        if(ev.Player.Role.Team == Team.SCPs)
        {
            if(ev.Player.Role==RoleTypeId.Scp079) return;
            ProximityChatting.Add(ev.Player, true);
            if(CanChangeState)
                ev.Player.Broadcast(5, "<color=green>Proximity Chat Enabled. Press Left-Alt to disable.</color>");
            else
                ev.Player.Broadcast(5, "<color=green>Proximity Chat Enabled.</color>");
        }
    }

    public void OnPressNoClip(TogglingNoClipEventArgs ev)
    {
        if(!CanChangeState) return;
        if(ev.Player.Role==RoleTypeId.Scp079) return;
        if (ev.Player.IsNoclipPermitted)
        {
            //Need special logic to still allow no clip
            if (ev.Player.Role is FpcRole fpcRole)
            {
                if (fpcRole.MoveState != PlayerMovementState.Sneaking)
                {
                    ev.Player.Broadcast(2, "<color=red>Sneak and press no-clip to toggle.</color>");
                    return;
                }
            }
        }
        
        if (ev.Player.Role.Team == Team.SCPs)
        {
            if (ProximityChatting[ev.Player])
            {
                ProximityChatting[ev.Player] = false;
                ev.Player.Broadcast(2, "<color=red>Proximity Chat Disabled. Press Left-Alt to enable.</color>");
            }
            else
            {
                ProximityChatting[ev.Player] = true;
                ev.Player.Broadcast(2, "<color=green>Proximity Chat Enabled. Press Left-Alt to disable.</color>");
            }
        }
        
    }
    
    protected override void RegisterModifier()
    {
        Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
        Exiled.Events.Handlers.Player.Spawned += OnSpawned;
        Exiled.Events.Handlers.Player.TogglingNoClip += OnPressNoClip;

        ProximityChatting = DictionaryPool<Player, bool>.Pool.Get();
    }

    protected override void UnregisterModifier()
    {
        Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;
        Exiled.Events.Handlers.Player.Spawned -= OnSpawned;
        Exiled.Events.Handlers.Player.TogglingNoClip -= OnPressNoClip;
        
        DictionaryPool<Player, bool>.Pool.Return(ProximityChatting);
    }

    public override ModInfo ModInfo { get; } = new ModInfo
    {
        Name = "ScpChat",
        Aliases = new []{"scpchat", "scpchatting"},
        FormattedName = "<color=red>Scp Chat</color>",
        Description = "SCPs can talk to human players in proximity chat.",
        MustPreload = false,
        Balance = -1,
        Impact = ImpactLevel.MinorGameplay,
        Category = Category.Voice
    };
}