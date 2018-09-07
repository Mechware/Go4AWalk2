using CustomEvents;
using G4AW2.Combat.Swiping;
using G4AW2.Utils;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Combat {

	public class PlayerAttackEnemy : MonoBehaviour {
		public Player Player;
		public EnemyDisplay Enemy;

		public float MaxSwipeTime = 0.25f;

		public SwipeDrawer EnemySwipe;

		public UnityEvent PlayerSwipingDone;
		public UnityEventSwipe EnemySwipeBroken;

		private bool swiping = false;
		private float swipeTime = 0;

		void Update() {
			SwipeUpdate();
		}

		public void AttackPlayer( int damage ) {
			print("Player Attacked with: " + damage + " damage");
			Player.Hit(damage);
		}

		#region Swiping
		private void SwipeUpdate() {
			if (swiping) {
				swipeTime += Time.deltaTime;
				if (swipeTime > MaxSwipeTime) {
					PlayerSwipingDone.Invoke();
				}
			}
		}

		public void OnScreenTap() {
			Enemy.ApplyDamage(Player.GetLightDamage());
		}

		public void OnScreenSwipe( Vector3[] points ) {
			swiping = false;
		}

		public void OnSwipeStart() {
			swipeTime = 0;
			swiping = true;
		}

		public void OnSwiping( Vector3[] line ) {
			if (!swiping)
				return;

			if (EnemySwipe.drawer.IsAnimating) {
				if (LineUtils.IsLineBetweenPoints(EnemySwipe.drawer.lr, line[0], line[1])) {
					EnemySwipeBroken.Invoke(EnemySwipe.Swipe);
				}
			}

			float distanceChange = (line[0] - line[1]).magnitude;
			if (distanceChange > 600) {
				PlayerSwipingDone.Invoke();
			}
		}
		#endregion
	}

}

