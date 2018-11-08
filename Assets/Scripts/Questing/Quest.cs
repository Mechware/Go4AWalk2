using UnityEngine;

namespace G4AW2.Questing {
	public class Quest : ScriptableObject {

		public string DisplayName;
		public bool Completed;
		public bool Active;


		public virtual void Start() {

		}

		public virtual void Clicked() {
			
		}

		public virtual void GPSUpdate(float distance) {
			
		}
	}
}


