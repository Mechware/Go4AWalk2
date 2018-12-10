using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.DropSystem;
using G4AW2.Data.Inventory;
using Sirenix.OdinInspector;
using UnityEngine;

public class InventoryUI : MonoBehaviour {

	public InventoryList list;

	public Transform ItemParent;
	public GameObject ItemPrefab;

	private List<InventoryItemDisplay> AllItems = new List<InventoryItemDisplay>();

	[ShowInInspector] [ReadOnly] private ItemFilter currentItemFilter;

	public void Awake() {
		ShowInventory(ItemFilter.Equipment);
	}

	// I hate this so much. Why can't I pass in a enum param from the inspector?!
	public void ShowEquipmentFilter() { ShowInventory(ItemFilter.Equipment);}
	public void ShowMaterialFilter() { ShowInventory(ItemFilter.Material);}
	public void ShowConsumableFilter() { ShowInventory(ItemFilter.Consumable); }

	public void ShowInventory( ItemFilter itemType ) {
		currentItemFilter = itemType;

		// TODO: Pool these
		AllItems.ForEach(it => Destroy(it.gameObject));
		AllItems.Clear();

		IEnumerable<Item> toDisplay;
		if (itemType == ItemFilter.Equipment) {
			toDisplay = list.allItems.Value.Where(it => it.type == ItemType.Boots || it.type == ItemType.Accessory ||
			                                            it.type == ItemType.Hat || it.type == ItemType.Torso ||
			                                            it.type == ItemType.Weapon);
		} else if (itemType == ItemFilter.Consumable) {
			toDisplay = list.allItems.Value.Where(it => it.type == ItemType.Consumable);
		} else if (itemType == ItemFilter.Material) {
			toDisplay = list.allItems.Value.Where(it => it.type == ItemType.Material);
		}
		else {
			toDisplay = list.allItems.Value;
		}

		foreach (Item it in toDisplay) {
			GameObject go = GameObject.Instantiate(ItemPrefab, ItemParent);
			InventoryItemDisplay iid = go.GetComponent<InventoryItemDisplay>();
			iid.SetData(it, 1);
			AllItems.Add(iid);
		}
	}

	public void UpdateInventory() {
		ShowInventory(currentItemFilter);
	}
}
