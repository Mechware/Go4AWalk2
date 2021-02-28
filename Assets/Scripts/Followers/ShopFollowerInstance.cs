using G4AW2;
using G4AW2.Data;
using G4AW2.Managers;
using System.Collections.Generic;
using System.Linq;

public class ShopFollowerInstance : FollowerInstance {

    public new ShopFollowerConfig Config => (ShopFollowerConfig) base.Config;
    public new ShopFollowerSaveData SaveData => (ShopFollowerSaveData) base.SaveData;

    public List<ItemInstance> Items;
    
    public ShopFollowerInstance(ShopFollowerConfig config, ItemManager items) {
        foreach (var conf in config.Items) {
            var instance = items.CreateInstance(conf.ItemConfig, conf.Level);
            Items.Add(instance);
            SaveData.Items.Add(instance.SaveData);
        }
    }

    public ShopFollowerInstance(ShopFollowerSaveData saveData, ShopFollowerConfig config, ItemManager items) {
        base.Config = config;
        foreach(var itemData in SaveData.Items) {
            var instance = items.CreateInstance(itemData);
            Items.Add(instance);
        }
    }
}
