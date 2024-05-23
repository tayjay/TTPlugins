using Exiled.API.Features;
using Exiled.API.Features.Items;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;
using UnityEngine;

namespace RoundModifiers.Modifiers.WeaponStats;

public class SlipperyStat : Stat, IShooting
{

    public void OnShooting(ShootingEventArgs ev)
    {
        
    }

    public void OnShot(ShotEventArgs ev)
    {
        float chance = Random.Range(0, 100);
        if (chance < 2)
        {
            ev.Player.DropHeldItem(true);
            ev.Damage *= 10;
        }
        else
        {
            ev.Damage *= 2f;
        }
    }

    public override string Name => "Slippery";
    public override int Rarity => 3;
    public override string Description => "This weapon has a chance to slip out of your hands and deal more damage";

}