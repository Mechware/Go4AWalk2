using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Questing {
    public class PassiveQuest : Quest, ISaveable {

#if UNITY_EDITOR
        [ContextMenu("Pick ID")]
        public new void PickID() {
            ID = IDUtils.PickID<Quest>();
        }
#endif

        public Action<PassiveQuest> OnComplete;

        public virtual void StartQuest(Action<PassiveQuest> onComplete) {
            OnComplete = onComplete;
        }

        public virtual void ResumeQuest(Action<PassiveQuest> onComplete) {
            OnComplete = onComplete;
        }

        public virtual void FinishQuest() {
            OnComplete?.Invoke(this);
        }

        public virtual string GetSaveString() {
            throw new System.NotImplementedException();
        }

        public virtual void SetData(string saveString, params object[] otherData) {
            throw new System.NotImplementedException();
        }
    }
}


