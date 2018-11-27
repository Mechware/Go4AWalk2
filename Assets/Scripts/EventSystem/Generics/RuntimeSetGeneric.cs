using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {
	public class RuntimeSetGeneric<T, TEvent> : ScriptableObject where TEvent : UnityEvent<T> {
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
}


