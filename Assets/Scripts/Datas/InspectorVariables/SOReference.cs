using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace G4AW2.Variables {
	public abstract class SOReference<T> {

		public bool UseConstant = true;
		public T ConstantValue;
		public SOVariable<T> Variable;

		public SOReference() { }

		public SOReference( T value ) {
			UseConstant = true;
			ConstantValue = value;
		}

		public T Value {
			get { return UseConstant ? ConstantValue : Variable.Value; }
		}

		public static implicit operator T( SOReference<T> reference ) {
			return reference.Value;
		}
	}


}

