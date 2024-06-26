﻿using System;
using Exiled.API.Features.Items;
using PlayerRoles;
using TTCore.Npcs.AI.Core.World.Targetables;
using UnityEngine;

namespace TTCore.Npcs.AI.Core.World.AIModules;

public class AIBehaviorBase : AIModuleBase
    {
        /// <summary>
        /// Modules that makes the NPC attack enemies.
        /// </summary>
        public const string AttackerTag = "Attacker";
        /// <summary>
        /// Modules that makes the NPC chase after the enemy.
        /// </summary>
        public const string AggroTag = "Aggro";
        /// <summary>
        /// Modules that move the NPC to their follow target or location.
        /// </summary>
        public const string MoverTag = "Mover";
        /// <summary>
        /// Modules that allow the NPC to act on their own.
        /// </summary>
        public const string AutonomyTag = "Autonomy";
        /// <summary>
        /// Modules that scans the area for enemies and/or friendlies.
        /// </summary>
        public const string DetectorTag = "Detector";

        public Func<bool> TimerStopCondition;

        protected float Timer;

        public override void Init()
        {
            Parent.OnLostEnemy += OnLostEnemy;
        }

        public override void OnDisabled() { }

        public override void OnEnabled() { }

        public void OnLostEnemy(TargetableBase enemy, Vector3 lastLocation)
        {
            Parent.ChangeModule(AttackerTag, false);
            Parent.ChangeModule(AggroTag, true);
            Parent.ChangeModule(MoverTag, true);
            Parent.ChangeModule(AutonomyTag, false);
            TimerStopCondition = () => Parent.HasEnemyTarget;
            Timer = Mathf.Lerp(3f, 10f, Mathf.InverseLerp(0f, 70f, Vector3.Distance(Parent.Position, lastLocation)));
        }

        public override void Tick()
        {
            if (Parent.CurrentItem == null && (Parent.Role.GetTeam() == Team.FoundationForces || Parent.Role.GetTeam() == Team.ChaosInsurgency))
                Parent.EquipItem<Firearm>();

            if (Timer > 0f)
            {
                Timer -= Time.fixedDeltaTime;

                if (TimerStopCondition != null && TimerStopCondition.Invoke())
                {
                    TimerStopCondition = null;
                    Timer = 0f;
                }
                else
                    return;
            }

            if (Parent.HasEnemyTarget && Parent.ActivateRandomModuleByTag(AttackerTag, out AIModuleBase mod, true))
            {
                Timer = mod.Duration;
                Parent.ChangeModule(MoverTag, true);
                Parent.ChangeModule(AutonomyTag, false);
                Parent.ChangeModule(AggroTag, false);
            }
            else if (!Parent.HasEnemyTarget && Parent.HasFollowTarget)
            {
                Parent.ChangeModule(AttackerTag, false);
                Parent.ChangeModule(AggroTag, false);
                Parent.ChangeModule(MoverTag, true);
                Parent.ChangeModule(AutonomyTag, true);
            }
            else
            {
                Parent.ChangeModule(AttackerTag, true);
                Parent.ChangeModule(AggroTag, false);
                Parent.ChangeModule(MoverTag, true);
                Parent.ChangeModule(AutonomyTag, true);
            }
        }
    }