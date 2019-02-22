using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Questing {
    public class PassiveQuest : Quest, ISaveable {

#if UNITY_EDITOR
        [ContextMenu("Pick ID")]
        public void PickID() {
            ID = IDUtils.PickID<Quest>();
        }
#endif

        public virtual string GetSaveString() {
            throw new System.NotImplementedException();
        }

        public virtual void SetData(string saveString, params object[] otherData) {
            throw new System.NotImplementedException();
        }
    }
}


