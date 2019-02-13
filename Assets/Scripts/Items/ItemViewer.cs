using CustomEvents;
using G4AW2.Data.DropSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Material = G4AW2.Data.DropSystem.Material;

public class ItemViewer : MonoBehaviour {

    public GameObject ItemDisplayPrefab;

    public GameObject Content;

    private List<GameObject> items = new List<GameObject>();

    public RuntimeSetItem Inventory;

    public void ShowItemsFromInventory<T>(bool showAmounts, Action<Item> onClick) where T : Item {
        if(showAmounts)
            ShowItems(Inventory.Value.OfType<T>(), onClick);
        else
            ShowItems(Inventory.Value.OfType<T>().Distinct(), onClick);
    }

    public void ShowMaterialFromInventory(MaterialType type, bool showAmounts, Action<Item> onClick) {
        if(showAmounts) {
            ShowItems(Inventory.Value.OfType<Material>().Where((mat) => mat.Type == type), onClick);
        } else {
            ShowItems(Inventory.Value.OfType<Material>().Distinct().Where((mat) => mat.Type == type), onClick);
        }
    }

    public void ShowAllMaterialFromInventory(bool showAmounts, Action<Item> onClick) {
        if(showAmounts) {
            ShowItems(Inventory.Value, onClick);
        } else {
            ShowItems(Inventory.Value.Distinct(), onClick);
        }
    }

    public void Clear() {
        items.ForEach(it => Destroy(it));
    }

    public void ShowItems(IEnumerable<Item> itemsToAdd, Action<Item> onClick) {
        Clear();

        gameObject.SetActive(true);

        Dictionary<Item, int> ItemsToCount = new Dictionary<Item, int>();

        foreach (var it in itemsToAdd) {

            if(ItemsToCount.ContainsKey(it)) {
                ItemsToCount[it]++;
            } else {
                ItemsToCount.Add(it, 1);
            }
        }

        foreach(var kvp in ItemsToCount) {
            GameObject go = GameObject.Instantiate(ItemDisplayPrefab, Content.transform);
            InventoryItemDisplay iid = go.GetComponent<InventoryItemDisplay>();
            iid.SetData(kvp.Key, kvp.Value, (it2) => onClick?.Invoke(it2.Item));
            items.Add(iid.gameObject);
        }
    }
}
