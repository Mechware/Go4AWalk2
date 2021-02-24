using CustomEvents;
using G4AW2.Data.DropSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
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