using Exiled.API.Features.Pickups;
using UnityEngine;

namespace TTCore.Npcs.AI.Core.World.Targetables;

public class TargetableItem(Pickup item) : TargetableBase
{
    public readonly Pickup Item = item;

    public override Vector3 GetPosition(AIModuleRunner module) => Item.Position;

    public override Vector3 GetHeadPosition(AIModuleRunner module) => Item.Position;

    public override bool IsAlive => true;

    public override bool CanFollow(AIModuleRunner module) =>
        !module.IsDisarmed(out _)
        && module.WithinDistance(this, module.ItemDistance)
        && module.Player.IsHuman;

    public override bool CanTarget(AIModuleRunner module, out bool cannotAttack)
    {
        cannotAttack = true;
        return false;
    }

    public static implicit operator Pickup(TargetableItem t) => t.Item;
    public static implicit operator TargetableItem(Pickup t) => new(t);
}