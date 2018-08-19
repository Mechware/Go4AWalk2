using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Variables {

    public class ListSOChangedEvent : UnityEvent<List<ScriptableObject>> { }

    [CreateAssetMenu(menuName = "Variable/ScriptableObjectList")]
	public class ListSOVariable : ListVariable<ScriptableObject, ListSOChangedEvent> {
	    
	}
}

