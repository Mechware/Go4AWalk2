using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PersistentSetGeneric<T, TEvent> : ScriptableObject where TEvent : UnityEvent<T> {
	public List<T> Value;

	public TEvent OnAdd, OnRemove, OnChange;

	public void Add(T item) {
		if (!Value.Contains(item)) {
			Value.Add(item);
			OnAdd.Invoke(item);
			OnChange.Invoke(item);
		}
	}

	public void Remove( T item ) {
		if (Value.Contains(item)) {
			Value.Remove(item);
			OnChange.Invoke(item);
			OnRemove.Invoke(item);
		}
	}
}
