using G4AW2.Data;
using UnityEngine;

namespace G4AW2.Managers
{
    [CreateAssetMenu(menuName = "Managers/TutorialManager")]
    public class TutorialManager : ScriptableObject
    {
        public QuestConfig BlockingAndParryingTutorialStart;
        public QuestConfig BlockingAndParryingTutorialEnd;
        [SerializeField] private QuestManager _quests;

        private void OnEnable()
        {
            _quests.QuestStarted += q => SetQuest(q.Config);
        }

        private void OnDisable()
        {
            _quests.QuestStarted -= q => SetQuest(q.Config);
        }

        public void SetQuest(QuestConfig questConfig)
        {
            if (questConfig == BlockingAndParryingTutorialStart)
            {
                GlobalSaveData.SaveData.ShowParryAndBlockColors = true;
            }
            if (questConfig == BlockingAndParryingTutorialEnd)
            {
                GlobalSaveData.SaveData.ShowParryAndBlockColors = false;
            }
        }
    }

}
