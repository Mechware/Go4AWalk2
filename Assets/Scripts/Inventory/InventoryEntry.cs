using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventoryEntry {
    public Item Item;
    public int Amount;

    public InventoryEntryWithID GetIdEntry() {
        return new InventoryEntryWithID() { Id = Item.ID, Amount = Amount };
    }

    [System.Serializable]
    public class InventoryEntryWithID {
        public int Id;
        public int Amount;
    }
}
