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


    private QuestConfig previous;

    private void Awake() {
        Instance = this;
    }

    public void SetQuest(QuestConfig currentQuestConfig) {

        RemoveListeners(previous);

        if (currentQuestConfig is ActiveWalkingQuestConfig) {
            ActiveWalkingQuestConfig awq = currentQuestConfig as ActiveWalkingQuestConfig;
            awq.AmountSoFar.OnChange.AddListener(OnChange);
        } else if (currentQuestConfig is ActiveQuestConfig<int, IntVariable, UnityEventInt>) {
            ActiveQuestConfig < int, IntVariable, UnityEventInt> awq = currentQuestConfig as ActiveQuestConfig<int, IntVariable, UnityEventInt>;
            awq.AmountSoFar.OnChange.AddListener(OnChange);
        } else if (currentQuestConfig is ReachValueQuestConfig) {
            var awq = currentQuestConfig as ReachValueQuestConfig;
            awq.TotalAmount.OnChange.AddListener(OnChange);
        }

        var prog = currentQuestConfig.GetProgress();
        ProgressText.text = $"{prog.current} / {prog.max}";
        ProgressFill.rectTransform.anchorMax =
            ProgressFill.rectTransform.anchorMax.SetX(Mathf.Clamp01((float) (prog.current / prog.max)));
        QuestTitle.text = currentQuestConfig.DisplayName;
        previous = currentQuestConfig;
    }

    void RemoveListeners(QuestConfig previousQuestConfig) {
        if (previousQuestConfig == null) return;

        if(previousQuestConfig is ActiveWalkingQuestConfig) {
            ActiveWalkingQuestConfig awq = previousQuestConfig as ActiveWalkingQuestConfig;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        } else if(previousQuestConfig is ActiveEnemySlayerQuestConfig) {
            ActiveEnemySlayerQuestConfig awq = previousQuestConfig as ActiveEnemySlayerQuestConfig;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        } else if(previousQuestConfig is ActiveItemCollectQuestConfig) {
            ActiveItemCollectQuestConfig awq = previousQuestConfig as ActiveItemCollectQuestConfig;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        }

        if(previousQuestConfig is ActiveWalkingQuestConfig) {
            ActiveWalkingQuestConfig awq = previousQuestConfig as ActiveWalkingQuestConfig;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        } else if(previousQuestConfig is ActiveQuestConfig<int, IntVariable, UnityEventInt>) {
            ActiveQuestConfig<int, IntVariable, UnityEventInt> awq = previousQuestConfig as ActiveQuestConfig<int, IntVariable, UnityEventInt>;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        } else if(previousQuestConfig is ReachValueQuestConfig) {
            var q = previousQuestConfig as ReachValueQuestConfig;
            q.TotalAmount.OnChange.RemoveListener(OnChange);
        }
    }

    void OnChange(float val) {
        SetQuest(previous);

    }

    public Transform SpawnPointOfNumberIncreasePopUp;

    private int prevVal = -1;
    void OnChange(int val) {
        if (prevVal != -1 && val > prevVal) {
            SmoothPopUpManager.ShowPopUp(SpawnPointOfNumberIncreasePopUp.position, "+" + (val - prevVal), Color.green, true);
        }
        prevVal = val;
        var prog = previous.GetProgress();
        ProgressText.text = $"{prog.current} / {prog.max}";
        ProgressFill.rectTransform.anchorMax =
            ProgressFill.rectTransform.anchorMax.SetX(Mathf.Clamp01((float) (prog.current / prog.max)));
        
    }
}
