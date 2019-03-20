using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.DropSystem;
using G4AW2.Dialogue;
using UnityEngine;
using UnityEngine.Events;

public class ChangeItemBase<T, TRef, TVar, TEvent> : MonoBehaviour 
    where T : Item, ITrashable
    where TEvent : UnityEvent<T>, new()
    where TVar : Variable<T, TEvent>
    where TRef : Reference<T, TVar, TEvent> {

    public InventoryItemDisplay IID;

    public Inventory Inventory;

    public TRef Item;

    public ItemViewer Viewer;
    public BoolReference ShowTrash;


    // Use this for initialization
    public void Awake() {
        Item.Variable.OnChange.AddListener((it) => IID.SetData(it, 1, Onclick, Onhold));
        IID.SetData(Item, 1, Onclick, Onhold);
    }

    private void Onhold(InventoryItemDisplay inventoryItemDisplay) {
        //throw new NotImplementedException();
    }

    protected virtual void Onclick(InventoryItemDisplay inventoryItemDisplay) {

        Viewer.ShowItemsFromInventory<T>(false, ShowTrash, it => {
            PopUp.SetPopUp($"{it.GetName()}\n{it.GetDescription()}", new string[] {"Equip", it.IsTrash() ? "Untrash" : "Trash", "Cancel"}, new Action[] {
                () => {
                    Inventory.Add(Item.Value);
                    Item.Value = (T) it;
                    Inventory.Remove(it);
                    Viewer.Close();
                },
                () => {
                    it.SetTrash(!it.IsTrash());
                },
                () => { }
            });
        });
    }
}
