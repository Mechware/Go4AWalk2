using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Questing;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public static TutorialManager Instance;
    
    public ActiveQuestBase BlockingAndParryingTutorialStart;
    public ActiveQuestBase BlockingAndParryingTutorialEnd;

    void Awake() {
        Instance = this;
    }

    public void Initialize() {
        QuestManager.Instance.QuestUpdated += QuestUpdated;
    }
    
    public void QuestUpdated(ActiveQuestBase quest) {
        if (quest == BlockingAndParryingTutorialStart) {
            SaveGame.SaveData.ShowParryAndBlockColors = true;
        }
        if (quest == BlockingAndParryingTutorialEnd) {
            SaveGame.SaveData.ShowParryAndBlockColors = false;
        }
    }
}
