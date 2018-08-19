using System;
using System.Collections;
using System.Collections.Generic;
using G4AW2.Variables;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Variables {
    [Serializable]
    public abstract class ListReference<T, TVar, TEvent> : SOReference<List<T>, TVar, TEvent> where TEvent : UnityEvent<List<T>>, new() where TVar : SOVariable<List<T>, TEvent> { }

}

