using System;
using System.Collections.Generic;
using System.Linq;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {

	public class SaveableScriptableObject : ScriptableObject {
		public virtual string GetSaveString() {
			throw new NotImplementedException();
		}

		public virtual void SetData(string saveString, params object[] otherData) {
			throw new NotImplementedException();
		}
	}

	public class RuntimeSetGeneric<T, TEvent> : SaveableScriptableObject where TEvent : UnityEvent<T> {
		public List<T> Value { get { return list; } }
		[ShowInInspector][ReadOnly] private List<T> list = new List<T>();

		public int Count => list.Count;

		public TEvent OnAdd, OnRemove, OnChange;

		public void AddRange(IEnumerable<T> items) {
			list.AddRange(items);
			OnChange.Invoke(default(T));
		}

		public void RemoveAt(int index) {
			list.RemoveAt(index);
			OnChange.Invoke(default(T));
		}

		public void Insert(int index, T item ) {
			list.Insert(index, item);
			OnChange.Invoke(default(T));
		}

		public void Add( T item, bool silently = false) {
			list.Add(item);
			if (!silently) {
				OnAdd.Invoke(item);
				OnChange.Invoke(item);
			}
		}

		public bool Contains(T it) => Value.Contains(it);

		public void Remove( T item ) {
			list.Remove(item);
			OnChange.Invoke(item);
			OnRemove.Invoke(item);
		}

		public void Clear() {
			list.Clear();
			OnChange.Invoke(default(T));
		}

		public static implicit operator List<T>(RuntimeSetGeneric<T, TEvent> val) {
			return val.list;
		}

		public T this[int i] {
			get { return list[i]; }
			set { list[i] = value; }
		}

		private void OnEnable() {
			Clear();
		}
	}

	public abstract class RuntimeSetGenericSaveable<T, TEvent> : RuntimeSetGeneric<T, TEvent> where TEvent : UnityEvent<T> where T : IID {

		[System.Serializable]
		private struct SaveObject {
			public List<int> List;
		}

		public override string GetSaveString() {
			return JsonUtility.ToJson( new SaveObject { List = Value.Select(d => d.GetID()).ToList()});
		}

		public override void SetData(string saveString, params object[] otherData) {
			Clear();

			List<T> listOfAllPossibleObjects = ((PersistentSetGeneric<T, TEvent>) otherData[0]).ToList();
			List<int> ids = JsonUtility.FromJson<SaveObject>(saveString).List;
			foreach (int id in ids) {
				T obj = listOfAllPossibleObjects.FirstOrDefault(o => o.GetID() == id);
				if (obj != null && !obj.Equals(default(T))) {
					Add(obj);
				}
				else {
					Debug.LogWarning("Tried to load an ID that does not exist");
				}
			}
		}
	}
}


