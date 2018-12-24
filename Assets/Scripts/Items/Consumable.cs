using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[CreateAssetMenu(menuName ="Data/DropSystem/Consumable")]
public class Consumable : Item {

    public UnityEvent OnUse;

}
