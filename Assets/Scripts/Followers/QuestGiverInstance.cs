using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Followers;
using G4AW2.Questing;
using G4AW2.Utils;
using UnityEngine;

public class QuestGiverInstance {
    public QuestGiverConfig Config;
    public QuestGiverSaveData SaveData;

    public QuestConfig QuestToGive;

    public QuestGiverInstance(QuestGiverConfig c) {
        Config = c;
        QuestToGive = c.QuestConfigToGive.GetRandom();
        SaveData = new QuestGiverSaveData();
        SaveData.QuestToGiveId = QuestToGive.Id;
    }

    public QuestGiverInstance(QuestGiverSaveData savedata) {
        SaveData = savedata;
        Config = Configs.Instance.QuestGivers.First(f => f.Id == savedata.Id);
    }

}
