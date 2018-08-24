using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.Combat;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace G4AW2.Followers {

	public class FollowerDisplay : MonoBehaviour, IPointerClickHandler {
		private FollowerData Data;

		public UnityEventFollowerData FollowerClicked;

		public void SetData(FollowerData data) {
			Data = data;
			print("Setting data");
            print(Data.name);
			// Do things.
		}

		public void OnPointerClick(PointerEventData eventData) {
			FollowerClicked.Invoke(Data);
		}
	}

}

