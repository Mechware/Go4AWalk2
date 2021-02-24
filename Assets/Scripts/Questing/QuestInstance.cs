using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Data;
using UnityEngine;

public class QuestInstance {
    public QuestConfig Config;
    public QuestSaveData SaveData;

    public QuestInstance(QuestConfig config, bool active) {
        Config = config;
        SaveData = new QuestSaveData();
        SaveData.Id = config.Id;
        SaveData.Active = active;
    }
    
    public QuestInstance(QuestSaveData saveData) {
        SaveData = saveData;
        Config = Configs.Instance.Quests.First(q => q.Id == saveData.Id);
    }

    public bool IsFinished() {
        return Config.ValueToReach >= SaveData.Progress;
    }
}
