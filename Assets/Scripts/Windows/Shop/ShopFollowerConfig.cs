using G4AW2.Data.DropSystem;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Data
{
    [CreateAssetMenu(menuName = "Data/Follower/Shop Follower")]
    public class ShopFollowerConfig : FollowerConfig {
        public AnimationClip WalkingAnimation;

        public List<SellableItem> Items;
        public float BuyingPriceMultiplier = 1.5f;
        public float SellingPriceMultiplier = 0.5f;
    }
    
    [Serializable]
    public class SellableItem {
        public ItemConfig ItemConfig;
        public int Amount;
        public int Level;
    }
}


