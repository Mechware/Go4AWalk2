using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G4AW2.Data.Inventory;

namespace G4AW2.Data.DropSystem
{
    public enum Rarity
    {
        Shit = 0,
        PolishedShit = 1,
        Rarish = 2,
        Rare = 3,
        Legendary = 4,
        AMAZING = 5
    }

    public enum ItemType
    {
        Material = 0,
        Consumable = 1,
        Weapon = 2,
        Hat = 3,
        Torso = 4,
        Boots = 5,
        Accessory = 6
    }

    [CreateAssetMenu(menuName = "Data/DropSystem/Item")]
    public class Item : ScriptableObject, IID {
	    public int ID;
        public AnimationClip Walking;
        public Sprite image;
        public int value;
        public string description;
        public Rarity rarity;
        public ItemType type;
	    public int maxStackSize;

	    public int GetID() {
		    return ID;
	    }

#if UNITY_EDITOR
	    [ContextMenu("Pick ID")]
	    public void PickID() {
		    ID = IDUtils.PickID<Item>();
	    }

	    void OnEnable() {
		    if (ID == 0)
			    PickID();
	    }
#endif
	}


}

