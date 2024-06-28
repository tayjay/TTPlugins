using CustomPlayerEffects;
using Exiled.API.Extensions;
using Exiled.API.Features;
using PlayerRoles;
using UnityEngine;

namespace TTCore.Npcs.AI.Core.World.Targetables;

public class TargetablePlayer(Player p) : TargetableBase
    {
        public readonly Player Player = p;

        public override Vector3 GetPosition(AIModuleRunner module) => Player.Position;

        public override Vector3 GetHeadPosition(AIModuleRunner module) => Player.ReferenceHub.PlayerCameraReference.position;

        public override bool IsAlive => Player.IsAlive;

        public override bool CanTarget(AIModuleRunner module, out bool cannotAttack)
        {
            cannotAttack = false;
            return 
                Player != null
                && Player.ReferenceHub != module.ReferenceHub
                && IsAlive
                && !Player.IsCuffed
                && !Player.IsGodModeEnabled
                && !IsInvisible(Player)
                && IsEnemy(module)
                && (!AIModuleRunner.DisableKOS || module.ReferenceHub.IsSCP() || !IsCivilian(Player) || IsArmed(Player))
                && module.HasLOS(this, out _, out cannotAttack, allowWallHack: true);
        }

        public override bool CanFollow(AIModuleRunner module) => 
            Player != null
            && Player.ReferenceHub != module.ReferenceHub
            && Player.IsAlive
            && !Player.IsGodModeEnabled
            && !IsInvisible(Player)
            && (!IsEnemy(module) || (module.IsDisarmed(out Player disarmer) && disarmer == Player))
            && module.WithinDistance(this, module.FollowDistanceMax);

        public bool IsEnemy(AIModuleRunner p) => Player.Role.Side != p.Role.GetSide();

        public static bool IsCivilian(Player p) => p.Role == RoleTypeId.ClassD || p.Role == RoleTypeId.Scientist;

        public static bool IsArmed(Player p) => p.CurrentItem != null && (p.CurrentItem.Category == ItemCategory.Firearm || p.CurrentItem.Category == ItemCategory.Grenade);

        public static bool IsInvisible(Player p) => p.TryGetEffect(out Invisible inv) && inv.IsEnabled;

        public static implicit operator Player(TargetablePlayer t) => t.Player;
        public static implicit operator TargetablePlayer(Player p) => new(p);
    }