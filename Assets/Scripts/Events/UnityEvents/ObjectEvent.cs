using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Events {
	public class ObjectEvent : UnityEvent<object> { }
	[Serializable] public class UnityEventFloat : UnityEvent<float> { }
    [Serializable] public class UnityEventInt : UnityEvent<int> { }
    [Serializable] public class UnityEventString : UnityEvent<string> { }
    [Serializable] public class UnityEventListSO : UnityEvent<List<ScriptableObject>> { }
    [Serializable] public class UnityEventListFollowerData : UnityEvent<List<Data.FollowerData>> { }
    [Serializable] public class UnityEventVector3Arr : UnityEvent<Vector3[]> { }
}