using G4AW2;
using System.Linq;
using UnityEngine;

public class EnchanterInstance : ItemInstance {
    public new EnchanterConfig Config => (EnchanterConfig) base.Config;
    public new EnchanterSaveData SaveData => (EnchanterSaveData) base.SaveData;
    
    private const float DAMAGE_SCALING = 2.5F;

    
    public EnchanterInstance(EnchanterSaveData saveData, EnchanterConfig config) {
        base.Config = config;
        base.SaveData = saveData;
    }

    public EnchanterInstance(EnchanterConfig config) {
        
        base.SaveData = new EnchanterSaveData();
        SaveData.Id = config.Id;
        SaveData.Random = UnityEngine.Random.Range(0, 101);
    }

    
    public float GetAdditiveDamage(WeaponInstance w) {
        return DAMAGE_SCALING * (1 + Config.Type.GetDamage(SaveData.Random) / 10f) * (1 + w.SaveData.Level / 10);
    }
    
    public override int GetValue() {
        return Mathf.RoundToInt(Config.Value * (1 + SaveData.Random / 100f));
    }

    public override string GetName() {
        return Config.Type.GetPrefix(SaveData.Random) + " " + Config.Name;
    }

    public string GetPrefix() {
        return Config.Type.GetPrefix(SaveData.Random);
    }

    public override string GetDescription() {
        return $"Type: {Config.Type.name}\nValue: {GetValue()}";
    }
}
