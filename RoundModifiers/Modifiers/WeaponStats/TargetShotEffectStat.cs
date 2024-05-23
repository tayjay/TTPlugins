using Exiled.API.Enums;
using Exiled.Events.EventArgs.Player;
using RoundModifiers.Modifiers.WeaponStats.Interfaces;
using TTCore.HUDs;

namespace RoundModifiers.Modifiers.WeaponStats;

public class TargetShotEffectStat : Stat, IShooting
{
    public override string Name { get; } = "Give Effect on Shot";
    public override int Rarity { get; } = 3;

    public override string Description =>
        $"Gives the effect:"+ (EffectFriendly ? $" {FriendlyEffectType} to allies" : "") + (EffectEnemy ? $" {EnemyEffectType} to enemies" : "");
    
    public bool EffectFriendly { get; }
    public EffectType FriendlyEffectType { get; }
    
    public bool EffectEnemy { get; }
    public EffectType EnemyEffectType { get; }
    
    public TargetShotEffectStat(bool effectEnemy=false, EffectType enemyEffectType=EffectType.None, bool effectFriendly=false, EffectType friendlyEffectType=EffectType.None)
    {
        EffectFriendly = effectFriendly;
        FriendlyEffectType = friendlyEffectType;
        
        EffectEnemy = effectEnemy;
        EnemyEffectType = enemyEffectType;
    }
    
    public void OnShooting(ShootingEventArgs ev)
    {
        
    }

    public void OnShot(ShotEventArgs ev)
    {
        if(ev.Target == null) return;
        float random = UnityEngine.Random.Range(0, 100);
        if(random > 10) return;
        if(ev.Target.Role.Side == ev.Player.Role.Side && EffectFriendly)
        {
            ev.Target.EnableEffect(FriendlyEffectType, 1, 3, true);
            ev.Player.ShowHUDHint($"<color=green>Gave effect {FriendlyEffectType} to {ev.Target.DisplayNickname}</color>" , 5f);
            ev.Target.ShowHUDHint($"<color=green>You were given effect {FriendlyEffectType} by {ev.Player.DisplayNickname}</color>", 5f);
        }
        else if(EffectEnemy)
        {
            ev.Target.EnableEffect(EnemyEffectType, 1, 3, true);
            ev.Player.ShowHUDHint($"<color=red>Gave effect {EnemyEffectType} to {ev.Target.DisplayNickname}</color>" , 5f);
            ev.Target.ShowHUDHint($"<color=red>You were given effect {EnemyEffectType} by {ev.Player.DisplayNickname}</color>", 5f);
        }
        
    }
}