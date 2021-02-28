using G4AW2;
using G4AW2.Data;
using G4AW2.Followers;
using G4AW2.Utils;
using System.Linq;

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

    public QuestGiverInstance(QuestGiverSaveData savedata, QuestGiverConfig config) {
        base.SaveData = savedata;
        base.Config = config;
    }

}
