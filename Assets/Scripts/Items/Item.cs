using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using UnityEditor;
using UnityEngine;

namespace G4AW2.Data.DropSystem
{
    public enum Rarity
    {
        Common = 0,
        Uncommon = 1,
        Rare = 2,
        VeryRare = 3,
        Legendary = 4,
        Mythical = 5
    }

    [CreateAssetMenu(menuName = "Data/Items/Item")]
    public class Item : ScriptableObject, IID {
	    public int ID;
        public Sprite Image;
        public int Value;
        public string Description;
        public Rarity Rarity;

	    public int GetID() {
		    return ID;
	    }

        public virtual bool ShouldCreateNewInstanceWhenCrafted() {
            return false;
        }

        public virtual void OnAfterCrafted() {
        }

#if UNITY_EDITOR
	    [ContextMenu("Pick ID")]
	    public void PickID() {
		    ID = IDUtils.PickID<Item>();
	    }
#endif
        
    }


}

