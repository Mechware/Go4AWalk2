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

public class SongBuffController : MonoBehaviour {

    public static SongBuffController Instance;

    private void Awake() {
        Instance = this;
    }

    public SongBuffData CurrentBuffs;
    public InstrumentData CurrentInstrument => DataManager.Instance.Player.Instrument;

    private void Update() {

        double currentTime = RandomUtils.GetTime();
        
        for(int i = 0; i < CurrentBuffs.Buffs.Count; i++) {
            var buff = CurrentBuffs.Buffs[i];

            if (currentTime > buff.BuffEndTime) {
                // Buff is done, but want to check if you could spawn any other monsters
                
                while (buff.NextSpawnTime < buff.BuffEndTime) {
                    // Pick a monster that you can spawn
                    var monster = GetDrop(buff.Accuracy, buff.Song, buff.Instrument);

                    // Drop it
                    FollowerSpawner.Instance.Drop(monster);
                    buff.NextSpawnTime += buff.Song.GetSpawnTime(buff.Accuracy);
                    
                }
                CurrentBuffs.Buffs.RemoveAt(i);
                i--;
                continue;
            }
            
            if (currentTime > buff.NextSpawnTime) {
                // Pick a monster that you can spawn
                var monster = GetDrop(buff.Accuracy, buff.Song, buff.Instrument);

                // Check if it is in the list
                FollowerSpawner.Instance.Drop(monster);   
                buff.NextSpawnTime += buff.Song.GetSpawnTime(buff.Accuracy);
            }
        }
    }

    public void OnSongFinish(SongData song, float accuracy) {
        if (accuracy < 0.01f) return;
        
        SingleSongBuffData data = new SingleSongBuffData();
        data.Instrument = CurrentInstrument;
        data.Song = song;
        data.Accuracy = accuracy;
        if (CurrentInstrument != null) {
            data.Accuracy += CurrentInstrument.SongAccuracyAdd;
        }
        
        float instrumentBuff = 1;
        if (CurrentInstrument != null) {
            instrumentBuff = CurrentInstrument.BoostDurationMultiplier;
        }

        data.BuffEndTime = RandomUtils.GetTime() + song.BuffDuration * instrumentBuff;
        data.NextSpawnTime = RandomUtils.GetTime() + song.GetSpawnTime(accuracy) / 2f; // Spawn first one more quickly
        
        CurrentBuffs.Buffs.Add(data);
    }

    public FollowerData GetDrop(float acc, SongData song, InstrumentData instrumentData) {
        int total = song.DropChances.Where(d => d.MinAccuracy <= acc).Sum(d => d.Chance);

        if (instrumentData != null) {
            total += (instrumentData.MonsterDropBoost.Where(d => d.MinAccuracy <= acc).Sum(d => d.Chance));
        }

        int roll = Random.Range(1, total + 1);

        foreach (var drop in song.DropChances) {
            if (drop.MinAccuracy > acc) continue;
            roll -= drop.Chance;
            if (roll <= 0) {
                return drop.Data;
            }
        }

        if (instrumentData != null) {
            foreach (var drop in instrumentData.MonsterDropBoost) {
                if (drop.MinAccuracy > acc) continue;
                roll -= drop.Chance;
                if (roll <= 0) {
                    return drop.Data;
                }
            }
        }

        Debug.LogError($"Could not find a drop from song: {name}");
        return null;
    }

}

[Serializable]
public class SongBuffData {
    public List<SingleSongBuffData> Buffs;
}
[Serializable]
public class SingleSongBuffData {
    public double BuffEndTime;
    public double NextSpawnTime;
    public float Accuracy;
    public SongData Song;
    public InstrumentData Instrument;
}