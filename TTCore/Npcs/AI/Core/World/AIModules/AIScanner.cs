﻿using System.Collections.Generic;
using System.Linq;
using Exiled.API.Features;
using TTCore.Npcs.AI.Core.World.Targetables;

namespace TTCore.Npcs.AI.Core.World.AIModules;

public class AIScanner : AIModuleBase
    {
        public float SearchRadiusEnemy = 70f;
        public float BreakBreakableRadius = 5f;
        public float SearchRadiusFollow = 20f;

        public bool CanBreakBreakables = true;

        public TargetableBase LookTarget => Parent.FollowTarget;

        public override void Init()
        {
            Tags = [AIBehaviorBase.DetectorTag];
        }

        public override void OnDisabled() { }

        public override void OnEnabled() { }

        public override void Tick()
        {
            if (Parent.RetargetTimer <= 0f)
            {
                Parent.RetargetTimer = AIModuleRunner.RetargetTime;

                List<Player> players = Player.List.ToList();

                TargetableBase target = null;
                TargetableBase follow = null;

                foreach (Player p in players)
                {
                    if (Parent.WithinDistance(p, SearchRadiusEnemy) && Parent.CanTarget(p, out _) && Parent.IsInView(p) && (target == null || Parent.GetDistance(target) > Parent.GetDistance(p)))
                        target = p;
                    else if (!Parent.HasFollowTarget && Parent.GetFollowWeight(p) > 0 && Parent.WithinDistance(p, SearchRadiusFollow) && Parent.CanFollow(p) && Parent.IsInView(p) && (follow == null || Parent.GetFollowWeight(follow) < Parent.GetFollowWeight(p) || Parent.GetDistance(follow) > Parent.GetDistance(p)))
                        follow = p;
                }

                /*if (CanBreakBreakables)
                    foreach (BreakableToyBase toy in BreakableToyManager.Breakables)
                        if (Parent.WithinDistance(toy, BreakBreakableRadius) && Parent.CanTarget(toy, out _) && (target == null || target is TargetablePlayer || Parent.GetDistance(target) > Parent.GetDistance(toy)))
                            target = toy;*/

                Parent.EnemyTarget = target;

                if (follow != null)
                    Parent.FollowTarget = follow;
            }
        }
    }