using G4AW2.Data.Combat;
using System;
using System.Collections;
using CustomEvents;
using G4AW2.Combat.Swiping;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Combat {
	[RequireComponent(typeof(Animator))]
    public class EnemyDisplay : MonoBehaviour {

		public EnemyData Enemy;
	    public IntReference Level;

		public IntReference MaxHealth;
	    public IntReference CurrentHealth;

		public FloatReference TimeBetweenLightAttacks;
	    public FloatReference TimeBetweenHeavyAttacks;
	    public IntReference HeavyDamage;

        // Events
        public UnityEventSwipe OnSwipeBegin;
        public UnityEventInt OnSwipeHit;
		public UnityEvent OnAttackBroken;
	    public UnityEventInt OnLightAttack;
	    public UnityEvent OnDeath;
	    public UnityEvent OnStun;
	    public UnityEvent OnUnStun;
		public UnityEventInt OnHit;

		private bool isDead = false;

	    void Start() {
		    if (Enemy != null) {
			    SetEnemy(Enemy, Level);
		    }
	    }

		public void SetEnemy( EnemyData data, int level ) {
			Enemy = data;
			Level.Value = level;

			MaxHealth.Value = data.GetHealth(level);
			CurrentHealth.Value = MaxHealth;
			HeavyDamage.Value = data.GetHeavyDamage(level);
			TimeBetweenLightAttacks.Value = data.GetTimeBetweenLightAttacks(level);
			TimeBetweenHeavyAttacks.Value = data.GetTimeBetweenHeavyAttacks(level);

			StopAllCoroutines();
			//StartCoroutine(Attack());
			StartCoroutine(DoSwipingAttack());

			AnimatorOverrideController aoc = (AnimatorOverrideController)GetComponent<Animator>().runtimeAnimatorController;
			aoc["Death"] = Enemy.Death;
			aoc["Dead"] = Enemy.Dead;
			aoc["Flinch"] = Enemy.Flinch;
			aoc["LightAttack"] = Enemy.LightAttack;
			aoc["BeforeSwipeAttack"] = Enemy.BeforeSwipeAttack;
			aoc["SwipeAttack"] = Enemy.SwipeAttack;
			aoc["Idle"] = Enemy.Idle;
		}

		public void Stun() {
			StopAllCoroutines();
			OnStun.Invoke();
		}

		public void UnStun() {
			StartCoroutine(DoSwipingAttack());
			OnUnStun.Invoke();
		}

		#region Attack

		public IEnumerator Attack() {
			for (; ; ) {
				yield return new WaitForSeconds(TimeBetweenLightAttacks);
				if (isDead)
					break;
				OnLightAttack.Invoke(HeavyDamage);
			}
		}

		private bool swipeBroken = false;
		public IEnumerator DoSwipingAttack() {
			for (; ; ) {
				yield return new WaitForSeconds(TimeBetweenHeavyAttacks);
				if (isDead)
					break;
				Swipe swipe = Enemy.Swipes.GetSwipe(Level);
				OnSwipeBegin.Invoke(swipe);
				swipeBroken = false;
				yield return new WaitForSeconds(swipe.GetEntireSwipeTime());
				if(!swipeBroken) SwipeCompleted(swipe);
			}
		}

		public void SwipeBroken() {
			swipeBroken = true;
			OnAttackBroken.Invoke();
		}

		public void SwipeCompleted( Swipe s ) {
			OnSwipeHit.Invoke(HeavyDamage);
		}

		public void ApplyDamage( int amount ) {
			if (isDead)
				return;

			CurrentHealth.Value -= amount;
			if (CurrentHealth.Value <= 0) {
				isDead = true;
				OnDeath.Invoke();
			} else {
				OnHit.Invoke(amount);
			}
		}

		#endregion


#if UNITY_EDITOR
		[ContextMenu("Reload Enemy")]
		public void ReloadLevel() {
			SetEnemy(Enemy, Level);
		}
#endif
	}
}

