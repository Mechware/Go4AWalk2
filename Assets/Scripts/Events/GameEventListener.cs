using System;
using G4AW2.Events;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Events {

	public class GameEventListener : MonoBehaviour {

		public GameEvent Event;
		public UnityEvent Response;

		private void OnEnable() {
			Event.RegisterListener(this);
		}

		private void OnDisable() {
			Event.UnregisterListener(this);
		}

		public void OnEventRaised() {
			Response.Invoke();
		}
	}
}
