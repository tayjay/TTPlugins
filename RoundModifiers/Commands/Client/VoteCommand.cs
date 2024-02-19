using System;
using System.Linq;
using CommandSystem;
using Exiled.API.Features;
using RoundModifiers.API;

namespace RoundModifiers.Commands.Client
{
    [CommandHandler(typeof(ClientCommandHandler))]
    public class VoteCommand : ICommand
    {
        public bool Execute(ArraySegment<string> arguments, ICommandSender sender, out string response)
        { 
            if(RoundModifiers.Instance.Config.IsEnabled==false)
            {
                response = "Round Modifiers are disabled.";
                return true;
            }
            if(RoundModifiers.Instance.Config.EnableVoting==false)
            {
                response = "Voting is disabled.";
                return true;
            }

            Player player = Player.Get(sender);

            if (arguments.Count > 1)
            {
                //a vote was specified
                string selection = arguments.At(0).ToLower();
                if (selection.Equals("none"))
                {
                    //remove vote
                    return RoundModifiers.Instance.RoundManager.RemoveVote(player, out response);
                } else if (selection.Equals("random"))
                {
                    //random vote
                    return RoundModifiers.Instance.RoundManager.RandomVote(player, out response);
                } else if(RoundModifiers.Instance.TryGetModifier(selection, out var modifier))
                {
                    //vote for modifier
                    return RoundModifiers.Instance.RoundManager.TakeVote(player, modifier.ModInfo, out response);
                }
                else
                {
                    response = "Invalid vote.";
                    return false;
                }
            }

            response = RoundModifiers.Instance.RoundManager.GetVoteInfo(player);
            return true;
        }

        public string Command { get; } = "vote";
        public string[] Aliases { get; } = {"v"};
        public string Description { get; } = "Vote if you want a modifier this round.";
    }
}