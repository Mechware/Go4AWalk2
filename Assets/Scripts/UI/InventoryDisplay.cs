using G4AW2.Combat;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class InventoryDisplay : MonoBehaviour {

    public Transform ItemsParent;
    public GameObject ItemPrefab;

    public InventoryItemDisplay ItemDisplay;
    public TextMeshProUGUI ItemText;

    private ObjectPrefabPool _pool = null;
    private ObjectPrefabPool pool {
        get {
            if(_pool == null)
                _pool = new ObjectPrefabPool(ItemPrefab, ItemsParent);
            return _pool;
        }
    }

    private void OnEnable() {
        Refresh();
    }

    public void Refresh() {
        pool.Reset();

        foreach(var item in Inventory.Instance) {
            var go = pool.GetObject();
            var id = go.GetComponent<InventoryItemDisplay>();
            id.SetDataInstance(item, 1, ItemClicked);
            id.gameObject.transform.SetAsLastSibling();
        }
    }

    public void ItemClicked(InventoryItemDisplay it) {
        ItemDisplay.SetDataInstance(it.CurrentItem, 1, null);
        string text = "";
        text += $"Name: {it.CurrentItem.GetName()}\n";
        text += $"Type: {it.CurrentItem.GetType().Name}\n";
        text += $"Value: {it.CurrentItem.GetValue()}\n";
        text += $"'{it.CurrentItem.GetDescription()}'";
        ItemText.SetText(text);

        EquipItemProcessor.Instance.ProcessItem(it.CurrentItem, Refresh);
    }

}
