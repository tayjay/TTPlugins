
using Exiled.API.Features;
using Exiled.API.Features.Items;

namespace RoundModifiers.Modifiers.LevelUp.Boosts
{
    public class UpgradeKeycardBoost : Boost
    {
        
        public bool CanBeUpgraded(Keycard keycard)
        {
            switch (keycard.Base.ItemTypeId)
            {
                case ItemType.KeycardMTFCaptain:
                case ItemType.KeycardFacilityManager:
                case ItemType.KeycardChaosInsurgency:
                case ItemType.KeycardO5:
                    return false;
                default:
                    return true;
            }
        }
        
        public ItemType UpgradeKeycard(Keycard keycard)
        {
            switch (keycard.Base.ItemTypeId)
            {
                case ItemType.KeycardJanitor:
                    return GenerateOutcome(ItemType.KeycardZoneManager, ItemType.KeycardScientist);
                case ItemType.KeycardScientist:
                    return ItemType.KeycardResearchCoordinator;
                case ItemType.KeycardResearchCoordinator:
                    return ItemType.KeycardFacilityManager;
                case ItemType.KeycardZoneManager:
                    return GenerateOutcome(ItemType.KeycardFacilityManager, ItemType.KeycardMTFOperative);
                case ItemType.KeycardGuard:
                case ItemType.KeycardMTFPrivate:
                    return ItemType.KeycardMTFOperative;
                case ItemType.KeycardMTFOperative:
                    return ItemType.KeycardMTFCaptain;
                case ItemType.KeycardContainmentEngineer:
                    return GenerateOutcome(ItemType.KeycardScientist, ItemType.KeycardFacilityManager);
                default:
                    return ItemType.None;
            }
        }
        
        public ItemType GenerateOutcome(params ItemType[] outcomes)
        {
            return outcomes[UnityEngine.Random.Range(0, outcomes.Length)];
        }

        public override bool AssignBoost(Player player)
        {
            return ApplyBoost(player);
        }

        public override bool ApplyBoost(Player player)
        {
            //Get the player's current keycard
            foreach (Item item in player.Items)
            {
                if (item.IsKeycard)
                {
                    Keycard keycard = ((Keycard)item);
                    if (!CanBeUpgraded(keycard)) return false; //If they already have the highest keycard, don't upgrade and give something else
                    //Get the next keycard in the list
                    ItemType newItem = UpgradeKeycard(keycard);
                    if (newItem == ItemType.None) continue;
                    //Set the player's keycard to the next keycard
                    player.RemoveItem(item);
                    player.AddItem(newItem);
                    return true;
                }
            }
            //If the player doesn't have a keycard, give them a keycard
            player.AddItem(GenerateOutcome(ItemType.KeycardZoneManager, ItemType.KeycardScientist));
            return true;
        }

        public override string GetName()
        {
            return "Upgrade Keycard";
        }

        public override string GetDescription()
        {
            return "Upgrade your keycard!";
        }

        public UpgradeKeycardBoost(Tier tier = Tier.Common) : base(tier)
        {
        }
    }
}