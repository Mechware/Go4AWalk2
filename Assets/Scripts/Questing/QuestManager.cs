using CustomEvents;
using G4AW2.Dialogue;
using G4AW2.Questing;
using System;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour {

    public ActiveQuestBaseVariable CurrentQuest;

    public Dialogue QuestDialogUI;

    public RuntimeSetQuest CurrentQuests;
    public BossQuestController BossController;

    [Header("Events")]
    public UnityEventActiveQuestBase AreaQuestChanged;
    public UnityEvent ResetQuestState;

    public void Initialize() {
        if(CurrentQuest.Value.ID == 1) {
            SetCurrentQuest(CurrentQuest.Value);
        } else {
            CurrentQuest.Value.ResumeQuest(FinishQuest);
            AreaQuestChanged.Invoke(CurrentQuest.Value);

            if(CurrentQuest.Value is BossQuest) {
                BossController.ResumeBossQuest();
            }
        }
    }

    private void FinishQuest(ActiveQuestBase quest) {
        QuestDialogUI.SetConversation(CurrentQuest.Value.EndConversation, () => DropRewardAndAdvanceConversation(quest));
    }

    public ItemDropBubbleManager ItemDropManager;

    private void DropRewardAndAdvanceConversation(ActiveQuestBase q) {

        List<Item> todrops = new List<Item>();
        foreach (var reward in q.QuestRewards) {
            Item it = reward.it;
            if(it.ShouldCreateNewInstanceWhenPlayerObtained()) {
                if(it is Weapon) {
                    Weapon w = ScriptableObject.Instantiate(it) as Weapon;
                    w.OnAfterObtained();
                    w.Level = reward.Level;
                    if(reward.RandomRoll != -1) {
                        w.Random = reward.RandomRoll;
                        w.SetValuesBasedOnRandom();
                    }
                    it = w;
                } else if(it is Armor) {
                    Armor a = ScriptableObject.Instantiate(it) as Armor;
                    a.OnAfterObtained();
                    a.Level = reward.Level;
                    if(reward.RandomRoll != -1) {
                        a.Random = reward.RandomRoll;
                        a.SetValuesBasedOnRandom();
                    }
                    it = a;
                }
            }
            todrops.Add(it);
        }

        if (todrops.Count == 0) {
            AdvanceQuestAfterConversation();
        }
        else {
            ItemDropManager.AddItems(todrops, AdvanceQuestAfterConversation);
        }
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

        ItemDropManager.Clear();

        CurrentQuest.Value = quest;
        AreaQuestChanged.Invoke(quest);
        quest.StartQuest(FinishQuest);
        QuestDialogUI.SetConversation(quest.StartConversation, () => {

            if (quest is BossQuest) {
                BossController.StartBossQuest();
            }
        });
    }

    public void QuestClicked(Quest q) {
        //TODO: Show some sort of info on the quest.
        if(!(q is ActiveQuestBase)) {
            return;
        }

        PopUp.SetPopUp($"{q.DisplayName}\nWhat would you like to do?", new[] {"Set Active", "Remove", "Cancel"},
            new Action[] {
                () => {
                    // Set Active
                    if (!CurrentQuest.Value.IsFinished()) {
                        PopUp.SetPopUp(
                            "Are you sure you want to switch quests? You will lose all progress in this one.",
                            new[] {"Yep", "Nope"}, new Action[] {
                                () => {
                                    ResetQuestState.Invoke();
                                    CurrentQuests.Add(CurrentQuest);
                                    CurrentQuests.Remove(q);
                                    SetCurrentQuest((ActiveQuestBase) q);
                                },
                                () => { }
                            });
                    }
                    else {
                        // You've already completed the quest
                        ResetQuestState.Invoke();
                        SetCurrentQuest((ActiveQuestBase) q);
                    }
                },
                () => {
                    // Remove
                    CurrentQuests.Remove(q);
                },
                () => {
                    // Cancel

                }
            });
    }


}
