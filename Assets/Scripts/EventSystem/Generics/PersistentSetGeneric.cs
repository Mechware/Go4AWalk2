using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PersistentSetGeneric<T, TEvent> : ScriptableObject where TEvent : UnityEvent<T> {
	public List<T> List;

	public TEvent OnAdd, OnRemove, OnChange;

	public void Add(T item) {
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