using G4AW2.Data.Combat;
using System;
using System.Collections;
using CustomEvents;
using G4AW2.Combat.Swiping;
using G4AW2.Events;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Combat {
	[RequireComponent(typeof(Animator))]
    public class EnemyDisplay : MonoBehaviour {

		public enum State {
			Idle, BeforeAttack, ExecuteAttack, AfterAttack, Stun, Disabled
		}

		public State EnemyState;
		public EnemyData Enemy;
	    public IntReference Level;

		public IntReference MaxHealth;
	    public IntReference CurrentHealth;

	    public FloatReference TimeBetweenHeavyAttacks;
		public FloatReference AttackPrepTime;
		public FloatReference AttackExecuteDuration;
		public FloatReference AfterAttackWaitTime;
		public IntReference HeavyDamage;

        // Events
        public UnityEvent OnAttackBegin;
        public UnityEvent OnAttackExecute;
		public UnityEventInt OnAttackHit;
	    public UnityEvent OnAttackParried;
		public UnityEventEnemyData OnDeath;
	    public UnityEventIEnumerableLoot OnDropLoot;
		public UnityEvent OnStun;
	    public UnityEvent OnUnStun;
		public UnityEventInt OnHit;

		private bool isDead = false;

	    void Start() {
			EnemyState = State.Disabled;

			if (Enemy != null) {
			    SetEnemy(Enemy, Level);
		    }
	    }

		public void SetEnemy(EnemyData data) {
			SetEnemy(data, 1);
		}

		public void SetEnemy( EnemyData data, int level ) {
            isDead = false;
			Enemy = data;
			Level.Value = level;

			MaxHealth.Value = data.GetHealth(level);
			CurrentHealth.Value = MaxHealth;
			HeavyDamage.Value = data.GetHeavyDamage(level);
			TimeBetweenHeavyAttacks.Value = data.GetTimeBetweenHeavyAttacks(level);

			AnimatorOverrideController aoc = (AnimatorOverrideController)GetComponent<Animator>().runtimeAnimatorController;
			aoc["Death"] = Enemy.Death;
			aoc["Dead"] = Enemy.Dead;
			aoc["Flinch"] = Enemy.Flinch;
			aoc["BeforeAttack"] = Enemy.BeforeAttack;
			aoc["AttackExecute"] = Enemy.AttackExecute;
			aoc["AfterAttack"] = Enemy.AttackExecute;
			aoc["Idle"] = Enemy.Idle;
		}

		public void StartAttacking() {
			StopAllCoroutines();
			StartCoroutine(DoAttack());
		}

		public void Stun() {
			StopAllCoroutines();
			OnStun.Invoke();
		}

		public void UnStun() {
			StartCoroutine(DoAttack());
			OnUnStun.Invoke();
		}

		#region Attack

		private bool attackBroken = false;
		private bool canParry = false;

		public IEnumerator DoAttack() {
			Animator animator = GetComponent<Animator>();
			for (; ; ) {
				EnemyState = State.Idle;
				yield return new WaitForSeconds(TimeBetweenHeavyAttacks);
				EnemyState = State.BeforeAttack;

				if (isDead)
					break;

				OnAttackBegin.Invoke();
				attackBroken = false;
				canParry = false;

				// Wind up
				yield return new WaitForSeconds(AttackPrepTime);
				EnemyState = State.ExecuteAttack;

				OnAttackExecute.Invoke();
				canParry = true;

				// Perform the attack
				yield return new WaitForSeconds(AttackExecuteDuration);
				EnemyState = State.AfterAttack;
				canParry = false;
				OnAttackHit.Invoke(HeavyDamage);

				// Wait for a bit (let dust settle)
				yield return new WaitForSeconds(AfterAttackWaitTime);
			}
		}

		public void AttemptedParry() {
			if(canParry) {
				attackBroken = true;
				OnAttackParried.Invoke();
			}
		}

		public void ApplyDamage( int amount ) {
			if (isDead)
				return;

			CurrentHealth.Value -= amount;
			if (CurrentHealth.Value <= 0) {
				isDead = true;
				OnDeath.Invoke(Enemy);
				OnDropLoot.Invoke(Enemy.Drops.GetItems());
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

