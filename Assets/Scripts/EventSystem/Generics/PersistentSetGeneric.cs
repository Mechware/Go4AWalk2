using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

public class PersistentSetGeneric<T, TEvent> : ScriptableObject where TEvent : UnityEvent<T> {
	[SerializeField] private List<T> Value;

	public TEvent OnAdd, OnRemove, OnChange;

	public int Count => Value.Count;

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

	public void ForEach(Action<T> func) {
		Value.ForEach(func);
	}

	public void AddRange(IEnumerable<T> range) {
		Value.AddRange(range);
	}

	public void Clear() {
		Value.Clear();
	}

	public T[] ToArray() {
		return Value.ToArray();
	}

	public List<T> ToList() {
		return Value.ToList();
	}
}
