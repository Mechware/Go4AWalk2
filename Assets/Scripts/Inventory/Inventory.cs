using CustomEvents;
using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Inventory")]
public class Inventory : SaveableScriptableObject, IEnumerable<InventoryEntry> {

    public List<InventoryEntry> InventoryEntries;
    public PersistentSetItem AllItems;

    public void Add(Item it) {
        Add(it, 1);
    }

    public void Add(Item it, int amount) {
        InventoryEntry entry = InventoryEntries.FirstOrDefault(e => e.Item == it && it.GetAdditionalInfo() == e.AdditionInfo);
        if(entry == default(InventoryEntry)) {
            InventoryEntries.Add(new InventoryEntry() { Item = it, Amount = amount, AdditionInfo = it.GetAdditionalInfo()});
        } else {
            entry.Amount += amount;
        }
    }

    public void Add(InventoryEntry it) {
        Add(it.Item, it.Amount);
    }

    public bool Remove(InventoryEntry it) {
        return Remove(it.Item, it.Amount);
    }

    public bool Remove(Item it) {
        return Remove(it, 1);
    }

    public bool Remove(Item it, int amount) {
        InventoryEntry entry = InventoryEntries.FirstOrDefault(e => e.Item == it);
        if(entry == default(InventoryEntry)) {
            return false;
        } else {
            if(entry.Amount - amount < 0) {
                return false;
            }

            entry.Amount -= amount;
            if(entry.Amount == 0) {
                InventoryEntries.Remove(entry);
            }
        }

        return true;
    }

    public bool Contains(Item it, int amount) {
        InventoryEntry entry = InventoryEntries.FirstOrDefault(e => e.Item.ID == it.ID);
        if(entry == default(InventoryEntry) || entry.Amount - amount < 0) {
            return false;
        }

        return true;
    }
    public bool Contains(Item it) => Contains(it, 1);
    public bool Contains(InventoryEntry it) => Contains(it.Item, it.Amount);

    public override string GetSaveString() {
        DummySave ds = new DummySave();
        InventoryEntries.ForEach(e => ds.entries.Add(e.GetIdEntry()));
        return JsonUtility.ToJson(ds);
    }

    private class DummySave {
        public List<InventoryEntry.InventoryEntryWithID> entries = new List<InventoryEntry.InventoryEntryWithID>();
    }

    public override void SetData(string saveString, params object[] otherData) {
        DummySave entries = JsonUtility.FromJson<DummySave>(saveString);
        InventoryEntries.Clear();
        foreach(var entry in entries.entries) {
            var ie = new InventoryEntry() {
                Item = AllItems.First(d => d.ID == entry.Id),
                Amount = entry.Amount,
                AdditionInfo = entry.AdditionalInfo
            };

            if (!ie.AdditionInfo.Equals("")) {
                ie.Item = ScriptableObject.Instantiate(ie.Item);
                ie.Item.Create(ie.AdditionInfo);
            }

            InventoryEntries.Add(ie);
        }
        
    }

    public IEnumerator<InventoryEntry> GetEnumerator() {
        return InventoryEntries.GetEnumerator();
    }

    IEnumerator IEnumerable.GetEnumerator() {
        return InventoryEntries.GetEnumerator();
    }
}
