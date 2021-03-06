using G4AW2.Managers;
using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;

public class ItemViewer : MonoBehaviour {

    [Obsolete("Singleton")] public static ItemViewer Instance;
    public GameObject ItemDisplayPrefab;

    public GameObject Content;

    private List<GameObject> items = new List<GameObject>();

    public TextMeshProUGUI Title;

    [SerializeField] private ItemManager _items;
    [SerializeField] private ClickReceiver _bg;

    void Awake() {
        Instance = this;
        _bg.MouseClick.AddListener(Close);
    }

    public void Clear() {
        items.ForEach(it => Destroy(it)); // TODO: Pool Items
    }

    public void ShowItemsFromInventory<T>(string title, Action<ItemInstance> onClick, bool showAmount,
        bool showNull = false) where T : ItemInstance {
        ShowItems(title, _items.OfType<T>(), onClick, showAmount, showNull);
    }
    
    public void ShowItems(string title, IEnumerable<ItemInstance> itemsToAdd, Action<ItemInstance> onClick, bool showAmount, bool showNull = false) {
        Open();

        Title.text = title;
        
        foreach(var entry in itemsToAdd) {
            GameObject go = GameObject.Instantiate(ItemDisplayPrefab, Content.transform);
            InventoryItemDisplay iid = go.GetComponent<InventoryItemDisplay>();
            iid.SetDataInstance(entry, showAmount ? 1 : 0, (it2) => onClick?.Invoke(it2.CurrentItem));
            items.Add(iid.gameObject);
        }

        if (showNull) {
            GameObject go = GameObject.Instantiate(ItemDisplayPrefab, Content.transform);
            InventoryItemDisplay iid = go.GetComponent<InventoryItemDisplay>();
            iid.SetDataInstance(null, 0, (it2) => onClick?.Invoke(it2.CurrentItem));
            items.Add(iid.gameObject);
        }
    }

    public RobustLerperSerialized OpenLerper;
    public enum State { LerpingOpen, LerpingClosed, Open, Closed }
    public State state = State.Closed;

    public void ShowItems(IEnumerable<ItemInstance> itemsToAdd, Action<ItemInstance> onClick) {
        Open();
       
        foreach(var item in itemsToAdd) {
            GameObject go = GameObject.Instantiate(ItemDisplayPrefab, Content.transform);
            InventoryItemDisplay iid = go.GetComponent<InventoryItemDisplay>();
            iid.SetDataInstance(item, 0, (it2) => onClick?.Invoke(it2.CurrentItem));
            items.Add(iid.gameObject);
        }
    }

    public void Add(ItemInstance it, int amount, Action<ItemInstance> onClick) {
        GameObject go = GameObject.Instantiate(ItemDisplayPrefab, Content.transform);
        InventoryItemDisplay iid = go.GetComponent<InventoryItemDisplay>();
        iid.SetDataInstance(it, amount, (it2) => onClick?.Invoke(it2.CurrentItem));
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
