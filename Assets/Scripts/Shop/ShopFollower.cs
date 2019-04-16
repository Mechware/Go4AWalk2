using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Data {
    [CreateAssetMenu(menuName = "Data/Follower/Shop Follower")]
    public class ShopFollower : FollowerData, ISaveable {
        public AnimationClip WalkingAnimation;

        public PersistentSetItem AllItems;
        public List<InventoryEntry> Items;
        public float BuyingPriceMultiplier = 1.5f;
        public float SellingPriceMultiplier = 0.5f;

        private class SaveObject {
            public int ID;
            public List<InventoryEntry.InventoryEntryWithID> Entries;
        }

        public override string GetSaveString() {

            return JsonUtility.ToJson(new SaveObject() {
                ID = ID,
                Entries = Items.Select(it => it.GetIdEntry()).ToList()
            });
        }

        public override void SetData(string saveString, params object[] otherData) {
            SaveObject ds = JsonUtility.FromJson<SaveObject>(saveString);

            ID = ds.ID;

            if (AllItems != null && ds.Entries != null) {
                Items.Clear();

                foreach(InventoryEntry.InventoryEntryWithID entry in ds.Entries) {

                    Item it = AllItems.First(d => d.ID == entry.Id);

                    if(it is ISaveable) {
                        it = (Item) CreateInstance(it.GetType());
                        ((ISaveable) it).SetData(entry.AdditionalInfo, otherData);
                    }

                    InventoryEntry ie = new InventoryEntry() {
                        Item = it,
                        Amount = entry.Amount,
                    };

                    Items.Add(ie);
                }
            }

            FollowerData original;

            if(otherData[0] is PersistentSetFollowerData) {
                PersistentSetFollowerData allFollowers = (PersistentSetFollowerData) otherData[0];
                original = allFollowers.First(it => it.ID == ID);
            } else {
                original = otherData[0] as FollowerData;
                if (SizeOfSprite == original.SizeOfSprite && SideIdleAnimation == original.SideIdleAnimation) {


                    return; // This object may have been create based on the original. In which case, we don't need to do any copying
                }
                    
            }

            ID = original.ID;
            SizeOfSprite = original.SizeOfSprite;
            SideIdleAnimation = original.SideIdleAnimation;
            RandomAnimation = original.RandomAnimation;
            MinTimeBetweenRandomAnims = original.MinTimeBetweenRandomAnims;
            MaxTimeBetweenRandomAnims = original.MaxTimeBetweenRandomAnims;
            AllItems = ((ShopFollower) original).AllItems;
            if (ds.Entries == null) {
                Items = ((ShopFollower) original).Items;
            }
        }
    }
}


