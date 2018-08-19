using System;
using UnityEngine.Events;

namespace G4AW2.Variables {
    [Serializable]
	public abstract class SOReference<T, TVar, TEvent> where TEvent : UnityEvent<T>,new() where TVar : SOVariable<T, TEvent> {

		public bool UseConstant = true;
		public T ConstantValue;
		public TVar Variable;

		public SOReference() { }

		public SOReference( T value ) {
			UseConstant = true;
			ConstantValue = value;
		}

		public T Value {
			get { return UseConstant ? ConstantValue : Variable.Value; }
		}
	}


}

