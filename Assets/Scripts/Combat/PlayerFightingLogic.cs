using G4AW2.Combat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFightingLogic : MonoBehaviour {

	public Player player;
	public EnemyDisplay ed;
	public float minSwipeDistance;

	private bool blocking = false;
	private bool perfectBlock = false;
	private bool goodParry = false;
	private bool badParry = false;

	public void OnEnemyHitPlayer(int damage) {
		if (blocking)
			damage = damage / 2;

		if (perfectBlock)
			damage = 0;

		player.Health.Value -= damage;
	}

	public void OnPlayerTappingEnemy() {
		ed.ApplyDamage(player.Damage);
	}

	public void OnPlayerFinishSwipe( Vector3[] swipe ) {
		Vector3 start = swipe[0];
		Vector3 end = swipe[swipe.Length - 1];
		if ((start - end).magnitude > minSwipeDistance) {
			if(start.x > end.x) {
				PlayerParried();
			} else {
				PlayerBlocked();
			}
		}
	}

	public void PlayerBlocked() {
		if (blocking || goodParry || badParry)
			return;

		blocking = true;

		if (ed.EnemyState == EnemyDisplay.State.BeforeAttack) {
			perfectBlock = true;
		}
	}

	public void PlayerParried() {
		if (perfectBlock || blocking || goodParry || badParry) {
			return;
		}

		ed.AttemptedParry();
	}
}
