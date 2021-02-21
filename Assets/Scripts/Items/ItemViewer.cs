using CustomEvents;
using G4AW2.Data.DropSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using Material = G4AW2.Data.DropSystem.Material;

public class ItemViewer : MonoBehaviour {

    [Obsolete("Singleton")] public static ItemViewer Instance;
    public GameObject ItemDisplayPrefab;

    public GameObject Content;

    private List<GameObject> items = new List<GameObject>();

    public Inventory Inventory;

    public TextMeshProUGUI Title;
    
    public void Init() {
        Instance = this;
    }
    
    public void ShowItemsFromInventory<T>(string title, bool showAmounts, bool showTrash, Action<T> onClick, bool showNull = false) where T : Item, ITrashable {
        ShowItems(title, Inventory.Where(e => e.Item is T && (showTrash || !((T) e.Item).IsTrash())), i => onClick((T) i), showAmounts, showNull);
    }

    public void ShowItemsFromInventoryWhere<T>(string title, Func<InventoryEntry, bool> selection, bool showAmounts, Action<T> onClick, bool showNull = false) where T : Item {
        ShowItems(title, Inventory.Where(e => e.Item is T).Where(selection), i => onClick((T) i), showAmounts, showNull);
    }

    public void ShowItemsFromInventory<T>(string title, bool showAmounts, Action<T> onClick, bool showNull = false) where T : Item {
        ShowItems(title, Inventory.Where(e => e.Item is T), i => onClick((T) i), showAmounts, showNull);
    }

    public void ShowMaterialFromInventory(string title, MaterialType type, bool showAmounts, Action<Item> onClick, bool showNull = false) {
        ShowItems(title, Inventory.Where((e) => e.Item is Material && ((Material)e.Item).Type == type), onClick, showAmounts, showNull);
    }

    public void ShowAllMaterialFromInventory(string title, bool showAmounts, Action<Item> onClick, bool showNull = false) {
        ShowItems(title, Inventory, onClick, showAmounts, showNull);
    }

    public void Clear() {
        items.ForEach(it => Destroy(it)); // TODO: Pool Items
    }

    public void ShowItems(string title, IEnumerable<InventoryEntry> itemsToAdd, Action<Item> onClick, bool showAmount, bool showNull = false) {
        Open();

        Title.text = title;
        
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

    public RobustLerperSerialized OpenLerper;
    public enum State { LerpingOpen, LerpingClosed, Open, Closed }
    public State state = State.Closed;

    public void ShowItems(IEnumerable<Item> itemsToAdd, Action<Item> onClick) {
        Open();
       
        foreach(var item in itemsToAdd) {
            GameObject go = GameObject.Instantiate(ItemDisplayPrefab, Content.transform);
            InventoryItemDisplay iid = go.GetComponent<InventoryItemDisplay>();
            iid.SetData(item, 0, (it2) => onClick?.Invoke(it2.Item));
            items.Add(iid.gameObject);
        }
    }

    public void Add<T>(T it, int amount, Action<T> onClick) where T : Item {
        GameObject go = GameObject.Instantiate(ItemDisplayPrefab, Content.transform);
        InventoryItemDisplay iid = go.GetComponent<InventoryItemDisplay>();
        iid.SetData(it, amount, (it2) => onClick?.Invoke((T)it2.Item));
        items.Add(iid.gameObject);
    }

    void Update() {
        OpenLerper.Update(Time.deltaTime);
    }

    public void Open() {
        Clear();
        transform.SetAsLastSibling();
        gameObject.SetActive(true);

        if(state != State.Closed) {
            OpenLerper.EndLerping(true);
        } else {
            state = State.LerpingOpen;
            OpenLerper.StartLerping(() => {
                state = State.Open;
            });
        }
    }

    public void Close() {
        state = State.LerpingClosed;
        OpenLerper.StartReverseLerp(() => {
            state = State.Closed;
            gameObject.SetActive(false);
        });
    }
}
