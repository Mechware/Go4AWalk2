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

		IEnumerable<InventoryList.ItemWithNumber> toDisplay;
		if (itemType == ItemFilter.Equipment) {
			toDisplay = list.inventory
				.Where(it => it.item.type == ItemType.Boots || it.item.type == ItemType.Accessory ||
			                                            it.item.type == ItemType.Hat || it.item.type == ItemType.Torso ||
			                                            it.item.type == ItemType.Weapon);
		} else if (itemType == ItemFilter.Consumable) {
			toDisplay = list.inventory.Where(it => it.item.type == ItemType.Consumable);
		} else if (itemType == ItemFilter.Material) {
			toDisplay = list.inventory.Where(it => it.item.type == ItemType.Material);
		}
		else {
			toDisplay = list.inventory;
		}

		foreach (InventoryList.ItemWithNumber it in toDisplay) {
			GameObject go = GameObject.Instantiate(ItemPrefab, ItemParent);
			InventoryItemDisplay iid = go.GetComponent<InventoryItemDisplay>();
			iid.SetData(it.item, it.amount);
			AllItems.Add(iid);
		}
	}

	public void UpdateInventory() {
		ShowInventory(currentItemFilter);
	}
}
