using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.Area;
using G4AW2.Followers;
using G4AW2.Questing;
using UnityEngine;
using UnityEngine.UI;

namespace G4AW2.UI.Areas {
	public class AreaManager : MonoBehaviour {

		public static AreaManager Instance;
		
		public Area Area;

		public ScrollingImages backgrounds;
		public ScrollingImages clouds1;
		public ScrollingImages clouds2;
		public Image[] SkyImages;

		private void Awake() {
			Instance = this;
		}

		public void SetArea( Area area ) {
			Area = area;

            backgrounds.Images.ForEach(im => im.sprite = area.Background);
			clouds1.Images.ForEach(i => i.sprite = area.Clouds1);
			clouds2.Images.ForEach(i => i.sprite = area.Clouds2);
			SkyImages.ForEach(i => i.sprite = area.Sky);

		}

	    [ContextMenu("Set Area")]
	    public void SetArea() {
	        SetArea(Area);
	    }
	}

}

