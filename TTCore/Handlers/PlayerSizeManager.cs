using System;
using System.Collections.Generic;
using Exiled.API.Features;
using MEC;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace TTCore.Handlers
{
    public class PlayerSizeManager
    {
        //Need a list of player IDs and their associated sizes
        public Dictionary<int, float> PlayerSizes;

        public PlayerSizeManager()
        {
            PlayerSizes = new Dictionary<int, float>();
        }

        //Need a method to get the size of a player
        public float GetSize(Player player)
        {
            int playerId = player.Id;
            if (PlayerSizes.ContainsKey(playerId))
                return PlayerSizes[playerId];
            return 1.0f;
        }

        //Need a method to set the size of a player
        public void SetSize(Player player, float size)
        {
            int playerId = player.Id;
            PlayerSizes[playerId] = size;
            
            Log.Info(player.Nickname + " size set to " + size + "!");
            //ChangePlayerScale(player, size);
            MEC.Timing.KillCoroutines("Resize"+playerId);
            MEC.Timing.RunCoroutine(ChangeOverTime(player, size), "Resize"+playerId);
            //Timing.RunCoroutine(MonitorPlayerSize(player));
        }

        //Need a method to reset the size of a player
        public void ResetSize(Player player)
        {
            int playerId = player.Id;
            if (PlayerSizes.ContainsKey(playerId))
                PlayerSizes.Remove(playerId);
            MEC.Timing.KillCoroutines("Resize"+playerId);
            ChangePlayerScale(player, 1.0f);
            Log.Info(player.Nickname + " size set to 1.0!");
        }
        
        public void ResetAll()
        {
            foreach (int playerSizesKey in PlayerSizes.Keys)
            {
                Player player = Player.Get(playerSizesKey);
                if (player != null)
                {
                    ResetSize(player);
                }
            }
        }

        private IEnumerator<float> ChangeOverTime(Player player, float targetSize)
        {
            while(Math.Abs(player.Scale.y - targetSize) > 0.08)
            {
                float currentSize = player.Scale.y;
                float newSize = Mathf.Lerp(currentSize, targetSize, Timing.DeltaTime);
                player.Position += new Vector3(0, (newSize - currentSize)/2, 0);
                ChangePlayerScale(player, newSize);
                yield return Timing.WaitForOneFrame;
            }
        }

        private IEnumerator<float> MonitorPlayerSize(Player player)
        {
            while (Math.Abs(GetSize(player) - 1f) > 0.001f)
            {
                FirstPersonMovementModule fpcModule = ((IFpcRole)player.ReferenceHub.roleManager.CurrentRole).FpcModule;
                if (fpcModule.CurrentMovementState == PlayerMovementState.Sneaking || fpcModule.CurrentMovementState == PlayerMovementState.Crouching)
                {
                    if (player.Scale.y > 1.0f)
                    {
                        MEC.Timing.KillCoroutines("Resize"+player.Id);
                        Timing.RunCoroutine(ChangeOverTime(player, 1f),"Resize"+player.Id);
                    }
                        
                }
                else
                {
                    if (Math.Abs(player.Scale.y - GetSize(player)) > 0.01f)
                    {
                        MEC.Timing.KillCoroutines("Resize"+player.Id);
                        Timing.RunCoroutine(ChangeOverTime(player, GetSize(player)),"Resize"+player.Id);
                    }
                }
                yield return Timing.WaitForSeconds(1f);
            }
            
        }
        
        
        private void ChangePlayerScale(Player player, float size)
        {
            float oldSize = player.Scale.y;
            //Change their physical size first
            if (size > player.Scale.y)
            {
                //Need to move the player up as to not clip through the floor
                float newY = (size - player.Scale.y)/2;
                //player.Position += new Vector3(0, newY, 0);
            }
            player.Scale = new Vector3(size, size, size);
            
            
            //For balance also need to change their stats
            //First their health
            ///////player.MaxHealth = (player.MaxHealth/oldSize)*size;
            
            //Then for SCPs, their attack range
            if (player.Role.Team == Team.SCPs)
            {
                
            }

        }
    }
}