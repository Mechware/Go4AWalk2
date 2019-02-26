using CustomEvents;
using G4AW2.Dialogue;
using G4AW2.Questing;
using System;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour {

    public ActiveQuestBaseVariable CurrentQuest;

    public Dialogue QuestDialogUI;

    public RuntimeSetQuest CurrentQuests;

    [Header("Events")]
    public UnityEventActiveQuestBase AreaQuestChanged;
    public UnityEvent ResetQuestState;

    private bool receivedEndPopUp = false;

    public void Initialize() {
        if(CurrentQuest.Value.ID == 1 && ((ActiveWalkingQuest) CurrentQuest.Value).AmountSoFar < 1f) {
            SetCurrentQuest(CurrentQuest.Value);
        } else {
            CurrentQuest.Value.ResumeQuest(FinishQuest);
        }
    }

    private void FinishQuest(ActiveQuestBase quest) {
        if(receivedEndPopUp) {
            return;
        }

        QuestDialogUI.SetConversation(CurrentQuest.Value.EndConversation, AdvanceQuestAfterConversation);
    }

    private void AdvanceQuestAfterConversation() {

        if(CurrentQuest.Value.NextQuest == null) {
            PopUp.SetPopUp(
                "You finished the quest! You may either continue in this area or switch quests using the quest book on your screen.",
                new[] { "ok" }, new Action[] {
                    () => { }
                });
            return;
        }

        ResetQuestState.Invoke();
        SetCurrentQuest(CurrentQuest.Value.NextQuest);
    }

    public void SetCurrentQuest(ActiveQuestBase quest) {

        receivedEndPopUp = false;
        CurrentQuest.Value = quest;
        AreaQuestChanged.Invoke(quest);
        quest.StartQuest(FinishQuest);
        QuestDialogUI.SetConversation(quest.StartConversation, () => { });
    }

    public void QuestClicked(Quest q) {

        if(q == CurrentQuest) {
            PopUp.SetPopUp("This is your current quest.", new[] { "Cool", "Nice." }, new Action[] { () => { }, () => { } });
            return;
        }

        //TODO: Show some sort of info on the quest.

        if(!(q is ActiveQuestBase)) {
            return;
        }

        if(!CurrentQuest.Value.IsFinished()) {
            PopUp.SetPopUp("Are you sure you want to switch quests? You will lose all progress in this one.",
                new[] { "Yep", "Nope" }, new Action[] {
                    () => {
                        ResetQuestState.Invoke();
                        SetCurrentQuest((ActiveQuestBase)q);
                    },
                    () => { }
                });
        } else {
            // You've already completed the quest
            CurrentQuests.Remove(CurrentQuest);
            ResetQuestState.Invoke();
            SetCurrentQuest((ActiveQuestBase) q);
        }
    }


}
