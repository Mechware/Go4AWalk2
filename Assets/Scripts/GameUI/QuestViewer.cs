using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Dialogue;
using G4AW2.Questing;
using TMPro;
using UnityEngine;

public class QuestViewer : MonoBehaviour {

    public TextMeshProUGUI Title;

    public Dialogue StartQuestDialogueBox;

    void Awake() {
    }


    public void Update() {
        Title.text = QuestManager.Instance.CurrentQuest.Config.DisplayName;
    }

    public void ViewBeginningText() {
        StartQuestDialogueBox.SetConversation(QuestManager.Instance.CurrentQuest.Config.StartConversation, () => { });
    }
}
