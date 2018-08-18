using System.Collections;
using System.Collections.Generic;
using G4AW2.Data;
using G4AW2.Data.Combat;
using UnityEngine;

namespace G4AW2.Followers {

	public class FollowerDisplay : MonoBehaviour {
		private FollowerData Data;

		public void SetData(FollowerData data) {
			Data = data;
			print("Setting data");
			// Do things.
		}
	}

}

