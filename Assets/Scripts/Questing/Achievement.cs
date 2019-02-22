using CustomEvents;
using G4AW2.Data.Combat;
using G4AW2.Questing;
using System;
using UnityEngine;

namespace G4AW2.Data {
    [CreateAssetMenu(menuName = "Data/Quests/EnemySlayer")]
    public class Achievement : ScriptableObject {

        public int ID;
        public string AchievementCompletedText;
        public int NumberToReach;
        public IntVariable Number;
        public Action OnComplete;

        private void OnEnable() {
            Number.OnChange.AddListener(CountChange);
        }

        private void CountChange(int killed) {
            if(killed > NumberToReach) {
                FinishQuest();
            }
        }

        public void FinishQuest() {
            OnComplete?.Invoke();
            Number.OnChange.RemoveListener(CountChange);
            Debug.Log(AchievementCompletedText);
        }
    }
}


