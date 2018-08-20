using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.Events;

namespace G4AW2.Variables {
    public abstract class ListReference<T, TVar, TEvent> : SOReference<List<T>, TVar, TEvent>
        where TEvent : UnityEvent<List<T>>, new()
        where TVar : SOVariable<List<T>, TEvent> { }
    public abstract class ListVariable<T, TEvent> : SOVariable<List<T>, TEvent> 
        where TEvent : UnityEvent<List<T>>, new() {
    }
}
