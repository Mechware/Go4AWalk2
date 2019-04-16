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

        [NonSerialized] public bool CreatedFromOriginal = false;

        public string Name { get { return name.Replace(" (clone)", ""); } }

	    public int ID;
        public Sprite Image;
        public int Value;
        public string Description;
        public Rarity Rarity;

        public Action DataChanged;

	    public int GetID() {
		    return ID;
	    }

        public virtual bool ShouldCreateNewInstanceWhenPlayerObtained() {
            return false;
        }

        public virtual void OnAfterObtained() {
        }

        public virtual void CopyValues(Item original) {
            ID = original.ID;
            Image = original.Image;
            Value = original.Value;
            Description = original.Description;
            Rarity = original.Rarity;
        }

        public virtual string GetName() {
            return name;
        }

        public virtual string GetDescription() {
            return Description;
        }

#if UNITY_EDITOR
	    [ContextMenu("Pick ID")]
	    public void PickID() {
		    ID = IDUtils.PickID<Item>();
	    }
#endif
        
    }


}

