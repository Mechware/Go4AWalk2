using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PersistentSetGeneric<T, TEvent> : ScriptableObject where TEvent : UnityEvent<T> {
	[SerializeField] private List<T> list;

	public TEvent OnAdd, OnRemove, OnChange;

	public void Add(T item) {
		if (!list.Contains(item)) {
			list.Add(item);
			OnAdd.Invoke(item);
			OnChange.Invoke(item);
		}
	}

	public void Remove( T item ) {
		if (list.Contains(item)) {
			list.Remove(item);
			OnChange.Invoke(item);
			OnRemove.Invoke(item);
		}
	}

	public List<T> GetList() {
		return list;
	}
}
