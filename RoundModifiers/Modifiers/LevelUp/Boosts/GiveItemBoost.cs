﻿using Exiled.API.Features;
using TayTaySCPSL.Modifiers.LevelUp.Interfaces;

namespace TayTaySCPSL.Modifiers.LevelUp.Boost
{
    public class GiveItemBoost : Boost
    {
        private ItemType[] _itemTypes;
        
        
        
        public GiveItemBoost(ItemType itemType, Tier tier = Tier.Common) : base(tier)
        {
            _itemTypes = new ItemType[] {itemType};
        }
        
        public GiveItemBoost(ItemType[] itemTypes, Tier tier = Tier.Common) : base(tier)
        {
            _itemTypes = itemTypes;
        }

        public override bool AssignBoost(Player player)
        {
            if(player.HasItem(_itemTypes[0])) return false;
            return ApplyBoost(player);
        }

        public override bool ApplyBoost(Player player)
        {
            //Get a random item from the array
            ItemType itemType = _itemTypes[UnityEngine.Random.Range(0, _itemTypes.Length)];
            //Give this item to the player
            player.AddItem(itemType);
            return true;
        }

        public override string GetName()
        {
            return "Give Item: "+_itemTypes[0].ToString();
        }

        public override string GetDescription()
        {
            return Description;
        }


        public string Name { get; set; } = "Give Item";
        public string Description { get; set; } = "Receive a random item when you level up.";
    }
}