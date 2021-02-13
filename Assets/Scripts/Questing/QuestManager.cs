using CustomEvents;
using G4AW2.Dialogue;
using G4AW2.Questing;
using System;
using System.Collections.Generic;
using G4AW2.Data.Area;
using G4AW2.Data.DropSystem;
using G4AW2.UI.Areas;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour {

    public ActiveQuestBaseVariable CurrentQuest;
    
    public Dialogue QuestDialogUI;

    public RuntimeSetQuest CurrentQuests;
    public DragObject DraggableWorld;
    public GameObject ScrollArrow;
    public ItemDropBubbleManager ItemDropManager;
    public Inventory Inventory;

    public Action<ActiveQuestBase> OnQuestSet;

    private Area currentArea = null;

    public void Initialize() {
        
        currentArea = CurrentQuest.Value.Area;
        
        if(CurrentQuest.Value.ID == 1) {
            SetCurrentQuest(CurrentQuest.Value);
        } else {

            if (CurrentQuest.Value is BossQuest bq) {
                InteractionController.Instance.StartBossFight(bq);
            }
            CurrentQuest.Value.ResumeQuest(FinishQuest);
            OnQuestSet?.Invoke(CurrentQuest.Value);

            if(CurrentQuest.Value.IsFinished()) {
                CurrentQuest.Value.CleanUp();
                if (CurrentQuest.Value.NextQuest != null) {
                    Debug.LogWarning("Progressing quest?");
                    AdvanceQuestAfterConversation(CurrentQuest.Value);
                }
            }
        }
    }

    private void FinishQuest(ActiveQuestBase quest) {
        quest.CleanUp();
        StatTracker.Instance.CompleteQuest(quest);
        if(quest.NextQuest != null) CurrentQuest.Value = quest.NextQuest;
        QuestDialogUI.SetConversation(quest.EndConversation, () => DropRewardAndAdvanceConversation(quest));
    }

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
                } else if(it is Headgear) {
                    Headgear a = ScriptableObject.Instantiate(it) as Headgear;
                    a.OnAfterObtained();
                    a.Level = reward.Level;
                    if(reward.RandomRoll != -1) {
                        a.RandomRoll = reward.RandomRoll;
                        a.SetValuesBasedOnRandom();
                    }
                    it = a;
                }
            }
            todrops.Add(it);
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
                
                Inventory.AddItems(todrops);
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


                DataManager.Instance.Followers.Clear();
                DeadEnemyController.Instance.ClearEnemies();

                CurrentQuest.Value = quest;
                OnQuestSet?.Invoke(quest);
                
                quest.StartQuest(FinishQuest);
                
                AreaChangeInterpolater.StartReverseLerp(() => {
                    
                    AreaChangeAndFadeObject.SetActive(false);

                    QuestDialogUI.SetConversation(quest.StartConversation, () => {

                        // Check that the quest isn't finished (for reach quests)
                        if(quest.IsFinished()) FinishQuest(quest);

                        if (quest is BossQuest bq) {
                            InteractionController.Instance.StartBossFight(bq);
                        }
                        
                    });
                    
                });
            });
        }
        else {
            CurrentQuest.Value = quest;
            OnQuestSet?.Invoke(quest);

            quest.StartQuest(FinishQuest);
            
            QuestDialogUI.SetConversation(quest.StartConversation, () => {
                
                if(quest.IsFinished())
                    FinishQuest(quest);
                
                if (quest is BossQuest bq) {
                    InteractionController.Instance.StartBossFight(bq);
                }
            });
        }

        currentArea = quest.Area;
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
                    if (!CurrentQuest.Value.IsFinished()) {
                        if (!(CurrentQuest.Value is ReachValueQuest)) {
                            PopUp.SetPopUp(
                                "Are you sure you want to switch quests? You will lose all progress in this one.",
                                new[] {"Yep", "Nope"}, new Action[] {
                                    () => {
                                        CurrentQuests.Add(CurrentQuest);
                                        CurrentQuest.Value.CleanUp();
                                        CurrentQuests.Remove(q);

                                        SetCurrentQuest((ActiveQuestBase) q);
                                    },
                                    () => { }
                                });
                        }
                        else {
                            CurrentQuests.Add(CurrentQuest);
                            CurrentQuest.Value.CleanUp();
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
        CurrentQuest.Value.CleanUp();
        SetCurrentQuest(TestQuest);
    }
}
