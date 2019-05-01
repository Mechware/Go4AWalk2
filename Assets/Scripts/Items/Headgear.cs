using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Items/Headgear")]
public class Headgear : Item, ITrashable, ISaveable {

    private bool isTrash = false;

    public bool IsTrash() {
        return isTrash;
    }

    public void SetTrash(bool isTrash) {
        this.isTrash = isTrash;
        DataChanged?.Invoke();
    }

    private class DummySave {
        public int ID;
    }

    public string GetSaveString() {
        return JsonUtility.ToJson(new DummySave() { ID = ID});
    }

    public void SetData(string saveString, params object[] otherData) {

        DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

        ID = ds.ID;

        if(CreatedFromOriginal)
            return;

        Headgear original;

        if(otherData[0] is PersistentSetItem) {
            PersistentSetItem allItems = (PersistentSetItem) otherData[0];
            original = allItems.First(it => it.ID == ID) as Headgear;
        } else {
            original = otherData[0] as Headgear;
        }

        // Copy Original Values
        base.CopyValues(original);
    }
}
