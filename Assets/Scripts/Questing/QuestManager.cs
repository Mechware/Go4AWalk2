using CustomEvents;
using G4AW2.Dialogue;
using G4AW2.Questing;
using System;
using System.Collections.Generic;
using G4AW2.Data.Area;
using G4AW2.Data.DropSystem;
using G4AW2.Followers;
using G4AW2.UI.Areas;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour {

    public static QuestManager Instance;
    
    public ActiveQuestBase CurrentQuest;
    public Action OnQuestChange;
    
    public Dialogue QuestDialogUI;

    public RuntimeSetQuest CurrentQuests;
    public DragObject DraggableWorld;
    public GameObject ScrollArrow;

    private Area currentArea = null;

    private void Awake() {
        Instance = this;
    }

    public void Initialize() {
        
        currentArea = CurrentQuest.Area;
        
        if(CurrentQuest.ID == 1) {
            SetCurrentQuest(CurrentQuest);
        } else {

            if (CurrentQuest is BossQuest) {
                InteractionController.Instance.StartBossFight();
            }
            CurrentQuest.ResumeQuest(FinishQuest);
            SetQuest(CurrentQuest);

            if(CurrentQuest.IsFinished()) {
                CurrentQuest.CleanUp();
                if (CurrentQuest.NextQuest != null) {
                    Debug.LogWarning("Progressing quest?");
                    AdvanceQuestAfterConversation(CurrentQuest);
                }
            }
        }
    }

    private void FinishQuest(ActiveQuestBase quest) {
        quest.CleanUp();
        StatTracker.Instance.CompleteQuest(quest);
        if(quest.NextQuest != null) CurrentQuest = quest.NextQuest;
        QuestDialogUI.SetConversation(quest.EndConversation, () => DropRewardAndAdvanceConversation(quest));
    }

    public ItemDropBubbleManager ItemDropManager;
    public Inventory Inventory;

    private void DropRewardAndAdvanceConversation(ActiveQuestBase q) {


        List<ItemInstance> todrops = new List<ItemInstance>();
        foreach (var reward in q.QuestRewards) {
            ItemConfig it = reward.it;
            var instance = ItemFactory.GetInstance(it, reward.Level, reward.RandomRoll);
            todrops.Add(instance);
        }

        if (todrops.Count == 0) {
            AdvanceQuestAfterConversation(q);
        }
        else {

            DraggableWorld.Disable2();
            ScrollArrow.SetActive(false);

            
            ItemDropManager.AddItems(todrops, null, () => {
                ScrollArrow.SetActive(true);
                DraggableWorld.Enable2();
                AdvanceQuestAfterConversation(q);

                todrops.ForEach(Inventory.Add);
            });
        }
    }

    private void AdvanceQuestAfterConversation(ActiveQuestBase q) {

        q.CleanUp();

        if(q.NextQuest == null) {
            PopUp.SetPopUp(
                "You finished the quest! You may either continue in this area or switch quests using the quest book on your screen.",
                new[] { "ok" }, new Action[] {
                    () => { }
                });
            return;
        }

        SetCurrentQuest(q.NextQuest);
    }

    public GameObject AreaChangeAndFadeObject;
    public RobustLerperSerialized AreaChangeInterpolater;

    void Update() {
        AreaChangeInterpolater.Update(Time.deltaTime);
    }


    public void SetCurrentQuest(ActiveQuestBase quest) {

        quest.CleanUp();

        if (quest.Area != currentArea) {
            AreaChangeAndFadeObject.SetActive(true);
            AreaChangeInterpolater.StartLerping(() => {


                FollowerManager.Instance.Followers.Clear();
                DeadEnemyController.Instance.ClearEnemies();

                CurrentQuest = quest;
                SetQuest(quest);
                
                quest.StartQuest(FinishQuest);
                
                AreaChangeInterpolater.StartReverseLerp(() => {
                    
                    AreaChangeAndFadeObject.SetActive(false);

                    QuestDialogUI.SetConversation(quest.StartConversation, () => {

                        // Check that the quest isn't finished (for reach quests)
                        if(quest.IsFinished()) FinishQuest(quest);

                        if (quest is BossQuest) {
                            InteractionController.Instance.StartBossFight();
                        }
                        
                    });
                    
                });
            });
        }
        else {
            CurrentQuest = quest;
            OnQuestChange();
            SetQuest(quest);

            quest.StartQuest(FinishQuest);
            
            QuestDialogUI.SetConversation(quest.StartConversation, () => {
                
                if(quest.IsFinished())
                    FinishQuest(quest);
                
                if (quest is BossQuest) {
                    InteractionController.Instance.StartBossFight();
                }
            });
        }

        currentArea = quest.Area;
    }

    public void SetQuest(ActiveQuestBase quest) {
        AreaManager.Instance.SetArea(quest.Area);
        QuestingStatWatcher.Instance.SetQuest(quest);
        MiningPoints.Instance.QuestChanged(quest);
        TutorialManager.Instance.QuestUpdated(quest);
        FollowerManager.Instance.QuestChanged(quest);
    }

    public GameObject Journal;

    public void QuestClicked(Quest q) {
        //TODO: Show some sort of info on the quest.
        if(!(q is ActiveQuestBase)) {
            return;
        }

        PopUp.SetPopUp($"{q.DisplayName}\nWhat would you like to do?", new[] {"Set Active", "Remove", "Cancel"},
            new Action[] {
                () => {
                    // Set Active
                    if (!CurrentQuest.IsFinished()) {
                        if (!(CurrentQuest is ReachValueQuest)) {
                            PopUp.SetPopUp(
                                "Are you sure you want to switch quests? You will lose all progress in this one.",
                                new[] {"Yep", "Nope"}, new Action[] {
                                    () => {
                                        CurrentQuests.Add(CurrentQuest);
                                        CurrentQuest.CleanUp();
                                        CurrentQuests.Remove(q);

                                        SetCurrentQuest((ActiveQuestBase) q);
                                    },
                                    () => { }
                                });
                        }
                        else {
                            CurrentQuests.Add(CurrentQuest);
                            CurrentQuest.CleanUp();
                            CurrentQuests.Remove(q);
                            SetCurrentQuest((ActiveQuestBase) q);
                        }
                    }
                    else {
                        // You've already completed the quest
                        CurrentQuests.Remove(q);
                        SetCurrentQuest((ActiveQuestBase) q);
                    }
                    Journal.SetActive(false);

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

    public ActiveQuestBase TestQuest;

    [ContextMenu("Set Quest from test quest")]
    public void SetQuestFromTestQuest() {
        CurrentQuest.CleanUp();
        SetCurrentQuest(TestQuest);
    }
}
