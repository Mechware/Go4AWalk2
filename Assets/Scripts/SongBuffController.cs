using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Combat;
using G4AW2.Data;
using G4AW2.Followers;
using G4AW2.Utils;
using UnityEngine;
using Random = UnityEngine.Random;

public class BaitBuffController : MonoBehaviour {

    public static BaitBuffController Instance;

    [NonSerialized] public List<SingleBaitData> SaveData;
    
    private void Awake() {
        Instance = this;
    }

    public InstrumentData CurrentInstrument => DataManager.Instance.Player.Instrument;

    private void Update() {

        double currentTime = RandomUtils.GetTime();
        
        for(int i = 0; i < SaveData.Count; i++) {
            var buff = SaveData[i];

            if (currentTime > buff.BuffEndTime) {
                // Buff is done, but want to check if you could spawn any other monsters
                
                while (buff.NextSpawnTime < buff.BuffEndTime) {
                    // Pick a monster that you can spawn
                    var monster = GetDrop(buff.Bait);

                    // Drop it
                    FollowerSpawner.Instance.Drop(monster);
                    buff.NextSpawnTime += buff.Bait.GetSpawnTime(buff.Accuracy);
                    
                }
                SaveData.RemoveAt(i);
                i--;
                continue;
            }
            
            if (currentTime > buff.NextSpawnTime) {
                // Pick a monster that you can spawn
                var monster = GetDrop(buff.Bait);

                // Check if it is in the list
                FollowerSpawner.Instance.Drop(monster);   
                buff.NextSpawnTime += buff.Bait.GetSpawnTime(buff.Accuracy);
            }
        }
    }

    public FollowerData GetDrop(Bait bait) {
        int total = bait.DropChances.Sum(d => d.Chance);
        if (total == 0) return null;

        int roll = Random.Range(1, total + 1);

        foreach (var drop in bait.DropChances) {
            roll -= drop.Chance;
            if (roll <= 0) {
                return drop.Data;
            }
        }

        return null;
    }

}

[Serializable]
public class SingleBaitData {
    public double BuffEndTime;
    public double NextSpawnTime;
    public float Accuracy;
    public Bait Bait;
    public InstrumentData Instrument;
}