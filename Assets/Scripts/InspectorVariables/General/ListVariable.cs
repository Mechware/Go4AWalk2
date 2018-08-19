using System.Collections;
using System.Collections.Generic;
using G4AW2.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Variables {
	public abstract class ListVariable<T, TEvent> : SOVariable<List<T>, TEvent> where TEvent : UnityEvent<List<T>>, new() {
	}
}

