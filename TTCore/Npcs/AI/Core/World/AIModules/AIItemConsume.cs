using InventorySystem;
using InventorySystem.Items.Usables;
using UnityEngine;
using Utils.Networking;
using Consumable = Exiled.API.Features.Items.Consumable;

namespace TTCore.Npcs.AI.Core.World.AIModules;

/*public class AIItemConsume : AIModuleBase
    {
        public float HealPercentage = 0.5f;

        public override float Duration => cachedDuration;

        public float Delay = 2f;

        public bool InfiniteConsumables = false;

        private float cachedDuration;
        private float useTime;
        private float delay;
        private bool inUse;

        public override bool Condition()
        {
            if (delay > 0f)
                return false;

            if (Parent.HasItem(out Consumable cons))
            {
                cachedDuration = cons.UseTime + 0.5f;
                return Parent.Health.CurValue <= Parent.Health.MaxValue * HealPercentage;
            }
            return false;
        }

        public override void Init()
        {
            Tags = [AIBehaviorBase.AutonomyTag];
        }

        public override void OnDisabled()
        {
            useTime = 0f;
            inUse = false;
        }

        public override void OnEnabled() { }

        public override void Tick()
        {
            if (delay > 0f)
                delay -= Time.fixedDeltaTime;

            if (!Enabled)
                return;

            if (useTime > 0f)
                useTime -= Time.fixedDeltaTime;
            else if (Parent.TryGetItem(out Consumable con))
                Consume(con);
            else
                Parent.EquipItem<Consumable>();
        }

        public void Consume(Consumable item)
        {
            if (inUse && useTime <= 0f)
            {
                inUse = false;
                item.Base.ServerOnUsingCompleted();

                if (InfiniteConsumables)
                    Parent.Inventory.ServerAddItem(item.Base.ItemTypeId);

                delay = Delay;
                return;
            }

            if (!item.Base.CanStartUsing)
                return;

            useTime = item.UseTime;
            inUse = true;
            new StatusMessage(StatusMessage.StatusType.Start, item.Base.ItemSerial).SendToAuthenticated();
        }
    }*/