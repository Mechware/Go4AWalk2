using CustomEvents;
using G4AW2.Data.Combat;
using G4AW2.Questing;
using System;
using System.Linq;
using G4AW2.Data.DropSystem;
using UnityEngine;

namespace G4AW2.Data {
    [CreateAssetMenu(menuName = "Data/Quests/CollectItem")]
    public class CollectItemQuest : PassiveQuest {

        public Item Item;
        public int AmountToCollect;
        public int StartAmount = -1;
        public IntVariable TotalCollected;
        public Action OnComplete;

        public void StartQuest(Action onComplete) {
            OnComplete = onComplete;
            TotalCollected.OnChange.AddListener(CountChanged);
        }

        private void CountChanged(int collected) {
            if(collected > AmountToCollect + StartAmount) {
                FinishQuest();
            }
        }

        public void FinishQuest() {
            OnComplete?.Invoke();
            TotalCollected.OnChange.RemoveListener(CountChanged);
            Debug.Log("Collected " + AmountToCollect + " " + Item.name + "s.");
        }

        [Serializable]
        private class DummySave {
            public int ID;
            public int StartAmount;
        }

        public override string GetSaveString() {
            return JsonUtility.ToJson(new DummySave() { ID = ID, StartAmount = StartAmount });
        }

        public override void SetData(string saveString, params object[] otherData) {

            PersistentSetPassiveQuest quests = otherData[0] as PersistentSetPassiveQuest;
            DummySave ds = JsonUtility.FromJson<DummySave>(saveString);

            CollectItemQuest original = quests.First(q => q.ID == ds.ID) as CollectItemQuest;

            ID = original.ID;
            Item = original.Item;
            AmountToCollect = original.AmountToCollect;
            StartAmount = original.StartAmount;
            TotalCollected = original.TotalCollected;
        }
    }
}


