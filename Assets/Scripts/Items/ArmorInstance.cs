using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.DropSystem;
using UnityEngine;
using Random = UnityEngine.Random;

public class ArmorInstance : ItemInstance {

    public new ArmorConfig Config => (ArmorConfig) base.Config;
    public new ArmorSaveData SaveData => (ArmorSaveData) base.SaveData;
    
    public float Mod => ModRoll.GetMod(SaveData.Random);
    public string NameMod => ModRoll.GetName(SaveData.Random);

    
    public ArmorInstance(ArmorSaveData saveData) {
        base.Config = Configs.Instance.Items.First(w => w.Id == saveData.Id);
        base.SaveData = saveData;
    }

    public ArmorInstance(ArmorConfig config, int level) {
        base.Config = config;
        base.SaveData = new ArmorSaveData();
        SaveData.Id = config.Id;
        SaveData.Random = Random.Range(0, 101);
        SaveData.Level = level;
    }
    
    
    public float SubtractiveAmount => Config.ArmorAtLevel0 +  Config.ArmorScaling * SaveData.Level;

    public double GetDamage(double damage) {
        return Math.Max(damage - SubtractiveAmount, 0);
    }

    public override string GetName() {
        return $"{NameMod} {Config.Name}";
    }

    public override string GetDescription() {
        return $"ARM Value: {SubtractiveAmount}\n" +
               $"{Config.Description}";
    }

    public override int GetValue() {
        return Mathf.RoundToInt(Config.Value * (1 + SaveData.Level / 10f) * (1 + SaveData.Random / 100f));
    }

}

