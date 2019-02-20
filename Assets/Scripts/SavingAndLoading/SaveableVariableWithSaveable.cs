using System.Linq;
using UnityEngine;
using UnityEngine.Events;


namespace CustomEvents {
    public class SaveableVariableWithSaveable<T, TEvent> : Variable<T, TEvent>
        where TEvent : UnityEvent<T>, ISerializationCallbackReceiver, new()
        where T : ScriptableObject, ISaveable {

        public override string GetSaveString() {
            return Value.GetSaveString();
        }

        public override void SetData(string saveString, params object[] otherData) {
            T val = CreateInstance<T>();
            val.SetData(saveString, otherData);
            Value = val;
        }
    }
}
