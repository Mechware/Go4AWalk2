using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Instrument")]
public class InstrumentData : Item {
    public Sprite Icon;
    
    public float BoostDurationMultiplier;
    public float SongAccuracyAdd;
    public List<Drop> MonsterDropBoost;
    
    
#if UNITY_EDITOR
    [ContextMenu("Pick ID")]
    public void PickID() {
        ID = IDUtils.PickID<Item>();
    }
#endif
}
