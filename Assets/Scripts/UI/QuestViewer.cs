using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using TMPro;
using UnityEngine;

public class QuestViewer : MonoBehaviour {

    public TextMeshProUGUI Title;

    public ActiveQuestBaseVariable Quest;

    void Awake() {
        Refresh();
        Quest.OnChange.AddListener((e) => { Refresh(); });
    }


    public void Refresh() {
        Title.text = Quest.Value.DisplayName;
    }
}
