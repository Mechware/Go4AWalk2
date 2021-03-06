using G4AW2.Data;
using G4AW2.Data.DropSystem;
using UnityEngine;

[CreateAssetMenu(menuName ="Data/Items/Consumable")]
public class ConsumableConfig : ItemConfig {

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
