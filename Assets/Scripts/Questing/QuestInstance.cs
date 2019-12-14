using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Data.Combat;
using G4AW2.Data.DropSystem;
using G4AW2.Questing;
using UnityEngine;

public class QuestInstance {
    public QuestConfig Config;
    public QuestSaveData SaveData;

    public QuestInstance(QuestConfig config) {
        Config = config;
        SaveData = new QuestSaveData();
        SaveData.Id = config.Id;
    }
    
    public QuestInstance(QuestSaveData saveData) {
        Config = Configs.Instance.Quests.First(q => q.Id == saveData.Id);
    }

    public bool IsFinished() {
        return Config.ValueToReach >= SaveData.Progress;
    }
}
