using CustomEvents;
using G4AW2.Data.Combat;
using G4AW2.Questing;
using System;
using UnityEngine;

namespace G4AW2.Data {
    [CreateAssetMenu(menuName = "Data/Quests/EnemySlayer")]
    public class EnemySlayerQuest : ScriptableObject, ISaveable {

        public int ID;
        public EnemyData Enemy;
        public int TotalToKill;
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

        public string GetSaveString() {
            throw new NotImplementedException();
        }

        public void SetData(string saveString, params object[] otherData) {
            throw new NotImplementedException();
        }
    }
}


