using CustomEvents;
using G4AW2.Dialogue;
using G4AW2.Questing;
using System;
using System.Collections.Generic;
using G4AW2.Combat;
using G4AW2.Data.Area;
using G4AW2.Data.DropSystem;
using G4AW2.Followers;
using G4AW2.UI.Areas;
using UnityEngine;
using UnityEngine.Events;

public class QuestManager : MonoBehaviour {

    public static QuestManager Instance;
    
    public QuestInstance CurrentQuest;
    
    public Dialogue QuestDialogUI;

    public DragObject DraggableWorld;
    public GameObject ScrollArrow;

    public GameObject AreaChangeAndFadeObject;
    public RobustLerperSerialized AreaChangeInterpolater;
    
    public GameObject Journal;
    public QuestConfig TestQuestConfig;
    
    private void Awake() {
        Instance = this;
    }

    public void Initialize() {
        
        if(CurrentQuest.Config.Id == 1) {
            SetCurrentQuest(CurrentQuest);
        } else {
            SetQuest(CurrentQuest.Config);
        }

        InteractionController.Instance.OnEnemyDeath += (res) => {
            var (enemy, suicide) = res;
            if (CurrentQuest.Config.QuestType != QuestType.Boss &&
                CurrentQuest.Config.QuestType != QuestType.EnemySlaying) {
                return;
            }

            if (enemy.Config != CurrentQuest.Config.QuestParam) return;
            
            if (!suicide) {
                CurrentQuest.SaveData.Progress += 1;
            }

            if (CurrentQuest.IsFinished()) {
                FinishCurrentQuest();
            }
        };

        Inventory.Instance.OnItemObtained += it => {
            if (CurrentQuest.Config.QuestType != QuestType.ItemCollecting &&
                CurrentQuest.Config.QuestType != QuestType.ItemGiving) {
                return;
            }

            if (it.Config != CurrentQuest.Config.QuestParam) return;

            if (CurrentQuest.Config.QuestType == QuestType.ItemCollecting) {
                CurrentQuest.SaveData.Progress += 1;
            }
            else if (CurrentQuest.Config.QuestType == QuestType.ItemGiving) {
                CurrentQuest.SaveData.Progress =
                    Inventory.Instance.GetAmountOf((ItemConfig) CurrentQuest.Config.QuestParam);
            }
        };
    }
    
    public void MyUpdate() {
        AreaChangeInterpolater.Update(Time.deltaTime);
    }
    
    public void GiveQuest(QuestConfig config) {
        QuestInstance qi = new QuestInstance(config);
        SaveGame.SaveData.CurrentQuests.Add(qi.SaveData);
    }
    
    private void FinishCurrentQuest() {
        var config = CurrentQuest.Config;
        
        QuestDialogUI.SetConversation(config.EndConversation, () => {

            List<ItemInstance> todrops = new List<ItemInstance>();
            foreach (var reward in config.QuestRewards) {
                ItemConfig it = reward.it;
                var instance = ItemFactory.GetInstance(it, reward.Level, reward.RandomRoll);
                todrops.Add(instance);
            }

            if (todrops.Count == 0) {
                _TryAdvanceQuest();
            }
            else {

                DraggableWorld.Disable2();
                ScrollArrow.SetActive(false);
            
                ItemDropBubbleManager.Instance.AddItems(todrops, null, () => {
                    ScrollArrow.SetActive(true);
                    DraggableWorld.Enable2();
                    
                    
                    _TryAdvanceQuest();
                    
                    todrops.ForEach(Inventory.Instance.Add);
                });
            }
        });


        void _TryAdvanceQuest() {

            CurrentQuest.SaveData.Complete = true;

            
            if(config.NextQuestConfig == null) {
                PopUp.SetPopUp(
                    "You finished the quest! You may either continue in this area or switch quests using the quest book on your screen.",
                    new[] { "ok" }, new Action[] {
                        () => { }
                    });
            }
            else {
                SetCurrentQuest(new QuestInstance(CurrentQuest.Config.NextQuestConfig));
            }
        }
    }
    
    public void SetCurrentQuest(QuestInstance quest) {

        if (CurrentQuest.SaveData.Complete) {
            CurrentQuest.SaveData.Active = false;
            SaveGame.SaveData.CompletedQuests.Add(CurrentQuest.SaveData);
            SaveGame.SaveData.CurrentQuests.Remove(CurrentQuest.SaveData);    
        }

        quest.SaveData.Active = true;
        
        if (quest.Config.Area != AreaManager.Instance.Area) {
            AreaChangeAndFadeObject.SetActive(true);
            AreaChangeInterpolater.StartLerping(() => {

                DeadEnemyController.Instance.ClearEnemies();

                CurrentQuest = quest;
                SetQuest(quest.Config);
                
                AreaChangeInterpolater.StartReverseLerp(() => {
                    
                    AreaChangeAndFadeObject.SetActive(false);

                    QuestDialogUI.SetConversation(quest.Config.StartConversation, () => {
                    });
                    
                });
            });
        }
        else {
            CurrentQuest = quest;
            SetQuest(quest.Config);

            QuestDialogUI.SetConversation(quest.Config.StartConversation, () => {
            });
        }
    }

    public void SetQuest(QuestConfig questConfig) {
        AreaManager.Instance.SetArea(questConfig.Area);
        MiningPoints.Instance.SetQuest(questConfig);
        TutorialManager.Instance.SetQuest(questConfig);
    }

    public void QuestClicked(QuestInstance q) {

        PopUp.SetPopUp($"{q.Config.DisplayName}\nWhat would you like to do?", new[] {"Set Active", "Remove", "Cancel"},
            new Action[] {
                () => {
                    // Set Active
                    if (!CurrentQuest.SaveData.Complete) {
                        PopUp.SetPopUp(
                            "Are you sure you want to switch quests? You will lose all progress in this one.",
                            new[] {"Yep", "Nope"}, new Action[] {
                                () => {
                                    SetCurrentQuest(q);
                                },
                                () => { }
                            });
                    }
                    else {
                        // You've already completed the quest
                        SetCurrentQuest(q);
                    }
                    Journal.SetActive(false);

                },
                () => {
                    // Remove
                    SaveGame.SaveData.CurrentQuests.Remove(q.SaveData);
                },
                () => {
                    // Cancel
                }
            });
    }
    
    [ContextMenu("Set Quest from test quest")]
    public void SetQuestFromTestQuest() {
        SetCurrentQuest(new QuestInstance(TestQuestConfig));
    }
}
