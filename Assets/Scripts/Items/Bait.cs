using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using UnityEngine;

[CreateAssetMenu(menuName = "Data/Items/Bait")]
public class Bait : Consumable
{
    public List<Drop> DropChances;
    public float MinDropTime;
    public float MaxDropTime;


    public float GetSpawnTime() {
		
        return MinDropTime + UnityEngine.Random.Range(0, 1f) * (MaxDropTime - MinDropTime);
    }
}


[Serializable]
public class Drop {
    public FollowerConfig Config;
    public int Chance;
}

