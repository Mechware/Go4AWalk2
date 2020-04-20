using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data.Area;
using G4AW2.Followers;
using G4AW2.Questing;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace G4AW2.UI.Areas {
	public class AreaManager : MonoBehaviour {

		public static AreaManager Instance;
		
		[FormerlySerializedAs("Area")] public Background Background;

		public ScrollingImages backgrounds;
		public ScrollingImages clouds1;
		public ScrollingImages clouds2;
		public Image[] SkyImages;

		private void Awake() {
			Instance = this;
		}

		public void SetArea( Background background ) {
			Background = background;

            backgrounds.Images.ForEach(im => im.sprite = background.Sprite);
			clouds1.Images.ForEach(i => i.sprite = background.Clouds1);
			clouds2.Images.ForEach(i => i.sprite = background.Clouds2);
			SkyImages.ForEach(i => i.sprite = background.Sky);

		}

	    [ContextMenu("Set Area")]
	    public void SetArea() {
	        SetArea(Background);
	    }
	}

}

