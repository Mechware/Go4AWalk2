using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class ShopFollowerInstance {

    public ShopFollowerConfig Config;
    public ShopFollowerSaveData SaveData;

    public List<ItemInstance> Items;
    
    public ShopFollowerInstance(ShopFollowerConfig config) {
        foreach (var conf in config.Items) {
            var instance = ItemFactory.GetInstance(conf.ItemConfig, conf.Level);
            Items.Add(instance);
            SaveData.Items.Add(instance.SaveData);
        }
    }

    public ShopFollowerInstance(ShopFollowerSaveData saveData) {
        Config = Configs.Instance.Shops.First(f => f.Id == saveData.Id);
        foreach(var itemData in SaveData.Items) {
            var instance = ItemFactory.GetInstance(itemData);
            Items.Add(instance);
        }

    }

}
