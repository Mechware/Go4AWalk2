using G4AW2;
using G4AW2.Data;
using System.Collections.Generic;
using System.Linq;

public class ShopFollowerInstance : FollowerInstance {

    public new ShopFollowerConfig Config => (ShopFollowerConfig) base.Config;
    public new ShopFollowerSaveData SaveData => (ShopFollowerSaveData) base.SaveData;

    public List<ItemInstance> Items;
    
    public ShopFollowerInstance(ShopFollowerConfig config) {
        foreach (var conf in config.Items) {
            var instance = ItemFactory.GetInstance(conf.ItemConfig, conf.Level);
            Items.Add(instance);
            SaveData.Items.Add(instance.SaveData);
        }
    }

    public ShopFollowerInstance(ShopFollowerSaveData saveData) {
        base.Config = Configs.Instance.Followers.First(f => f.Id == saveData.Id);
        foreach(var itemData in SaveData.Items) {
            var instance = ItemFactory.GetInstance(itemData);
            Items.Add(instance);
        }

    }

}
