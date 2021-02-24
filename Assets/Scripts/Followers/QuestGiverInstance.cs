using System.Collections;
using System.Collections.Generic;
using System.Linq;
using G4AW2.Followers;
using G4AW2.Data;
using G4AW2.Utils;
using UnityEngine;

public class QuestGiverInstance : FollowerInstance {
    public new QuestGiverConfig Config => (QuestGiverConfig) base.Config;
    public new QuestGiverSaveData SaveData => (QuestGiverSaveData) base.SaveData;

    public QuestConfig QuestToGive;


    public QuestGiverInstance(QuestGiverConfig c) {
        base.Config = c;
        QuestToGive = c.QuestConfigToGive.GetRandom();
        base.SaveData = new QuestGiverSaveData();
        SaveData.QuestToGiveId = QuestToGive.Id;

    }

    public QuestGiverInstance(QuestGiverSaveData savedata) {
        base.SaveData = savedata;
        base.Config = (QuestGiverConfig) Configs.Instance.Followers.First(f => f.Id == savedata.Id);
    }

}
