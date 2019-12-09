using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using CustomEvents;
using G4AW2.Data.DropSystem;
using G4AW2.Utils;
using UnityEngine;
using Random = System.Random;

public class WeaponInstance : ItemInstance {
    
    public new WeaponConfig Config => (WeaponConfig) base.Config;
    public new WeaponSaveData SaveData => (WeaponSaveData) base.SaveData;
    
    public int RawDamage => GetDamage();
    public int Mastery => Mathf.FloorToInt( ConfigObject.GetLevel(Config.Rarity, MasteryLevels.GetTaps(Config.Id)));
    public float RawMastery => ConfigObject.GetLevel(Config.Rarity, MasteryLevels.GetTaps(Config.Id));
    
    public bool IsEnchanted => Enchantment != null;
    public EnchanterInstance Enchantment { get; private set; }

    public float Mod => ModRoll.GetMod(SaveData.Random);
    public string NameMod => ModRoll.GetName(SaveData.Random);


    public WeaponInstance(WeaponSaveData saveData) {
        base.Config = Configs.Instance.ItemConfigs.First(w => w.Id == saveData.Id);
        base.SaveData = saveData;

        if (saveData.EnchanterId > 0) {
            Enchantment = new EnchanterInstance(new EnchanterSaveData() {
                Id = saveData.EnchanterId,
                Random = saveData.EnchanterRandom
            });
        }
    }

    public WeaponInstance(WeaponConfig config, int level) {
        
        base.SaveData = new WeaponSaveData();
        SaveData.Id = config.Id;
        SaveData.Random = UnityEngine.Random.Range(0, 101);
        SaveData.Level = level;
    }
    
    public int GetDamage(int? mastery = null, float? damageAtLevel0 = null, int? level = null, float? mod = null, float? additiveDamage = null, float? damageMultiple = null) {
        int imastery = mastery ?? Mastery;
        float fdamageAtLevel0 = damageAtLevel0 ?? Config.DamageAtLevel0;
        float fmod = mod ?? Mod;
        float ilevel = level ?? SaveData.Level;
            
        float masteryMod = imastery == 99 ? 2.15f : 1 + imastery / 100f;
        float baseRawDamage = fdamageAtLevel0 * masteryMod * (1 + ilevel / 10f) * fmod;
        return Mathf.RoundToInt(baseRawDamage);
    }

    public int GetDamage() {
        float masteryMod = Mastery == 99 ? 2.15f : 1 + Mastery / 100f;
        float baseRawDamage = Config.DamageAtLevel0 * masteryMod * (1 + SaveData.Level / 10f) * Mod;
        return Mathf.RoundToInt(baseRawDamage);
    }
    
    private int lastLevel = -1;
    
    // TODO(Mike): Move this.
    void TapsChanged(int amount) {

        if (!MasteryLevels.Loaded) return;

        if (lastLevel == -1) {
            lastLevel = Mastery;
            return;
        }

        MasteryLevels.Tap(Config.Id);

        if (Mastery != lastLevel) {
            
            MainUI.Instance.MasteryPopUp($"Mastery Level {Mastery}");
            lastLevel = Mastery;
        }
    }

    public override string GetName() {
        return GetName(true, true);
    }
    
    public string GetName(bool enchantInclude, bool includeNameMod) {
        enchantInclude = IsEnchanted && enchantInclude;
        if (enchantInclude && includeNameMod) {
            return $"{Enchantment.GetPrefix()} {NameMod} {Config.Name}";
        }
        if (enchantInclude) {
            return $"{Enchantment.GetPrefix()} {Config.Name}";
        }
        if (includeNameMod) {
            return $"{NameMod} {Config.Name}";
        }

        return Config.Name;
        
    }

    public override string GetDescription() {
        if (IsEnchanted) {
            return $"Level: {SaveData.Level}\nMastery: {Mastery}\nDamage: {RawDamage}\n{Enchantment.Config.Type.name} Damage: {GetEnchantDamage()}\nValue: {GetValue()}\n{Config.Description}";
        }
        return $"Level: {SaveData.Level}\nMastery: {Mastery}\nDamage: {RawDamage}\nValue: {GetValue()}\n{Config.Description}";
    }

    public override int GetValue() {
        return Mathf.RoundToInt(Config.Value * (1 + SaveData.Level / 10f) * (1 + SaveData.Random / 100f)) + (IsEnchanted ? Enchantment.GetValue() : 0);
    }

    public void Enchant(EnchanterInstance e) {
        Enchantment = e;
    }

    public int GetEnchantDamage() {
        return Enchantment == null ? 0 : Mathf.RoundToInt(Enchantment.GetAdditiveDamage(this));
    }
}
