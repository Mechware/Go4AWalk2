using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Questing;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestingStatWatcher : MonoBehaviour {

    public static QuestingStatWatcher Instance;
    
    public TextMeshProUGUI QuestTitle;
    public TextMeshProUGUI ProgressText;
    public Image ProgressFill;


    private int prevVal = -1;
    private QuestConfig previous;
    public Transform SpawnPointOfNumberIncreasePopUp;

    public void Awake() {
        Instance = this;
    }

    public void MyUpdate() {

        int current = QuestManager.Instance.CurrentQuest.SaveData.Progress;
        int max = QuestManager.Instance.CurrentQuest.Config.ValueToReach;

        ProgressText.text = $"{current} / {max}";
        ProgressFill.rectTransform.anchorMax =
            ProgressFill.rectTransform.anchorMax.SetX(Mathf.Clamp01((float) (current / max)));
        QuestTitle.text = QuestManager.Instance.CurrentQuest.Config.DisplayName;

        if(prevVal != -1 && current> prevVal) {
            SmoothPopUpManager.ShowPopUp(SpawnPointOfNumberIncreasePopUp.position, "+" + (current - prevVal), Color.green, true);
            prevVal = current;
        }
    }

}
