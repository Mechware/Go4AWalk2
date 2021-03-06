using G4AW2.Data;
using G4AW2.Data.DropSystem;
using G4AW2.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace G4AW2.Managers
{
    [CreateAssetMenu(menuName = "Managers/Quests")]
    public class QuestManager : ScriptableObject
    {
        [SerializeField] private ItemManager _items;
        [SerializeField] private FollowerManager _followers;
        [SerializeField] private SaveManager _saveManager;

        [SerializeField] private QuestConfig StartQuest;

        public QuestInstance _currentQuest { private set; get; }
        public List<QuestInstance> _currentQuests { private set; get; } = new List<QuestInstance>();
        public List<QuestInstance> _completedQuests { private set; get; } = new List<QuestInstance>();

        public Action<QuestInstance> QuestStarted;
        public Action<QuestInstance> QuestFinished;
        public Action<Area> AreaChanged;

        public List<QuestConfig> AllQuests;
        [ContextMenu("Add all Quests in project")]
        private void SearchForAllItems()
        {
            EditorUtils.AddAllOfType(AllQuests);
        }

        private void OnEnable()
        {
            _saveManager.RegisterLoadFunction("QuestManager", Load);
            _saveManager.RegisterSaveFunction("QuestManager", Save);

            _followers.FollowerRemoved += (follower) => {
                if (_currentQuest.Config.QuestType != QuestType.Boss &&
                    _currentQuest.Config.QuestType != QuestType.EnemySlaying)
                {
                    return;
                }

                if (follower.Config != _currentQuest.Config.QuestParam) return;

                var enemy = (EnemyInstance)follower;

                if (!enemy.Suicide)
                {
                    _currentQuest.SaveData.Progress += 1;
                }

                if (_currentQuest.IsFinished())
                {
                    QuestFinished?.Invoke(_currentQuest);
                }
            };

            _items.OnItemObtained += it => {
                if (_currentQuest.Config.QuestType != QuestType.ItemCollecting &&
                    _currentQuest.Config.QuestType != QuestType.ItemGiving)
                {
                    return;
                }

                if (it.Config != _currentQuest.Config.QuestParam) return;

                if (_currentQuest.Config.QuestType == QuestType.ItemCollecting)
                {
                    _currentQuest.SaveData.Progress += 1;
                }
                else if (_currentQuest.Config.QuestType == QuestType.ItemGiving)
                {
                    _currentQuest.SaveData.Progress =
                        _items.GetAmountOf((ItemConfig)_currentQuest.Config.QuestParam);
                }
            };
        }

        public void Initialize()
        {
            if(_currentQuest == null)
            {
                _currentQuest = new QuestInstance(StartQuest, true);
                SetQuest(_currentQuest);
            }
        }

        [Serializable]
        private class SaveData
        {
            public QuestSaveData CurrentQuest;
            public List<QuestSaveData> CurrentQuests;
            public List<QuestSaveData> CompletedQuests;
        }

        public object Save()
        {
            return new SaveData()
            {
                CurrentQuest = _currentQuest.SaveData,
                CurrentQuests = _currentQuests.Select(q => q.SaveData).ToList(),
                CompletedQuests = _completedQuests.Select(q => q.SaveData).ToList()
            };
        }

        private void Load(object o)
        {
            if (o == null) return;

            var data = (SaveData)o;
            _currentQuest = new QuestInstance(AllQuests.First(q => q.name == data.CurrentQuest.Id), data.CurrentQuest);
            _currentQuests = data.CurrentQuests.Select(q => new QuestInstance(AllQuests.First(config => q.Id == config.name), q)).ToList();
            _completedQuests = data.CompletedQuests.Select(q => new QuestInstance(AllQuests.First(config => q.Id == config.name), q)).ToList();
        }

        public void GiveQuest(QuestConfig config)
        {
            QuestInstance qi = new QuestInstance(config, false);
            _currentQuests.Add(qi);
        }


        public void SetCurrentQuest(QuestInstance quest)
        {
            if (_currentQuest.SaveData.Complete)
            {
                _currentQuest.SaveData.Active = false;
                _completedQuests.Add(_currentQuest);
                _currentQuests.Remove(_currentQuest);
            }

            quest.SaveData.Active = true;

            _currentQuest = quest;
            SetQuest(quest);
        }


        public void SetQuest(QuestInstance questConfig)
        {
            QuestStarted?.Invoke(questConfig);
        }
    }

}
