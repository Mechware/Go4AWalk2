using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Questing;
using UnityEngine;

public class TutorialManager : MonoBehaviour {

    [Obsolete("Singleton")]
    public static TutorialManager Instance;
    
    public BoolVariable ShowParryAndBlockColor;
    public ActiveQuestBase BlockingAndParryingTutorialStart;
    public ActiveQuestBase BlockingAndParryingTutorialEnd;

    void Awake() {
        Instance = this;
    }
    
    public void QuestUpdated(ActiveQuestBase Quest) {
        if (Quest == BlockingAndParryingTutorialStart) {
            ShowParryAndBlockColor.Value = true;
        }
        if (Quest == BlockingAndParryingTutorialEnd) {
            ShowParryAndBlockColor.Value = false;
        }
    }
}
