using G4AW2.Data.DropSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Consumable : Item {

    public enum ConsumableType {
        Health,
        Damage,
        Speed,
        Bait,
        
    }
    
    public float Duration;
    public float Affect;
    public ConsumableType Type;
}
