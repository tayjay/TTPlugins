using Exiled.API.Enums;
using Exiled.API.Features;
using Exiled.API.Features.Items;

namespace RoundModifiers.Modifiers.LevelUp.Boosts
{
    public class UpgradeWeaponBoost : Boost
    {
        public UpgradeWeaponBoost(Tier tier) : base(tier)
        {
        }

        public override bool AssignBoost(Player player)
        {
            if(player.IsCuffed) return false;
            return ApplyBoost(player);
        }

        public override bool ApplyBoost(Player player)
        {
            //This will apply regardless of Tier
            foreach (Item item in player.Items)
            {
                if(!item.IsWeapon) continue;
                ItemType newWeapon = UpgradeWeapon(player.CurrentItem as Firearm);
                if(newWeapon == ItemType.None) continue;
                Firearm newFirearm = Item.Create(newWeapon) as Firearm;
                player.RemoveItem(item);
                player.AddItem(newFirearm);
                player.AddAmmo(GetAmmoType(newFirearm), newFirearm.MaxAmmo);
                return true;
            }

            //Don't want to give away a free gun with the Common upgrade
            if (Tier >= Tier.Uncommon)
            {
                player.AddItem(GenerateOutcome(ItemType.GunCOM15, ItemType.GunCOM18, ItemType.GunFSP9));
                return true;
            }
            else
            {
                return false;
            }

        }

        public ItemType UpgradeWeapon(Firearm firearm)
        {
            if(firearm == null) return ItemType.None;
            switch (firearm.Base.ItemTypeId)
            {
                //Foundation Guns
                case ItemType.GunCOM15:
                    return GenerateOutcome(ItemType.GunCOM18, ItemType.GunCom45, ItemType.GunRevolver);
                case ItemType.GunCOM18:
                    return ItemType.GunFSP9;
                case ItemType.GunFSP9:
                    return ItemType.GunCrossvec;
                case ItemType.GunCrossvec:
                    return ItemType.GunE11SR;
                case ItemType.GunE11SR:
                    return ItemType.GunFRMG0;
                //Chaos Guns
                case ItemType.GunRevolver:
                    return ItemType.GunShotgun;
                case ItemType.GunShotgun:
                    return ItemType.GunLogicer;
                default:
                    return ItemType.None;
            }
        }

        public AmmoType GetAmmoType(Firearm firearm)
        {
            return firearm.AmmoType;
        }
        
        
        
        public ItemType GenerateOutcome(params ItemType[] outcomes)
        {
            return outcomes[UnityEngine.Random.Range(0, outcomes.Length)];
        }

        public override string GetName()
        {
            throw new System.NotImplementedException();
        }

        public override string GetDescription()
        {
            throw new System.NotImplementedException();
        }
    }
}