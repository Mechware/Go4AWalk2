﻿using CustomEvents;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class QuickItem : MonoBehaviour {

    public InventoryItemDisplay ItemDisplay;

    public ItemVariable Item;
    public RuntimeSetItem Inventory;

    public GameObject ItemViewer;

	// Use this for initialization
	void Start () {
        SetData();
	}

    void OnHold(InventoryItemDisplay item) {
        ItemViewer.GetComponent<ItemViewer>().ShowItems(
            Inventory.Value.Where(it => it.type == ItemType.Consumable).Distinct(), 
            null, 
            ItemSelect);
    }

    void ItemSelect(Item it) {
        Item.Value = it;
        SetData();
        ItemViewer.SetActive(false);
    }

    public void SetData() {
        ItemDisplay.SetData(Item, Item.Value == null ? 0 : Inventory.Value.Count(it => it.ID == Item.Value.ID), OnClick, OnHold);
    }

    void OnClick(InventoryItemDisplay it) {
        if (it == null)
            return;

        Consumable c = (Consumable) it.Item;
        c.OnUse.Invoke();
    }
}