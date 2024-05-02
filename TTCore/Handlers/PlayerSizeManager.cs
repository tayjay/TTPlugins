using System;
using System.Collections.Generic;
using Exiled.API.Features;
using Exiled.API.Features.Pools;
using MEC;
using PlayerRoles;
using PlayerRoles.FirstPersonControl;
using UnityEngine;

namespace TTCore.Handlers
{
    public class PlayerSizeManager
    {
        //Need a list of player IDs and their associated sizes
        public Dictionary<int, Vector3> PlayerSizes;

        public PlayerSizeManager()
        {
            PlayerSizes = DictionaryPool<int, Vector3>.Pool.Get();
        }

        //Need a method to get the size of a player
        public Vector3 GetSize(Player player)
        {
            int playerId = player.Id;
            if (PlayerSizes.ContainsKey(playerId))
                return PlayerSizes[playerId];
            return Vector3.one;
        }

        public void SetSize(Player player, float size)
        {
            SetSize(player, new Vector3(size, size, size));
        }

        //Need a method to set the size of a player
        public void SetSize(Player player, Vector3 size)
        {
            int playerId = player.Id;
            PlayerSizes[playerId] = size;
            
            Log.Debug(player.Nickname + " size set to " + size + "!");
            //ChangePlayerScale(player, size);
            MEC.Timing.KillCoroutines("Resize"+playerId);
            MEC.Timing.RunCoroutine(ChangeOverTime(player, size), "Resize"+playerId);
            //Timing.RunCoroutine(MonitorPlayerSize(player));
        }

        //Need a method to reset the size of a player
        public void ResetSize(Player player)
        {
            int playerId = player.Id;
            /*if (PlayerSizes.ContainsKey(playerId)) // Modifies enumerable
                PlayerSizes.Remove(playerId);*/
            MEC.Timing.KillCoroutines("Resize"+playerId);
            //ChangePlayerScale(player, Vector3.one);
            player.Scale = Vector3.one;
            Log.Debug(player.Nickname + " size set to 1.0!");
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
            PlayerSizes.Clear();
        }

        private IEnumerator<float> ChangeOverTime(Player player, Vector3 targetSize)
        {
            /*while(Math.Abs(player.Scale.y - targetSize) > 0.08)
            {
                float currentSize = player.Scale.y;
                float newSize = Mathf.Lerp(currentSize, targetSize, Timing.DeltaTime);
                player.Position += new Vector3(0, (newSize - currentSize)/2, 0);
                ChangePlayerScale(player, newSize);
                yield return Timing.WaitForOneFrame;
            }*/
            float startTime = Time.time;
            while (Vector3.Distance(player.Scale, targetSize) > Timing.DeltaTime+0.01f)
            {
                if (Time.time - startTime > 5.0f)
                { 
                    break;
                }
                float currX = player.Scale.x;
                float currY = player.Scale.y;
                float currZ = player.Scale.z;
                float newX = Mathf.Lerp(currX, targetSize.x, Timing.DeltaTime);
                float newY = Mathf.Lerp(currY, targetSize.y, Timing.DeltaTime);
                float newZ = Mathf.Lerp(currZ, targetSize.z, Timing.DeltaTime);
                if(newY>currY)
                    player.Position += new Vector3(0, (newY - currY)/2, 0);
                
                ChangePlayerScale(player, new Vector3(newX, newY, newZ));
                
                yield return Timing.WaitForOneFrame;
            }
            ChangePlayerScale(player, targetSize);
        }

        /*private IEnumerator<float> MonitorPlayerSize(Player player)
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
            
        }*/
        
        
        private void ChangePlayerScale(Player player, Vector3 size)
        {
            float oldSize = player.Scale.y;
            //Change their physical size first
            player.Scale = size;
            
            
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