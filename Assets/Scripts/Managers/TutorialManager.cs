using G4AW2.Data;
using UnityEngine;

namespace G4AW2.Managers
{
    public class TutorialManager : MonoBehaviour
    {
        public QuestConfig BlockingAndParryingTutorialStart;
        public QuestConfig BlockingAndParryingTutorialEnd;
        [SerializeField] private QuestManager _quests;

        private void Awake()
        {
            _quests.QuestStarted += q => SetQuest(q.Config);
        }

        private void OnDestroy()
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
