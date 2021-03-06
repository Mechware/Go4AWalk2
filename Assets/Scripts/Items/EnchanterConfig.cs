using G4AW2.Data.DropSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Items/Enchanter")]
public class EnchanterConfig : ItemConfig {

    public ElementalType Type;
    [Tooltip("Gem/Crystal/Other")]
    public GemType GemTypeType;
}

public enum GemType {
    Gem = 0,
    Jewel = 15,
    Crystal = 30,
}