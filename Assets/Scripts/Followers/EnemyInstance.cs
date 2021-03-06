using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2;
using G4AW2.Data.Combat;
using UnityEngine;

public class EnemyInstance : FollowerInstance {
    public new EnemyConfig Config => (EnemyConfig) base.Config;
    public new EnemySaveData SaveData => (EnemySaveData) base.SaveData;

    public bool Suicide = false;
    public int MaxHealth => Mathf.RoundToInt(Config.HealthAtLevel0 * (1 + SaveData.Level / 10f));
    public int Damage => Mathf.RoundToInt(Config.DamageAtLevel0 * (1 + SaveData.Level / 10f));
    public int ElementalDamage => Mathf.RoundToInt(Config.ElementalDamageAtLevel0 * (1 + SaveData.Level / 10f));

    public EnemyInstance(EnemyConfig config, int level) {
        base.SaveData = new EnemySaveData(config.name, level);
        base.Config = config;
    } 

    public EnemyInstance(EnemySaveData saveData, EnemyConfig config) {
        base.Config = config;
        base.SaveData = saveData;
    }

    public float GetElementalWeakness(ElementalType type) {
        return Config.ElementalWeaknesses?[type] ?? 1;
    }
}
