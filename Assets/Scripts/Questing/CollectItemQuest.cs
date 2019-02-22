using CustomEvents;
using G4AW2.Data.Combat;
using G4AW2.Questing;
using System;
using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Data {
    [CreateAssetMenu(menuName = "Data/Quests/EnemySlayer")]
    public class CollectItemQuest : ScriptableObject, IID {

        public int ID;
        public string DisplayName;
        public Item ItemToCollect;
        public int TotalToCollect;
        public IntVariable CollectedCount;
        public Action OnComplete;

        public void StartQuest(Action onComplete) {
            OnComplete = onComplete;
            CollectedCount.OnChange.AddListener(CountChange);
        }

        private void CountChange(int killed) {
            if(killed > TotalToCollect) {
                FinishQuest();
            }
        }

        public void FinishQuest() {
            OnComplete?.Invoke();
            CollectedCount.OnChange.RemoveListener(CountChange);
        }

#if UNITY_EDITOR
        [ContextMenu("Pick ID")]
        public void PickID() {
            ID = IDUtils.PickID<Quest>();
        }

        void OnEnable() {
            if(ID == 0)
                PickID();
        }
#endif
        public int GetID() {
            return ID;
        }
    }
}


