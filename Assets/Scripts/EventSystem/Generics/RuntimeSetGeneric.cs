using System;
using System.Collections.Generic;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {
	public class RuntimeSetGeneric<T, TEvent> : ScriptableObject where TEvent : UnityEvent<T> {
		public List<T> Value { get { return list; } }
		[ShowInInspector][ReadOnly] private List<T> list = new List<T>();

		public TEvent OnAdd, OnRemove, OnChange;

		public void Add( T item ) {
			list.Add(item);
			OnAdd.Invoke(item);
			OnChange.Invoke(item);
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
	}
}


