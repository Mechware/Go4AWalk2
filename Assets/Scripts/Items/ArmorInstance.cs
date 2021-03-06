using G4AW2;
using G4AW2.Data.DropSystem;
using System.Linq;
using UnityEngine;

public class ArmorInstance : ItemInstance {

    public new ArmorConfig Config => (ArmorConfig) base.Config;
    public new ArmorSaveData SaveData => (ArmorSaveData) base.SaveData;
    
    public float Mod => ModRoll.GetMod(SaveData.Random);
    public string NameMod => ModRoll.GetName(SaveData.Random);

    
    public ArmorInstance(ArmorSaveData saveData, ArmorConfig config) {
        base.Config = config;
        base.SaveData = saveData;
    }

    public ArmorInstance(ArmorConfig config, int level) {
        base.Config = config;
        base.SaveData = new ArmorSaveData(config.name, Random.Range(0, 101), level);
    }
    
    
    private float BadParryMod = 0.5f;
    public float ArmValue => Mathf.RoundToInt(Config.ArmorAtLevel0 * Mod * (1 + SaveData.Level / 100f));
    private float NoBlockModifierWithMod => Mathf.Max(1 - ArmValue / 100, 0);
    private float PerfectBlockModifierWithMod => (-1*(ArmValue/25)); // blocking heals
    private float MistimedBlockModifierWithMod => (-1*(ArmValue/50)); // blocking heals


    public float GetDamage(int damage, BlockState state) {
        float fdamage = damage;
        fdamage = Mathf.Max(0, fdamage);

        if(state == BlockState.PerfectlyBlocking) {
            return fdamage * PerfectBlockModifierWithMod;
        }

        if(state == BlockState.Blocking) {
            return fdamage * MistimedBlockModifierWithMod;
        }

        if(state == BlockState.BadParry) {
            return fdamage * BadParryMod * NoBlockModifierWithMod;
        }

        return fdamage * NoBlockModifierWithMod;
    }

    public override string GetName() {
        return $"{NameMod} {Config.Name}";
    }

    public override string GetDescription() {
        return $"ARM Value: {ArmValue}\n" +
               $"{Config.Description}";
    }

    public override int GetValue() {
        return Mathf.RoundToInt(Config.Value * (1 + SaveData.Level / 10f) * (1 + SaveData.Random / 100f));
    }

}

public enum BlockState { None, Blocking, PerfectlyBlocking, BadParry}
