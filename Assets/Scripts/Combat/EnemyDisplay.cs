using G4AW2.Data.Combat;
using System;
using System.Collections;
using CustomEvents;
using G4AW2.Events;
using UnityEngine;
using UnityEngine.Events;

namespace G4AW2.Combat {
	[RequireComponent(typeof(Animator))]
    public class EnemyDisplay : MonoBehaviour {

		public enum State {
			Idle, BeforeAttack, ExecuteAttack, AfterAttack, Stun, Disabled
		}

        public FloatReference StunDuration;

		public State EnemyState;
		public EnemyData Enemy;
	    public IntReference Level;

		public IntReference MaxHealth;
	    public IntReference CurrentHealth;

	    public FloatReference TimeBetweenHeavyAttacks;
		public FloatReference AttackPrepTime;
		public FloatReference AttackExecuteDuration;
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
        public UnityEvent OnStartWalking;

		private bool isDead = false;
	    private Animator MyAnimator;

	    void Awake() {
	        MyAnimator = GetComponent<Animator>();
        }

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
            AttackPrepTime.Value = data.AttackPrepTime;
            AttackExecuteDuration.Value = data.AttackExecuteTime;

			AnimatorOverrideController aoc = (AnimatorOverrideController)GetComponent<Animator>().runtimeAnimatorController;
			aoc["Death"] = Enemy.Death;
			aoc["Dead"] = Enemy.Dead;
			aoc["Flinch"] = Enemy.Flinch;
			aoc["BeforeAttack"] = Enemy.BeforeAttack;
			aoc["AttackExecute"] = Enemy.AttackExecute;
			aoc["AfterAttack"] = Enemy.AfterAttack;
			aoc["Idle"] = Enemy.Idle;
            aoc["Walking"] = Enemy.Walking;


            // I wish there was a better way to do this
            Vector3 pos = transform.localPosition;
            pos.x = -70;
            transform.localPosition = pos;

		    Vector2 r = ((RectTransform) transform).sizeDelta;
		    r.x = data.SizeOfSprite.x;
		    r.y = data.SizeOfSprite.y;
		    ((RectTransform) transform).sizeDelta = r;
        }

        public void StartWalking()
        {
            StopAllCoroutines();
            OnStartWalking.Invoke();
            MyAnimator.SetTrigger("Walking");
        }

		public void StartAttacking() {
			StopAllCoroutines();
			StartCoroutine(DoAttack());
		}

		public void Stun() {
			StopAllCoroutines();
			OnStun.Invoke();
		    MyAnimator.SetTrigger("Stun");
        }

        public void UnStun() {
			StartCoroutine(DoAttack());
			OnUnStun.Invoke();
            MyAnimator.SetTrigger("StunOver");
        }

        #region Attack

        private bool attackBroken = false;
		private bool canParry = false;

		public IEnumerator DoAttack() {
			for (; ; ) {
				EnemyState = State.Idle;
				yield return new WaitForSeconds(TimeBetweenHeavyAttacks);
				EnemyState = State.BeforeAttack;

				if (isDead)
					break;

				OnAttackBegin.Invoke();
			    MyAnimator.SetTrigger("AttackStart");
                attackBroken = false;
				canParry = false;

				// Wind up
				yield return new WaitForSeconds(AttackPrepTime);
				EnemyState = State.ExecuteAttack;

				OnAttackExecute.Invoke();
			    MyAnimator.SetTrigger("AttackExecute");
				canParry = true;

                // Perform the attack
                yield return new WaitForSeconds(AttackExecuteDuration);
				EnemyState = State.AfterAttack;
				canParry = false;
				OnAttackHit.Invoke(HeavyDamage);
			    MyAnimator.SetTrigger("AttackEnd");
			}
        }

		public bool AttemptedParry() {
			if(canParry) {
				attackBroken = true;
                Stun();
                Timer.StartTimer(this, StunDuration, () =>
                {
                    UnStun();
                });

                OnAttackParried.Invoke();
                return true;
			}
            return false;
		}

		public void ApplyDamage( int amount ) {
			if (isDead)
				return;

			CurrentHealth.Value -= amount;
			if (CurrentHealth.Value <= 0) {
				isDead = true;
				OnDeath.Invoke(Enemy);
                MyAnimator.SetTrigger("Death");
				OnDropLoot.Invoke(Enemy.Drops.GetItems(true));
			} else {
				OnHit.Invoke(amount);
			    MyAnimator.SetTrigger("Flinch");
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

