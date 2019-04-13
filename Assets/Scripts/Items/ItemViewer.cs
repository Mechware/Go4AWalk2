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

    public Inventory Inventory;

    public void ShowItemsFromInventory<T>(bool showAmounts, bool showTrash, Action<T> onClick, bool showNull = false) where T : Item, ITrashable {
        ShowItems(Inventory.Where(e => e.Item is T && (showTrash || !((T) e.Item).IsTrash())), i => onClick((T) i), showAmounts, showNull);
    }

    public void ShowItemsFromInventory<T>(bool showAmounts, Action<T> onClick, bool showNull = false) where T : Item {
        ShowItems(Inventory.Where(e => e.Item is T), i => onClick((T) i), showAmounts, showNull);
    }

    public void ShowMaterialFromInventory(MaterialType type, bool showAmounts, Action<Item> onClick, bool showNull = false) {
        ShowItems(Inventory.Where((e) => e.Item is Material && ((Material)e.Item).Type == type), onClick, showAmounts, showNull);
    }

    public void ShowAllMaterialFromInventory(bool showAmounts, Action<Item> onClick, bool showNull = false) {
        ShowItems(Inventory, onClick, showAmounts, showNull);
    }

    public void Clear() {
        items.ForEach(it => Destroy(it));
    }

    public void ShowItems(IEnumerable<InventoryEntry> itemsToAdd, Action<Item> onClick, bool showAmount, bool showNull = false) {
        Clear();

        gameObject.SetActive(true);


        foreach(var entry in itemsToAdd) {
            GameObject go = GameObject.Instantiate(ItemDisplayPrefab, Content.transform);
            InventoryItemDisplay iid = go.GetComponent<InventoryItemDisplay>();
            iid.SetData(entry.Item, showAmount ? entry.Amount : 0, (it2) => onClick?.Invoke(it2.Item));
            items.Add(iid.gameObject);
        }

        if (showNull) {
            GameObject go = GameObject.Instantiate(ItemDisplayPrefab, Content.transform);
            InventoryItemDisplay iid = go.GetComponent<InventoryItemDisplay>();
            iid.SetData(null, 0, (it2) => onClick?.Invoke(it2.Item));
            items.Add(iid.gameObject);
        }
    }

    public void Close() {
        gameObject.SetActive(false);
    }
}
