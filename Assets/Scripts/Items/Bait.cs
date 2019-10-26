using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using UnityEngine;

public class Bait : Consumable
{
    public float BuffDuration;
    public List<Drop> DropChances;
    public float MinDropTime;
    public float MaxDropTime;


    public float GetSpawnTime(float acc) {
		
        return MinDropTime + (1f - acc) * (MaxDropTime - MinDropTime) +
               UnityEngine.Random.Range(-0.5f, 0.5f) * (MaxDropTime - MinDropTime);
    }
}


[Serializable]
public class Drop {
    public FollowerData Data;
    public int Chance;
}

