using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.DropSystem;
using UnityEngine;

public class ItemInstance {
    public ItemConfig Config;
    public ItemSaveData SaveData;

    protected ItemInstance(){}
    
    public ItemInstance(ItemConfig c) {
        Config = c;
        SaveData = new ItemSaveData();
    }

    public ItemInstance(ItemSaveData save) {
        SaveData = save;
        Config = Configs.Instance.Items.FirstOrDefault(i => i.Id == SaveData.Id);
        if (Config == null) {
            Debug.LogError("Could not find config for id: " + save.Id);
        }
    }
    
    public virtual int GetValue() {
        return Config.Value;
    }

    public virtual string GetName() {
        return Config.Name;
    }

    public virtual string GetDescription() {
        return Config.Description;
    }
}
