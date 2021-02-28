using G4AW2.Component.UI;
using G4AW2.Managers;
using TMPro;
using UnityEngine;

public class QuestViewer : MonoBehaviour {

    public TextMeshProUGUI Title;

    public Dialogue StartQuestDialogueBox;

    [SerializeField] private QuestManager _quests;

    void Awake() {
    }


    public void Update() {
        Title.text = _quests.CurrentQuest.Config.DisplayName;
    }

    public void ViewBeginningText() {
        StartQuestDialogueBox.SetConversation(_quests.CurrentQuest.Config.StartConversation, () => { });
    }
}
