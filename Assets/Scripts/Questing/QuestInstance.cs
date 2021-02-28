using G4AW2;
using G4AW2.Data;
using System.Linq;

public class QuestInstance {
    public QuestConfig Config;
    public QuestSaveData SaveData;

    public QuestInstance(QuestConfig config, bool active) {
        Config = config;
        SaveData = new QuestSaveData();
        SaveData.Id = config.Id;
        SaveData.Active = active;
    }
    
    public QuestInstance(QuestConfig config, QuestSaveData saveData) {
        SaveData = saveData;
        Config = config;
    }

    public bool IsFinished() {
        return Config.ValueToReach <= SaveData.Progress;
    }
}
