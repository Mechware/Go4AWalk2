using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    public QuestConfig BlockingAndParryingTutorialStart;
    public QuestConfig BlockingAndParryingTutorialEnd;

    public void SetQuest(QuestConfig questConfig) {
        if (questConfig == BlockingAndParryingTutorialStart) {
            SaveGame.SaveData.ShowParryAndBlockColors = true;
        }
        if (questConfig == BlockingAndParryingTutorialEnd) {
            SaveGame.SaveData.ShowParryAndBlockColors = false;
        }
    }
}
