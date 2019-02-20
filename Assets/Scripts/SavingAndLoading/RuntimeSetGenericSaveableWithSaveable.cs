using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {
    public abstract class RuntimeSetGenericSaveableWithSaveable<T, TEvent> : RuntimeSetGeneric<T, TEvent>
        where TEvent : UnityEvent<T>
        where T : ScriptableObject, ISaveable {

        [System.Serializable]
        private struct SaveObject {
            public List<SaveObject2> List;
        }

        [System.Serializable]
        private class SaveObject2 {
            public string Data;
        }

        public override string GetSaveString() {
            SaveObject so = new SaveObject();
            so.List = new List<SaveObject2>();

            foreach(T val in Value) {
                SaveObject2 so2 = new SaveObject2();
                so2.Data = val.GetSaveString();
                so.List.Add(so2);
            }

            return JsonUtility.ToJson(so);
        }

        public override void SetData(string saveString, params object[] otherData) {
            Clear();
            List<SaveObject2> loadedStrings = JsonUtility.FromJson<SaveObject>(saveString).List;

            foreach(SaveObject2 loadedObject in loadedStrings) {
                T newItem = CreateInstance<T>();
                newItem.SetData(loadedObject.Data, otherData[0]);
                Add(newItem);
            }
        }
    }

}
