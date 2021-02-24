using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data;
using TMPro;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class QuestingStatWatcher : MonoBehaviour {

    [SerializeField] private TextMeshProUGUI QuestTitle;
    [SerializeField] private TextMeshProUGUI ProgressText;
    [SerializeField] private Image ProgressFill;
    [SerializeField] private SmoothPopUpManager PopUps;
    [SerializeField] private Transform SpawnPointOfNumberIncreasePopUp;

    private int prevVal = -1;
    private QuestInstance _quest;

    public void SetQuest(QuestInstance q)
    {
        _quest = q;
    }

    public void GameUpdate(float time)
    {
        int current = _quest.SaveData.Progress;
        int max = _quest.Config.ValueToReach;

        ProgressText.text = $"{current} / {max}";
        ProgressFill.rectTransform.anchorMax =
            ProgressFill.rectTransform.anchorMax.SetX(Mathf.Clamp01((float)(current / max)));
        QuestTitle.text = _quest.Config.DisplayName;

        if (prevVal != -1 && current > prevVal)
        {
            SmoothPopUpManager.ShowPopUp(SpawnPointOfNumberIncreasePopUp.position, "+" + (current - prevVal), Color.green, true);
            prevVal = current;
        }
    }
}
