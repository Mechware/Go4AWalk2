using System.Collections;
using System.Collections.Generic;
using G4AW2.Data.Combat;
using G4AW2.Followers;
using UnityEngine;
using UnityEngine.UI;

namespace G4AW2.Data.Area {
	[CreateAssetMenu(menuName = "Area")]
	public class Area : ScriptableObject {
		public FollowerDropData Enemies;
		public Sprite Background;
		public Sprite SkyUpper;
		public Sprite Sky;
		public Sprite Clouds1;
		public Sprite Clouds2;
	}
}
