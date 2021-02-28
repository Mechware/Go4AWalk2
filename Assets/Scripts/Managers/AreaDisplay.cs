using System;
using G4AW2.Data;
using G4AW2.Managers;
using UnityEngine;
using UnityEngine.UI;

namespace G4AW2.UI.Areas
{
    public class AreaDisplay : MonoBehaviour {

		[Obsolete("Singleton")] public static AreaDisplay Instance;
		
		public ScrollingImages backgrounds;
		public ScrollingImages clouds1;
		public ScrollingImages clouds2;
		public Image[] SkyImages;

		public Action OnAreaChange;
		
		public Area CurrentArea { get; private set; }

		[SerializeField] private QuestManager _quests;

		private void Awake() {
			Instance = this;

			_quests.QuestStarted += q => SetArea(q.Config.Area);
		}

		public void SetArea( Area area ) {
			var oldArea = CurrentArea;
			CurrentArea = area;

            backgrounds.SetImages(area.Background);
			clouds1.SetImages(area.Clouds1);
			clouds2.SetImages(area.Clouds2);
			SkyImages.ForEach(i => i.sprite = area.Sky);

			if (oldArea != area)
			{
				OnAreaChange?.Invoke();
			}
		}


		[SerializeField] private Area _debugArea;
	    [ContextMenu("Set Area")]
	    private void SetDebugArea() {
	        SetArea(_debugArea);
	    }
	}

}

