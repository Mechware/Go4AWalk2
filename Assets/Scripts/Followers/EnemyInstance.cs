using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.Combat;
using UnityEngine;

public class EnemyInstance {
    public EnemyStats Config;
    public EnemySaveData SaveData;
    
    
    public int MaxHealth => Mathf.RoundToInt(Config.HealthAtLevel0 * (1 + SaveData.Level / 10f));
    public int Damage => Mathf.RoundToInt(Config.DamageAtLevel0 * (1 + SaveData.Level / 10f));
    public int ElementalDamage => Mathf.RoundToInt(Config.ElementalDamageAtLevel0 * (1 + SaveData.Level / 10f));

    public EnemyInstance(EnemyStats config, int level) {
        SaveData = new EnemySaveData();
        Config = config;
        SaveData.Id = Config.Id;
        SaveData.Level = level;
    } 

    public EnemyInstance(EnemySaveData saveData) {
        Config = (EnemyStats) Configs.Instance.Followers.First(f => f.Id == saveData.Id);
    }

    public float GetElementalWeakness(ElementalType type) {
        return Config.ElementalWeaknesses?[type] ?? 1;
    }

    public float GetRandomAttackSpeed() {
        return Random.Range(Config.MinAttackSpeed, Config.MaxAttackSpeed);
    }

    public bool Dodge(float incomingSpeed) {
        return Formulas.Dodge(Config.Weight, incomingSpeed, Config.DodgePerk);
    }
}
