using System.Collections;
using System.Collections.Generic;
using G4AW2.Questing;
using TMPro;
using UnityEngine;

public class QuestingStatWatcher : MonoBehaviour {

    public TextMeshProUGUI MaxText;
    public TextMeshProUGUI CurrentText;

    private ActiveQuestBase previous;

    public void SetQuest(ActiveQuestBase currentQuest) {

        RemoveListeners(previous);

        if (currentQuest is ActiveWalkingQuest) {
            ActiveWalkingQuest awq = currentQuest as ActiveWalkingQuest;
            MaxText.text = "" + awq.AmountToReach;
            CurrentText.text = "" + awq.AmountSoFar.Value;
            awq.AmountSoFar.OnChange.AddListener(OnChange);
        } else if (currentQuest is ActiveEnemySlayerQuest) {
            ActiveEnemySlayerQuest awq = currentQuest as ActiveEnemySlayerQuest;
            MaxText.text = "" + awq.AmountToReach;
            CurrentText.text = "" + awq.AmountSoFar.Value;
            awq.AmountSoFar.OnChange.AddListener(OnChange);
        } else if (currentQuest is ActiveItemCollectQuest) {
            ActiveItemCollectQuest awq = currentQuest as ActiveItemCollectQuest;
            MaxText.text = "" + awq.AmountToReach;
            CurrentText.text = "" + awq.AmountSoFar.Value;
            awq.AmountSoFar.OnChange.AddListener(OnChange);
        } else if (currentQuest is BossQuest) {
            BossQuest awq = currentQuest as BossQuest;
            MaxText.text = "" + awq.AmountToReach;
            CurrentText.text = "" + awq.AmountSoFar.Value;
            awq.AmountSoFar.OnChange.AddListener(OnChange);
        }

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
    }

    void OnChange(float val) {
        CurrentText.text = "" + val;
    }

    void OnChange(int val) {
        CurrentText.text = "" + val;
    }
}
