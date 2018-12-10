using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using G4AW2.Data.DropSystem;
using CustomEvents;

namespace G4AW2.Data.Inventory
{
    [CreateAssetMenu(menuName = "Data/Inventory")]
    public class InventoryList : ScriptableObject
    {
	    public RuntimeSetItem allItems;

		public void addItem(Item item)
        {
			allItems.Add(item);
        }

        public void insertItem(Item item, int index)
        {
			allItems.Insert(index, item);
        }

        public void removeItem(Item item)
        {
			allItems.Remove(item);
        }

        public void removeItem(Item item, int index)
        {
			allItems.RemoveAt(index);
		}


    }

   
}
