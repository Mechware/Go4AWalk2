using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Questing;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public static TutorialManager Instance;
    
    public QuestConfig BlockingAndParryingTutorialStart;
    public QuestConfig BlockingAndParryingTutorialEnd;

    void Awake() {
        Instance = this;
    }

    public void SetQuest(QuestConfig questConfig) {
        if (questConfig == BlockingAndParryingTutorialStart) {
            SaveGame.SaveData.ShowParryAndBlockColors = true;
        }
        if (questConfig == BlockingAndParryingTutorialEnd) {
            SaveGame.SaveData.ShowParryAndBlockColors = false;
        }
    }
}
