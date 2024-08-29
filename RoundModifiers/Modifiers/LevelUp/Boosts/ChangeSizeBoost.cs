using System;
using Exiled.API.Features;
using TTCore.Extensions;
using TTCore.Handlers;
using UnityEngine;

namespace RoundModifiers.Modifiers.LevelUp.Boosts;

public class ChangeSizeBoost : Boost
{
    public float NewScale { get; set; }
    
    public ChangeSizeBoost(float newScale = 0.5f, Tier rarity = Tier.Epic) : base(rarity)
    {
        NewScale = newScale;
    }

    public override bool AssignBoost(Player player)
    {
        if(HasBoost.ContainsKey(player.NetId)) return false;
        return ApplyBoost(player);
    }

    public override bool ApplyBoost(Player player)
    {
        HasBoost[player.NetId] = true;
        //player.ChangeSize(NewScale);
        
        if (NewScale * player.Scale.y <= 1.1f)
        {
            player.Scale = player.Scale * NewScale;
        }
        else
        {
            player.Scale = Vector3.one * 1.1f;
        }
        player.MaxHealth *= NewScale;
        player.Health *= NewScale;
        if (NewScale >= 1)
        {
            player.Position += Vector3.up;
        }
        
        return true;
    }

    public override string GetName()
    {
        return "Size: "+NewScale;
    }

    public override string GetDescription()
    {
        return "Change the player's size to "+NewScale;
    }
}