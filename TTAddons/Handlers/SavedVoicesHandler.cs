using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Roles;
using Exiled.Events.EventArgs.Player;
using Exiled.Events.EventArgs.Scp939;
using MEC;
using TTCore.API;
using UnityEngine;
using VoiceChat;
using VoiceChat.Codec;
using VoiceChat.Codec.Enums;
using VoiceChat.Networking;

namespace TTAddons.Handlers
{
    public class SavedVoicesHandler : IRegistered
    {
        public Dictionary<string,PlaybackBuffer> SavedVoices = new Dictionary<string, PlaybackBuffer>();


        public void OnDogVoicePlay(PlayingVoiceEventArgs ev)
        {
            PlaybackBuffer pb = new PlaybackBuffer(24000, true);
            SavedVoices[ev.Player.UserId] = pb;
            Log.Debug("Playing voice...");
        }

        public void OnVoiceChatting(VoiceChattingEventArgs ev)
        {
            if (ev.VoiceMessage.Channel != VoiceChatChannel.Mimicry)
                return;
            float[] buffer = new float[ev.VoiceMessage.Data.Length];
            ev.VoiceModule.Decoder.Decode(ev.VoiceMessage.Data, ev.VoiceMessage.Data.Length, buffer);
            SavedVoices[ev.Player.UserId].Reorganize();
            SavedVoices[ev.Player.UserId].Write(buffer, buffer.Length, 0);
            Log.Debug(SavedVoices[ev.Player.UserId].WriteHead);
        }

        public void Debug(TogglingNoClipEventArgs ev)
        {
            Log.Debug("Voice count: " + SavedVoices.Count);
            foreach (var voice in SavedVoices)
                Log.Debug(voice.Key + " " + voice.Value.Buffer.Length + " " + voice.Value.Length);
        }

        public void SendVoice(Player player)
        {
            if (!SavedVoices.ContainsKey(player.UserId))
                return;
            byte[] encodedBuffer = new byte[512];
            OpusEncoder encoder = new OpusEncoder(OpusApplicationType.Voip);
            int length = encoder.Encode(SavedVoices[player.UserId].Buffer, encodedBuffer);
            VoiceTransceiver.ServerReceiveMessage(player.Connection, new VoiceMessage
            {
                Speaker = player.ReferenceHub,
                Channel = VoiceChatChannel.Mimicry,
                Data = encodedBuffer,
                DataLength = length
            });
            player.Connection.Send<VoiceMessage>(new VoiceMessage
            {
                Speaker = player.ReferenceHub,
                Channel = VoiceChatChannel.Mimicry,
                Data = encodedBuffer,
                DataLength = length
            });
        }

        public void PreviewVoice(Player player)
        {
            if (!SavedVoices.ContainsKey(player.UserId))
                return;
            /*byte[] encodedBuffer = new byte[512];
            OpusEncoder encoder = new OpusEncoder(OpusApplicationType.Voip);
            int length = encoder.Encode(SavedVoices[player.UserId].Buffer, encodedBuffer);
            player.Connection.Send<VoiceMessage>(new VoiceMessage
            {
                Speaker = player.ReferenceHub,
                Channel = VoiceChatChannel.Mimicry,
                Data = encodedBuffer,
                DataLength = length
            });*/
            SavedVoices[player.UserId].Reorganize();
            SavedVoices[player.UserId].ReadHead = 0;
            senderPlayback = new PlaybackBuffer(24000, true);
            senderPlayback.WriteHead = 0;
            Timing.RunCoroutine(Preview(player));
        }

        private int AllowedSamples = 0;
        private PlaybackBuffer senderPlayback = new PlaybackBuffer(24000, true);
        OpusEncoder encoder = new OpusEncoder(OpusApplicationType.Voip);

        public IEnumerator<float> Preview(Player player)
        {
            while (SavedVoices[player.UserId].Length > SavedVoices[player.UserId].ReadHead)
            {
                AllowedSamples += Mathf.CeilToInt(Time.deltaTime * 48000);
                int readLength = Mathf.Min(AllowedSamples, SavedVoices[player.UserId].Length);
                if (readLength > 0)
                {
                    SavedVoices[player.UserId].ReadTo(senderPlayback.Buffer, readLength, senderPlayback.WriteHead);
                    senderPlayback.WriteHead += readLength;
                }
                AllowedSamples = 0;
                byte[] encodedBuffer = new byte[512];
                int length = encoder.Encode(SavedVoices[player.UserId].Buffer, encodedBuffer);
                player.Connection.Send<VoiceMessage>(new VoiceMessage
                {
                    Speaker = player.ReferenceHub,
                    Channel = VoiceChatChannel.Mimicry,
                    Data = encodedBuffer,
                    DataLength = length
                });
                Log.Debug("ReadHead: "+SavedVoices[player.UserId].ReadHead+" WriteHead: "+SavedVoices[player.UserId].WriteHead+" Length: "+SavedVoices[player.UserId].Length);
                yield return Timing.WaitForOneFrame;
            }
            
        }

        public void Register()
        {
            Exiled.Events.Handlers.Scp939.PlayingVoice += OnDogVoicePlay;
            Exiled.Events.Handlers.Player.VoiceChatting += OnVoiceChatting;
            Exiled.Events.Handlers.Player.TogglingNoClip += Debug;
        }

        public void Unregister()
        {
            Exiled.Events.Handlers.Scp939.PlayingVoice -= OnDogVoicePlay;
            Exiled.Events.Handlers.Player.VoiceChatting -= OnVoiceChatting;
            Exiled.Events.Handlers.Player.TogglingNoClip -= Debug;
        }
    }
}