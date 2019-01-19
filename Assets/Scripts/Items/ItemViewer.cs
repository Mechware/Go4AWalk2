using G4AW2.Data.DropSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemViewer : MonoBehaviour {

    public GameObject ItemDisplayPrefab;

    public GameObject Content;

    private List<GameObject> items = new List<GameObject>();

    public void Clear() {
        items.ForEach(it => Destroy(it));
    }

    public void ShowItems(IEnumerable<Item> itemsToAdd, IEnumerable<int> amounts = null, Action<Item> OnClick = null) {
        Clear();

        gameObject.SetActive(true);


        IEnumerator<int> amountsE = amounts == null ? null : amounts.GetEnumerator();

        foreach (var it in itemsToAdd) {
            GameObject go = GameObject.Instantiate(ItemDisplayPrefab, Content.transform);
            InventoryItemDisplay iid = go.GetComponent<InventoryItemDisplay>();
            iid.SetData(it, amountsE == null ? 1 : amountsE.Current, (it2) => OnClick?.Invoke(it2.Item));
            items.Add(iid.gameObject);

            if(amountsE != null) amountsE.MoveNext();
        }
    }
}
