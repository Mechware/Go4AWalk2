using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class HeadgearInstance : ItemInstance{
    public new HeadgearConfig Config => (HeadgearConfig) base.Config;
    public new HeadgearSaveData SaveData => (HeadgearSaveData) base.SaveData;
    
    public int ExtraHealth => Formulas.GetValue(Config.HealthGainedAtLevel0, SaveData.Level, Mod);
    public float Mod => ModRoll.GetMod(SaveData.Random);
    public string NameMod => ModRoll.GetName(SaveData.Random);


    public HeadgearInstance(HeadgearSaveData saveData) {
        base.Config = Configs.Instance.ItemConfigs.First(w => w.Id == saveData.Id);
        base.SaveData = saveData;
    }

    public HeadgearInstance(HeadgearConfig config, int level) {
        
        base.SaveData = new HeadgearSaveData();
        SaveData.Id = config.Id;
        SaveData.Random = UnityEngine.Random.Range(0, 101);
        
        SaveGame.Instance.Items.Add(SaveData);
    }
    
    public override string GetDescription() {
        return $"Level: {SaveData.Level}\nHealth Add: {ExtraHealth}\nValue: {GetValue()}\n{Config.Description}";
    }

    public override string GetName() {
        return $"{NameMod} {Config.Name}";
    }

    public override int GetValue() {
        return Mathf.RoundToInt(Config.Value * (1 + SaveData.Level / 10f) * (1 + SaveData.Random / 100f));
    }
}
