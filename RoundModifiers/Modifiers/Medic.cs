using Exiled.API.Features;
using Exiled.Events.EventArgs.Player;
using PlayerRoles;
using RoundModifiers.API;
using RoundModifiers.Modifiers.LevelUp.XPs;
using TTCore.Extensions;
using TTCore.HUDs;
using UnityEngine;

namespace RoundModifiers.Modifiers
{
    /*
     * Allows players to heal other players
     * When using a medkit and looking at a player, the player will be healed
     * If the player is looking at a dead body, the player will be revived if that was their most recent corpse and they are currently dead.
     */
    public class Medic : Modifier
    {
        public MedicXP CustomXP = new MedicXP();
        
        public void OnUsingItem(UsingItemEventArgs ev)
        {
            if (ev.Item.Type == ItemType.Medkit)
            {
                //We're using a medkit, let's check if we're looking at a player
                if (ev.Player.TryGetPlayerOnSight(5f, out Player target))
                {
                    if(target.Health >= target.MaxHealth)
                    {
                        ev.IsAllowed = false;
                        Log.Info("Player is at full health");
                        ev.Player.ShowHUDHint(target.Nickname + " is already at full health!", 3f);
                        return;
                    }
                    ev.IsAllowed = true;
                    ev.Player.ShowHUDHint("Keep looking at " + target.Nickname +" to heal them.", 5f);
                } else if(ev.Player.TryGetRagdollOnSight(5f, out Ragdoll ragdoll))
                {
                    if (ragdoll.Owner.IsDead)
                    {
                        ev.IsAllowed = true;
                        Log.Info("Player is dead");
                        ev.Player.ShowHUDHint("Keep looking at " + ragdoll.Owner.Nickname +" to revive them.", 5f);
                    }
                    else
                    {
                        ev.IsAllowed = false;
                        Log.Info("Player cannot be revived");
                        ev.Player.ShowHUDHint("You can't revive " + ragdoll.Owner.Nickname + "!\nLook away to heal yourself.", 3f);
                    }
                }
            }
        }

        public void OnUsingItemCompleted(UsingItemCompletedEventArgs ev)
        {
            if (ev.Item.Type == ItemType.Medkit)
            {
                //We're using a medkit, let's check if we're looking at a player
                if (ev.Player.TryGetPlayerOnSight(5f, out Player target))
                {
                    ev.IsAllowed = false;
                    ev.Player.ShowHUDHint("You have healed " + target.Nickname +"!", 3f);
                    target.Heal(75f);
                    ev.Player.RemoveItem(ev.Item);
                    if (RoundModifiers.Instance.IsModifierActive("LevelUp"))
                    {
                        //RoundModifiers.Instance.GetModifier<LevelUp.LevelUp>().PlayerXP[ev.Player.NetId] += 100;
                        CustomXP.GiveHealOtherXP(ev.Player);
                    }
                } else if(ev.Player.TryGetRagdollOnSight(5f, out Ragdoll ragdoll))
                {
                    if (ragdoll.Owner.IsDead)
                    {
                        ev.IsAllowed = false;
                        ev.Player.ShowHUDHint("You have revived " + ragdoll.Owner.Nickname +"!", 5f);
                        ragdoll.Owner.RoleManager.ServerSetRole(ragdoll.Role, RoleChangeReason.Revived, RoleSpawnFlags.None);
                        ragdoll.Owner.Teleport(ragdoll.Position+Vector3.up);
                        ev.Player.RemoveItem(ev.Item);
                        if (RoundModifiers.Instance.IsModifierActive("LevelUp"))
                        {
                            //RoundModifiers.Instance.GetModifier<LevelUp.LevelUp>().PlayerXP[ev.Player.NetId] += 100;
                            CustomXP.GiveRezXP(ev.Player);
                        }
                    }
                    else
                    {
                        ev.IsAllowed = true;
                        ev.Player.ShowHUDHint("You can't revive " + ragdoll.Owner.Nickname + "!\nLook away to heal yourself.", 3f);
                    }
                        
                }
            }
        }
        
        
        protected override void RegisterModifier()
        {
            Exiled.Events.Handlers.Player.UsingItem += OnUsingItem;
            Exiled.Events.Handlers.Player.UsingItemCompleted += OnUsingItemCompleted;
        }

        protected override void UnregisterModifier()
        {
            Exiled.Events.Handlers.Player.UsingItem -= OnUsingItem;
            Exiled.Events.Handlers.Player.UsingItemCompleted -= OnUsingItemCompleted;
        }

        public override ModInfo ModInfo { get; } = new ModInfo() 
        {
            Name = "Medic",
            FormattedName = "<color=blue>Medic</color>",
            Aliases = new[] {"healer"},
            Description = "Allows players to heal other players",
            Impact = ImpactLevel.MajorGameplay,
            MustPreload = false
        };

        
        public class MedicXP : XP
        {
            public override string Name { get; set; } = "Medic";
            public override string Description { get; set; } = "Healing players";
            
            public void GiveHealOtherXP(Player player)
            {
                GiveXP(player, 100);
            }
            
            public void GiveRezXP(Player player)
            {
                GiveXP(player, 100);
            }
        }
    }
}