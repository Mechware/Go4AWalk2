using UnityEngine;

namespace G4AW2.Questing {
    public class Quest : ScriptableObject, IID {

        public int ID;
        public string DisplayName;

#if UNITY_EDITOR
        [ContextMenu("Pick ID")]
        public void PickID() {
            ID = IDUtils.PickID<Quest>();
        }
#endif
        public int GetID() {
            return ID;
        }
    }
}


