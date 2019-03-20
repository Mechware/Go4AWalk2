using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Items/Headgear")]
public class Headgear : Item, ITrashable {
    public bool IsTrash() {
        return false;
    }

    public void SetTrash(bool isTrash) {
        //throw new System.NotImplementedException();
    }
}
