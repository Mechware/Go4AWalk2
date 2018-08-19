using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Variables {

    public class FloatChangeEvent : UnityEvent<float> { }

    [CreateAssetMenu(menuName = "Variable/Float")]
	public class FloatVariable : SOVariable<float, FloatChangeEvent> {

        
	}
}

