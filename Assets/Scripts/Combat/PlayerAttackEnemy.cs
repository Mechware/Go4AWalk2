using System;
using System.Collections;
using System.Collections.Generic;
using CustomEvents;
using G4AW2.Combat;
using G4AW2.Combat.Swiping;
using Sirenix.OdinInspector;
using UnityEngine;
using UnityEngine.Events;

public class PlayerAttackEnemy : MonoBehaviour {
    public Player Player;
    public EnemyDisplay Enemy;
	public UnityEvent SwipingDone;

	public float CritPerPixel;

	public float MaxSwipeTime = 0.25f;

	public SwipeDrawer EnemySwipe;

	public UnityEventSwipe EnemySwipeBroken;

	[ShowInInspector] private float totalCritUsed;
	private bool swiping = false;
	private float swipeTime = 0;

	void Update() {
		if (swiping) {
			swipeTime += Time.deltaTime;
			if (swipeTime > MaxSwipeTime) {
				SwipingDone.Invoke();
			}
		}
	}

    public void OnScreenTap() {
        Enemy.ApplyDamage(Player.GetLightDamage());
    }

    public void OnScreenSwipe(Vector3[] points) {
        
    }

    public void AttackPlayer(int damage) {
        print("Player Attacked with: " + damage + " damage");
        Player.Hit(damage);
    }

	public void OnSwipeStart() {
		swipeTime = 0;
		totalCritUsed = 0;
		swiping = true;
	}

	public void OnSwiping(Vector3[] line) {
		if (!swiping) return;

		if (EnemySwipe.drawer.IsAnimating) {
			if (IsLineBetweenPoints(EnemySwipe.drawer.lr, line[0], line[1])) {
				EnemySwipeBroken.Invoke(EnemySwipe.Swipe);
			}
		}

		float distanceChange = (line[0] - line[1]).magnitude;
		distanceChange = Mathf.Max(distanceChange, 200f*Time.deltaTime);
		
	}

	/// <summary>
	/// Checks if the line renderer intersects with the line between p0 and p1.
	/// </summary>
	/// <param name="lr"></param>
	/// <param name="p0"></param>
	/// <param name="p1"></param>
	/// <returns></returns>
	private static bool IsLineBetweenPoints(LineRenderer lr, Vector2 p0, Vector2 p1) {

		for (int i = 1; i < lr.positionCount; i++) {
			Vector2 p2 = lr.GetPosition(i-1);
			Vector2 p3 = lr.GetPosition(i);

			Vector2 s1 = p1 - p0;
			Vector2 s2 = p3 - p2;

			// lel wtf
			float s = (-s1.y * (p0.x - p2.x) + s1.x * (p0.y - p2.y)) / (-s2.x * s1.y + s1.x * s2.y);
			float t = (s2.x * (p0.y - p2.y) - s2.y * (p0.x - p2.x)) / (-s2.x * s1.y + s1.x * s2.y);

			if (s >= 0f && s <= 1f && t >= 0f && t <= 1f) {
				return true;
			}
		}
		return false;
	}
	
}
