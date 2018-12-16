using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using G4AW2.Data.DropSystem;
using CustomEvents;
using Utils;

namespace G4AW2.Data.Inventory
{
    [CreateAssetMenu(menuName = "Data/Inventory")]
    public class InventoryList : ScriptableObject {

	    [SerializeField] private RuntimeSetItem allItems;
	    [SerializeField] private RuntimeSetInt numberOfItems;
	    [SerializeField] private RuntimeSetInt indexOfItems;

		public List<ItemWithNumber> GetItems() {
		    List<ItemWithNumber> inventory = new List<ItemWithNumber>();
		    for (int i = 0; i < allItems.Count; i++) {
			    inventory.Add(new ItemWithNumber { amount = numberOfItems[i], item = allItems[i], index = i });
		    }
		    return inventory;
	    }

		public void AddItem(Item item) {
			int i = 0;
	        for (i = 0; i < allItems.Count; i++) {
		        if (allItems[i] != item) continue;
		        if (numberOfItems[i] < item.maxStackSize) {
			        numberOfItems[i]++;
		        }
	        }

			allItems.Add(item);
			numberOfItems.Add(1);
		}

	    public void AddAmounts(int indexOfTaker, int indexOfGiver, int amount) {

#if UNITY_EDITOR
			Debug.Assert(indexOfTaker != -1 || allItems[indexOfGiver] == allItems[indexOfTaker], "Trying to add items to incompatible spot");
#endif

			if (indexOfTaker == -1) {
			    allItems.Add(allItems[indexOfGiver]);
			    numberOfItems.Add(amount);
		    }
		    else {
			    numberOfItems[indexOfTaker] += amount;
			    numberOfItems[indexOfGiver] -= amount;
		    }
	    }

	    public class ItemWithNumber {
		    public Item item;
		    public int amount;
		    public int index;
	    }
    }
}
