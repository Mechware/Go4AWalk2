using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Data;
using G4AW2.Data.Combat;
using UnityEditor.Animations;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace G4AW2.Followers {

	[RequireComponent(typeof(Animator))]
	public class FollowerDisplay : MonoBehaviour, IPointerClickHandler {
		private FollowerData Data;

		public AnimatorController BaseController;

		public UnityEventFollowerData FollowerClicked;
		private Animator Animator;

		private AnimatorOverrideController aoc;

		private float currentTime = 0;
		private float doRandomTime = 0;

		void Awake() {
			Animator = GetComponent<Animator>();
			aoc = new AnimatorOverrideController(BaseController);
			Animator.runtimeAnimatorController = aoc;

			if (Data != null) {
				aoc["Idle"] = Data.SittingAnimation;
				if (Data.HasRandomAnimation)
					aoc["Random"] = Data.RandomAnimation;
			}
			
		}

		void Update() {
			if (Data.HasRandomAnimation) {
				currentTime += Time.deltaTime;
				if (currentTime > doRandomTime) {
					currentTime = 0;
					Animator.SetTrigger("Random");
					doRandomTime = Random.Range(Data.MinTimeBetweenRandomAnims, Data.MaxTimeBetweenRandomAnims);
				}
			}
		}

		public void SetData(FollowerData data) {
			Data = data;
			print("Setting data: " + Data.name);

			aoc["Idle"] = Data.SittingAnimation;
			if (Data.HasRandomAnimation) {
				aoc["Random"] = Data.RandomAnimation;
				doRandomTime = Random.Range(Data.MinTimeBetweenRandomAnims, Data.MaxTimeBetweenRandomAnims);
			}
		}

		public void OnPointerClick(PointerEventData eventData) {
			FollowerClicked.Invoke(Data);
		}
	}

}

