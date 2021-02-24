using G4AW2.Data;
using System;
using System.Collections.Generic;
using G4AW2.Data.DropSystem;
using G4AW2.UI.Areas;
using UnityEngine;
using System.Linq;
using G4AW2.Component.UI;

namespace G4AW2.Managers
{
    public class QuestManager : MonoBehaviour
    {

        [Obsolete("Singleton")] public static QuestManager Instance;

        public QuestInstance CurrentQuest;
        
        public Dialogue QuestDialogUI;

        public DragObject DraggableWorld;

        public GameObject AreaChangeAndFadeObject;
        public RobustLerperSerialized AreaChangeInterpolater;

        public GameObject Journal;
        public QuestConfig TestQuestConfig;

        [SerializeField] private InteractionCoordinator _interactions;
        private ItemManager _inventory;

        private void Awake()
        {
            Instance = this;
        }

        private GameEvents _events;

        public void Initialize(GameEvents events, ItemManager inventory)
        {

            _events = events;
            _inventory = inventory;

            CurrentQuest = new QuestInstance(SaveGame.SaveData.CurrentQuests.First(q => q.Active));

            if (CurrentQuest.Config.Id == 1)
            {
                SetCurrentQuest(CurrentQuest);
            }
            else
            {

                if (CurrentQuest.Config.QuestType is QuestType.Boss)
                {
                    _interactions.StartBossFight(CurrentQuest);
                }

                SetQuest(CurrentQuest);
            }

            _interactions.OnEnemyDeath += (res) => {
                var (enemy, suicide) = res;
                if (CurrentQuest.Config.QuestType != QuestType.Boss &&
                    CurrentQuest.Config.QuestType != QuestType.EnemySlaying)
                {
                    return;
                }

                if (enemy.Config != CurrentQuest.Config.QuestParam) return;

                if (!suicide)
                {
                    CurrentQuest.SaveData.Progress += 1;
                }

                if (CurrentQuest.IsFinished())
                {
                    FinishCurrentQuest();
                }
            };

            _inventory.OnItemObtained += it => {
                if (CurrentQuest.Config.QuestType != QuestType.ItemCollecting &&
                    CurrentQuest.Config.QuestType != QuestType.ItemGiving)
                {
                    return;
                }

                if (it.Config != CurrentQuest.Config.QuestParam) return;

                if (CurrentQuest.Config.QuestType == QuestType.ItemCollecting)
                {
                    CurrentQuest.SaveData.Progress += 1;
                }
                else if (CurrentQuest.Config.QuestType == QuestType.ItemGiving)
                {
                    CurrentQuest.SaveData.Progress =
                        _inventory.GetAmountOf((ItemConfig)CurrentQuest.Config.QuestParam);
                }
            };
        }

        void Update()
        {
            AreaChangeInterpolater.Update(Time.deltaTime);
        }

        public void GiveQuest(QuestConfig config)
        {
            QuestInstance qi = new QuestInstance(config, false);
            SaveGame.SaveData.CurrentQuests.Add(qi.SaveData);
        }

        [Obsolete("Don't leave completing quests up to others")]
        public void FinishCurrentQuest()
        {
            var config = CurrentQuest.Config;

            QuestDialogUI.SetConversation(config.EndConversation, () => {

                List<ItemInstance> todrops = new List<ItemInstance>();
                foreach (var reward in config.QuestRewards)
                {
                    ItemConfig it = reward.it;
                    var instance = ItemFactory.GetInstance(it, reward.Level, reward.RandomRoll);
                    todrops.Add(instance);
                }

                if (todrops.Count == 0)
                {
                    _TryAdvanceQuest();
                }
                else
                {

                    DraggableWorld.Disable2();

                    ItemDropBubbleManager.Instance.AddItems(todrops, null, () => {
                        DraggableWorld.Enable2();


                        _TryAdvanceQuest();

                        todrops.ForEach(ItemManager.Instance.Add);
                    });
                }
            });


            void _TryAdvanceQuest()
            {

                CurrentQuest.SaveData.Complete = true;


                if (config.NextQuestConfig == null)
                {
                    PopUp.SetPopUp(
                        "You finished the quest! You may either continue in this area or switch quests using the quest book on your screen.",
                        new[] { "ok" }, new Action[] {
                        () => { }
                        });
                }
                else
                {
                    SetCurrentQuest(new QuestInstance(CurrentQuest.Config.NextQuestConfig, true));
                }
            }
        }

        public void SetCurrentQuest(QuestInstance quest)
        {

            if (CurrentQuest.SaveData.Complete)
            {
                CurrentQuest.SaveData.Active = false;
                SaveGame.SaveData.CompletedQuests.Add(CurrentQuest.SaveData);
                SaveGame.SaveData.CurrentQuests.Remove(CurrentQuest.SaveData);
            }

            quest.SaveData.Active = true;

            if (quest.Config.Area != AreaDisplay.Instance.CurrentArea)
            {
                AreaChangeAndFadeObject.SetActive(true);
                AreaChangeInterpolater.StartLerping(() => {

                    _events.OnAreaChanged(quest.Config.Area);

                    CurrentQuest = quest;
                    SetQuest(quest);

                    AreaChangeInterpolater.StartReverseLerp(() => {

                        AreaChangeAndFadeObject.SetActive(false);

                        QuestDialogUI.SetConversation(quest.Config.StartConversation, () => {

                            if (quest.Config.QuestType == QuestType.Boss)
                            {
                                _interactions.StartBossFight(quest);
                            }
                        });

                    });
                });
            }
            else
            {
                CurrentQuest = quest;
                SetQuest(quest);

                QuestDialogUI.SetConversation(quest.Config.StartConversation, () => {

                    if (quest.Config.QuestType == QuestType.Boss)
                    {
                        _interactions.StartBossFight(quest);
                    }
                });
            }
        }

        public void SetQuest(QuestInstance questConfig)
        {
            _events.OnQuestChanged(questConfig);
        }

        public void QuestClicked(QuestInstance q)
        {

            PopUp.SetPopUp($"{q.Config.DisplayName}\nWhat would you like to do?", new[] { "Set Active", "Remove", "Cancel" },
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
        public void SetQuestFromTestQuest()
        {
            SetCurrentQuest(new QuestInstance(TestQuestConfig, true));
        }
    }

}
