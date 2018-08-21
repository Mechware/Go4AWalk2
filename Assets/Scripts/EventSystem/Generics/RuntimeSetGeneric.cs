using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace CustomEvents {
	public class RuntimeSetGeneric<T, TEvent> : ScriptableObject where TEvent : UnityEvent<T> {
		private List<T> List;

		public TEvent OnAdd, OnRemove, OnChange;

		public void Add( T item ) {
			if (!List.Contains(item)) {
				List.Add(item);
				OnAdd.Invoke(item);
				OnChange.Invoke(item);
			}
		}

		public void Remove( T item ) {
			if (List.Contains(item)) {
				List.Remove(item);
				OnChange.Invoke(item);
				OnRemove.Invoke(item);
			}
		}
	}
}


