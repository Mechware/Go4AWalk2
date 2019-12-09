using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class Inventory : MonoBehaviour, IEnumerable<ItemInstance> {
    private List<ItemInstance> items = new List<ItemInstance>();
    
    public static Inventory Instance;

    private void Awake() {
        Instance = this;
    }

    public void Add(ItemInstance item) {
        SaveGame.Instance.Items.Add(item.SaveData);
        items.Add(item);
    }

    public void Remove(ItemInstance item) {
        SaveGame.Instance.Items.Remove(item.SaveData);
        items.Remove(item);
    }

    public bool Remove(int id) {
        var toRemove = items.FirstOrDefault(it => it.Config.Id == id);
        if (toRemove == null) return false;
        return items.Remove(toRemove);
    }
    
    public bool Contains(ItemConfig c) {
        return items.Any(i => i.Config == c);
    }
    
    public int GetAmountOf(ItemConfig it) {
        return items.Count(i => i.Config == it);
    }

    public IEnumerator<ItemInstance> GetEnumerator() {
        return items.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return GetEnumerator();
    }
}
