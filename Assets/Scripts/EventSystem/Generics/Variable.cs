using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using Sirenix.Utilities;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {

    [Serializable]
	public abstract class Variable<T, TEvent> : SaveableScriptableObject where TEvent : UnityEvent<T>, ISerializationCallbackReceiver, new() {
		[Multiline]
		public string DeveloperDescription = "";
	    public T InitialValue;
	    public TEvent OnChange = new TEvent();

		[ReadOnly] [SerializeField] private T _value;

		public T Value {
            get { return _value; }
            set {
                _value = value;
                OnChange.Invoke(Value);
            }
        }

        public static implicit operator T( Variable<T, TEvent> val) {
            return val.Value;
        }

	    public void OnEnable() {
		    _value = InitialValue;
		}

		public void OnAfterDeserialization() {
			Debug.Log("After Deserialize");
			_value = InitialValue;
		}

	    public void OnBeforeSerialization() { }

        public override string GetSaveString() {
            var ots = new SaveObject();
            ots.ObjectToSave = Value;
            return JsonUtility.ToJson(ots);
        }

        public override void SetData( string saveString, params object[] otherData ) {
            var ots = JsonUtility.FromJson<SaveObject>(saveString);
            Value = ots.ObjectToSave;
        }

        private class SaveObject {
            public T ObjectToSave;
        }
    }

    public class SaveableVariableWithIID<T, TEvent> : Variable<T, TEvent> 
        where TEvent : UnityEvent<T>, ISerializationCallbackReceiver, new() 
        where T : IID {

        public override string GetSaveString() {
            SaveObject2 so2 = new SaveObject2();
            if(Value != null && !Value.Equals(default(T))) {
                so2.id = Value.GetID();
            } else {
                so2.id = -1;
            }
            return JsonUtility.ToJson(so2);
        }

        public override void SetData( string saveString, params object[] otherData ) {
            SaveObject2 so2 = JsonUtility.FromJson<SaveObject2>(saveString);

            if (so2.id == -1)
                return;

            PersistentSetGeneric < T, TEvent > allitems = (PersistentSetGeneric<T, TEvent>)otherData[0];
            Value = allitems.ToList().First(item => item.GetID() == so2.id);
        }

        private class SaveObject2 { public int id; }
    }
}
