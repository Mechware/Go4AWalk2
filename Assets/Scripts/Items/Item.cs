using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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

    [CreateAssetMenu(menuName = "Data/Items/Item")]
    public class Item : ScriptableObject, IID {
	    public int ID;
        public Sprite Image;
        public int Value;
        public string Description;
        public Rarity Rarity;
	    public int MaxStackSize;

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

