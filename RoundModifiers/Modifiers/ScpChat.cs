using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using PlayerRoles.Voice;
using RoundModifiers.API;
using UnityEngine;
using VoiceChat;
using VoiceChat.Networking;

namespace RoundModifiers.Modifiers;

public class ScpChat : Modifier
{
    //false means they are talking like default in scp chat, true means they are talking in proximity chat.
    public Dictionary<Player, bool> ProximityChatting { get; set; }

    public void OnVoiceChatting(VoiceChattingEventArgs ev)
    {
        if(ev.VoiceMessage.Channel!=VoiceChatChannel.ScpChat) return;
        if (ev.Player.Role == RoleTypeId.Scp079) return;
        if(!ProximityChatting[ev.Player]) return;
        if(!Round.InProgress) return;
        //ev.VoiceModule.CurrentChannel = VoiceChatChannel.Proximity;
        
        foreach (Player player in Player.List.Where(
                     player => player.Role.Team != Team.SCPs))
        {
            VoiceMessage message = ev.VoiceMessage with
            {
                Channel = VoiceChatChannel.Proximity
            };
            if(player.Role == RoleTypeId.Spectator && ev.Player.CurrentSpectatingPlayers.Contains(player)) continue;
            if(player.ReferenceHub.roleManager.CurrentRole is not IVoiceRole voiceRole) continue;
            if(voiceRole.VoiceModule.ValidateReceive(message.Speaker, VoiceChatChannel.Proximity) == VoiceChatChannel.None) continue;
            if(Vector3.Distance(message.Speaker.transform.position, player.Position) > 20f) continue;
            ev.VoiceModule.CurrentChannel = VoiceChatChannel.Proximity;
            player.Connection.Send(message);
        }
        ev.VoiceModule.CurrentChannel = VoiceChatChannel.ScpChat;
    }

    public void OnSpawned(SpawnedEventArgs ev)
    {
        if(ev.Player.Role.Team == Team.SCPs)
        {
            if(ev.Player.Role==RoleTypeId.Scp079) return;

            if (CanChangeState)
            {
                ev.Player.ClearBroadcasts();
                ev.Player.Broadcast(5, "<color=green>Proximity Chat Disabled. Press Left-Alt to enable.</color>");
                ProximityChatting.Add(ev.Player, false);
            }
            else
            {
                ev.Player.ClearBroadcasts();
                ProximityChatting.Add(ev.Player, true);
                ev.Player.Broadcast(5, "<color=green>Proximity Chat Enabled.</color>");
            }
                
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
        
        if (ev.Player?.Role.Team == Team.SCPs)
        {
            if (ProximityChatting[ev.Player])
            {
                ProximityChatting[ev.Player] = false;
                ev.Player.Broadcast(1, "<color=red>Proximity Chat Disabled. Press Left-Alt to enable.</color>");
            }
            else
            {
                ProximityChatting[ev.Player] = true;
                ev.Player.Broadcast(1, "<color=green>Proximity Chat Enabled. Press Left-Alt to disable.</color>");
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


    public static Config ScpChatConfig => RoundModifiers.Instance.Config.ScpChat;
    public bool CanChangeState => ScpChatConfig.CanChangeState;


    public class Config : ModConfig
    {
        [Description("Whether SCPs can change the state of the ability. Default is true.")]
        public bool CanChangeState { get; set; } = true;
    }
}