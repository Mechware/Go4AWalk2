using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class ShopFollowerInstance : FollowerInstance {

    public new ShopFollowerConfig Config => (ShopFollowerConfig) base.Config;
    public new ShopFollowerSaveData SaveData => (ShopFollowerSaveData) base.SaveData;

    public List<ItemInstance> Items;
    
    public ShopFollowerInstance(ShopFollowerConfig config) {
        foreach (var conf in config.Items) {
            if (conf.ItemConfig is ArmorConfig a) {
                var armor = new ArmorInstance(a, conf.Level);
                Items.Add(armor);
                SaveData.Items.Add(armor.SaveData);
            }
            else if (conf.ItemConfig is WeaponConfig w) {
                var weapon = new WeaponInstance(w, conf.Level);
                Items.Add(weapon);
                SaveData.Items.Add(weapon.SaveData);
            }
        }
    }

}
