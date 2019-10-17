using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Questing;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestingStatWatcher : MonoBehaviour {

    public TextMeshProUGUI QuestTitle;
    public TextMeshProUGUI ProgressText;
    public Image ProgressFill;


    private ActiveQuestBase previous;

    public void SetQuest(ActiveQuestBase currentQuest) {

        RemoveListeners(previous);

        if (currentQuest is ActiveWalkingQuest) {
            ActiveWalkingQuest awq = currentQuest as ActiveWalkingQuest;
            awq.AmountSoFar.OnChange.AddListener(OnChange);
        } else if (currentQuest is ActiveQuest<int, IntVariable, UnityEventInt>) {
            ActiveQuest < int, IntVariable, UnityEventInt> awq = currentQuest as ActiveQuest<int, IntVariable, UnityEventInt>;
            awq.AmountSoFar.OnChange.AddListener(OnChange);
        } else if (currentQuest is ReachValueQuest) {
            var awq = currentQuest as ReachValueQuest;
            awq.TotalAmount.OnChange.AddListener(OnChange);
        }

        var prog = currentQuest.GetProgress();
        ProgressText.text = $"{prog.current} / {prog.max}";
        ProgressFill.rectTransform.anchorMax =
            ProgressFill.rectTransform.anchorMax.SetX(Mathf.Clamp01((float) (prog.current / prog.max)));
        QuestTitle.text = currentQuest.DisplayName;
        previous = currentQuest;
    }

    void RemoveListeners(ActiveQuestBase previousQuest) {
        if (previousQuest == null) return;

        if(previousQuest is ActiveWalkingQuest) {
            ActiveWalkingQuest awq = previousQuest as ActiveWalkingQuest;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        } else if(previousQuest is ActiveEnemySlayerQuest) {
            ActiveEnemySlayerQuest awq = previousQuest as ActiveEnemySlayerQuest;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        } else if(previousQuest is ActiveItemCollectQuest) {
            ActiveItemCollectQuest awq = previousQuest as ActiveItemCollectQuest;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        }

        if(previousQuest is ActiveWalkingQuest) {
            ActiveWalkingQuest awq = previousQuest as ActiveWalkingQuest;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        } else if(previousQuest is ActiveQuest<int, IntVariable, UnityEventInt>) {
            ActiveQuest<int, IntVariable, UnityEventInt> awq = previousQuest as ActiveQuest<int, IntVariable, UnityEventInt>;
            awq.AmountSoFar.OnChange.RemoveListener(OnChange);
        } else if(previousQuest is ReachValueQuest) {
            var q = previousQuest as ReachValueQuest;
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
