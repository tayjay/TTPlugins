using System;
using CommandSystem;
using Exiled.API.Features;
using Exiled.API.Features.Roles;

namespace TTAddons.Commands.Client
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class VoiceCommand : ICommand
    {
        // cmdbind <key> .voice <command>
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        {
            if(arguments.Count == 0)
            {
                response = "Usage: voice <command>";
                return false;
            }
            
            switch (arguments.At(0).ToLower())
            {
                case "play":
                    // Play saved voice
                    return PlayVoice(0, Player.Get(sender), out response);
                    break;
                case "preview":
                    // Preview voice
                    return PreviewVoice(0, Player.Get(sender), out response);
                    break;
                case "save":
                    // Save voice
                    response = "Saving voice...";
                    return true;
                    break;
                case "test":
                    // Test voice
                    Player player = Player.Get(sender);
                    if (player == null)
                    {
                        response = "Player not found.";
                        return false;
                    }

                    Scp939Role role = (Scp939Role)player.Role;
                    Log.Info("Mimicry sample rate: "+role.MimicryRecorder.Transmitter._samplesPerSecond);
                    response = "Testing voice...";
                    return true;
                default:
                    response = "Usage: voice <command>";
                    return false;
            }
        }
        
        public bool PlayVoice(int id, Player player, out string response)
        {
            // Play saved voice
            TTAddons.Instance.SavedVoicesHandler.SendVoice(player);
            
            
            response = "Playing voice...";
            return true;
        }
        
        public bool PreviewVoice(int id, Player player, out string response)
        {
            // Preview voice
            
            TTAddons.Instance.SavedVoicesHandler.PreviewVoice(player);
            response = "Previewing voice...";
            return true;
        }
        
        

        public string Command { get; } = "voice";
        public string[] Aliases { get; } 
        public string Description { get; } = "Voice command.";
    }
}