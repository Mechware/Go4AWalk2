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
    
    public double RawDamage => GetMaxDamage();
    public int MasteryLevel => Mathf.FloorToInt(RawMasteryLevel);
    public float RawMasteryLevel =>
        Formulas.GetMasteryFromTaps(TapsWithWeapon);
    
    public double TapsWithWeapon => 
        SaveGame.SaveData.IdsToNumberOfTaps.GetOrInsertDefault(Config.Id, 0);
    
    public bool IsEnchanted => Enchantment != null;
    public EnchanterInstance Enchantment { get; private set; }

    public float Mod => ModRoll.GetMod(SaveData.Random);
    public string NameMod => ModRoll.GetName(SaveData.Random);


    public WeaponInstance(WeaponSaveData saveData) {
        base.Config = Configs.Instance.Items.First(w => w.Id == saveData.Id);
        base.SaveData = saveData;

        if (saveData.EnchanterId > 0) {
            Enchantment = new EnchanterInstance(new EnchanterSaveData() {
                Id = saveData.EnchanterId,
                Random = saveData.EnchanterRandom
            });
        }
    }

    public WeaponInstance(WeaponConfig config, int level) {
        base.Config = config;
        base.SaveData = new WeaponSaveData();
        SaveData.Id = config.Id;
        SaveData.Random = UnityEngine.Random.Range(0, 101);
        SaveData.Level = level;
    }

    public double GetRandomDamage() {
        // TODO: Fix this so they aren't just set to floats
        return UnityEngine.Random.Range((float)GetMinDamage(), (float)GetMaxDamage());
    }
    
    public double GetMinDamage() {
        return Formulas.MultiplierFromMaster(MasteryLevel) * GetMaxDamage();
    }
    
    public double GetMaxDamage() {
        return Config.BaseDamage + Config.DamageScaling * SaveData.Level;
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
            return $"Level: {SaveData.Level}\nMastery: {MasteryLevel}\nDamage: {RawDamage}\n{Enchantment.Config.Type.name} Damage: {GetEnchantDamage()}\nValue: {GetValue()}\n{Config.Description}";
        }
        return $"Level: {SaveData.Level}\nMastery: {MasteryLevel}\nDamage: {RawDamage}\nValue: {GetValue()}\n{Config.Description}";
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
