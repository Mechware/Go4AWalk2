using System;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.Combat;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Random = UnityEngine.Random;

namespace G4AW2.Followers {

	[RequireComponent(typeof(Animator))]
	public class FollowerDisplay : MonoBehaviour, IPointerClickHandler {
		[NonSerialized] public FollowerInstance Instance;

		public AnimatorOverrideController BaseController;
        public TextMeshProUGUI followerID;

		public Action<FollowerDisplay> FollowerClicked;
		private Animator Animator;

		private AnimatorOverrideController aoc;

		private float currentTime = 0;
		private float doRandomTime = 0;

		void Awake() {
			Animator = GetComponent<Animator>();
			aoc = new AnimatorOverrideController(BaseController);
			Animator.runtimeAnimatorController = aoc;

			if (Instance != null) {
				aoc["Idle"] = Instance.Config.SideIdleAnimation;
				if (Instance.Config.HasRandomAnimation)
					aoc["Random"] = Instance.Config.RandomAnimation;
			}
			
		}
		void Update() {
			if (Instance.Config.HasRandomAnimation) {
				currentTime += Time.deltaTime;
				if (currentTime > doRandomTime) {
					currentTime = 0;
					Animator.SetTrigger("Random");
					doRandomTime = Random.Range(Instance.Config.MinTimeBetweenRandomAnims, Instance.Config.MaxTimeBetweenRandomAnims);
				}
			}
		}

		public void SetData(FollowerInstance instance) {
			Instance = instance;

			aoc["Idle"] = Instance.Config.SideIdleAnimation;
			if (Instance.Config.HasRandomAnimation) {
				aoc["Random"] = Instance.Config.RandomAnimation;
				doRandomTime = Random.Range(Instance.Config.MinTimeBetweenRandomAnims,
					Instance.Config.MaxTimeBetweenRandomAnims);
				currentTime = 0;
			}

			if (Instance is EnemyInstance) {
				EnemyInstance e = (EnemyInstance) instance;
				followerID.text = $"LVL {e.SaveData.Level}\n{e.Config.DisplayName}";
			}
			else {
				followerID.text = Instance.Config.DisplayName;
			}
		}

		public void OnPointerClick(PointerEventData eventData) {
			FollowerClicked.Invoke(this);
		}
	}

}

