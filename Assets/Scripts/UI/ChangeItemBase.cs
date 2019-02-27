using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.DropSystem;
using UnityEngine;
using UnityEngine.Events;

public class ChangeItemBase<T, TRef, TVar, TEvent> : MonoBehaviour 
    where T : Item
    where TEvent : UnityEvent<T>, new()
    where TVar : Variable<T, TEvent>
    where TRef : Reference<T, TVar, TEvent> {

    public InventoryItemDisplay IID;

    public Inventory Inventory;

    public TRef Item;

    public ItemViewer Viewer;

    // Use this for initialization
    public void Awake() {
        Item.Variable.OnChange.AddListener((it) => IID.SetData(it, 1, Onclick, Onhold));
        IID.SetData(Item, 1, Onclick, Onhold);
    }

    private void Onhold(InventoryItemDisplay inventoryItemDisplay) {
        //throw new NotImplementedException();
    }

    private void Onclick(InventoryItemDisplay inventoryItemDisplay) {
        Viewer.ShowItemsFromInventory<T>(false, it => {
            Inventory.Add(Item.Value);
            Item.Value = (T) it;
            Inventory.Remove(it);
            Viewer.Close();
        });
    }
}
