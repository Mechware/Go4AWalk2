using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Instrument")]
public class InstrumentData : ScriptableObject {

    public Sprite Icon;
    
    public float BoostDurationMultiplier;
    public float SongAccuracyAdd;
    public List<Drop> MonsterDropBoost;
}
