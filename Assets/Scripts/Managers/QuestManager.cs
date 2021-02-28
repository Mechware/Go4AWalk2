using G4AW2.Data;
using G4AW2.Data.DropSystem;
using System;
using UnityEngine;

namespace G4AW2.Managers
{
    [CreateAssetMenu(menuName = "Managers/Quests")]
    public class QuestManager : ScriptableObject
    {
        public QuestInstance CurrentQuest;

        [SerializeField] private ItemManager _items;
        [SerializeField] private FollowerManager _followers;

        [SerializeField] private QuestConfig StartQuest;

        public Action<QuestInstance> QuestStarted;
        public Action<QuestInstance> QuestFinished;
        public Action<Area> AreaChanged;

        public void Initialize(bool newGame)
        {
            if(newGame)
            {
                CurrentQuest = new QuestInstance(StartQuest, true);
                SetQuest(CurrentQuest);
            } 
            else
            {
                CurrentQuest = new QuestInstance(StartQuest, true);
            }

            _followers.FollowerRemoved += (res) => {
                if (CurrentQuest.Config.QuestType != QuestType.Boss &&
                    CurrentQuest.Config.QuestType != QuestType.EnemySlaying)
                {
                    return;
                }

                var (follower, suicide) = res;
                if (follower.Config != CurrentQuest.Config.QuestParam) return;

                if (!suicide)
                {
                    CurrentQuest.SaveData.Progress += 1;
                }

                if (CurrentQuest.IsFinished())
                {
                    QuestFinished?.Invoke(CurrentQuest);
                }
            };

            _items.OnItemObtained += it => {
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
                        _items.GetAmountOf((ItemConfig)CurrentQuest.Config.QuestParam);
                }
            };
        }



        public void GiveQuest(QuestConfig config)
        {
            QuestInstance qi = new QuestInstance(config, false);
            SaveGame.SaveData.CurrentQuests.Add(qi.SaveData);
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

            CurrentQuest = quest;
            SetQuest(quest);
        }


        public void SetQuest(QuestInstance questConfig)
        {
            QuestStarted?.Invoke(questConfig);
        }
    }

}
