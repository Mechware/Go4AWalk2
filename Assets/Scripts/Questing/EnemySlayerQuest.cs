using CustomEvents;
using G4AW2.Data.Combat;
using G4AW2.Questing;
using System;
using System.Linq;
using UnityEngine;

namespace G4AW2.Data {
    [CreateAssetMenu(menuName = "Data/Quests/EnemySlayer")]
    public class EnemySlayerQuest : PassiveQuest {
       
        public EnemyData Enemy;
        public int TotalToKill;
        public int StartAmount = -1;
        public IntVariable KilledCount;
        public Action OnComplete;

        public void StartQuest(Action onComplete) {
            OnComplete = onComplete;
            KilledCount.OnChange.AddListener(EnemyKillCountChange);
        }

        private void EnemyKillCountChange(int killed) {
            if(killed > TotalToKill) {
                FinishQuest();
            }
        }

        public void FinishQuest() {
            OnComplete?.Invoke();
            KilledCount.OnChange.RemoveListener(EnemyKillCountChange);
            Debug.Log("Killed " + TotalToKill + " " + Enemy.name + "s.");
        }

        [Serializable]
        private class DummySave {
            public int ID;
            public int StartAmount;
        }

        public override string GetSaveString() {
            return JsonUtility.ToJson(new DummySave() {ID = ID, StartAmount = StartAmount});
        }

        public override void SetData(string saveString, params object[] otherData) {

            PersistentSetPassiveQuest quests = otherData[0] as PersistentSetPassiveQuest;
            DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

            EnemySlayerQuest original = quests.First(q => q.ID == ds.ID) as EnemySlayerQuest;

            ID = original.ID;
            Enemy = original.Enemy;
            TotalToKill = original.TotalToKill;
            StartAmount = original.StartAmount;
            KilledCount = original.KilledCount;
        }
    }
}


